using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class PhotographerPathState
{
    private readonly ushort[] _tileIndices;

    public PhotographerPathState(int pointCapacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pointCapacity);
        _tileIndices = new ushort[pointCapacity];
    }

    public int StartGridIndex { get; private set; } = -1;
    public int TargetGridIndex { get; private set; } = -1;
    public int PointCount { get; private set; }
    public int PointCapacity => _tileIndices.Length;
    public int PointIndex { get; private set; }
    public int StartX { get; private set; }
    public int StartY { get; private set; }
    public int TargetX { get; private set; }
    public int TargetY { get; private set; }
    public int DistanceOnSegment { get; private set; }
    public bool HasCurrentPoint => PointCount > 0 && PointIndex < PointCount;
    public bool IsDirectPath =>
        PointCount is 2 && StartGridIndex == _tileIndices[0] && TargetGridIndex == _tileIndices[1];

    public void Set(
        int startGridIndex,
        int targetGridIndex,
        ReadOnlySpan<ushort> tileIndices,
        int startX,
        int startY,
        int targetX,
        int targetY
    )
    {
        if (tileIndices.Length > _tileIndices.Length)
            throw new ArgumentOutOfRangeException(
                nameof(tileIndices),
                "The path exceeds its native point capacity."
            );

        _tileIndices.AsSpan().Clear();
        tileIndices.CopyTo(_tileIndices);
        StartGridIndex = startGridIndex;
        TargetGridIndex = targetGridIndex;
        PointCount = tileIndices.Length;
        PointIndex = 0;
        StartX = startX;
        StartY = startY;
        TargetX = targetX;
        TargetY = targetY;
        DistanceOnSegment = 0;
    }

    public void Advance(
        int maximumStepLength,
        int gridWidth,
        GameObjectState photographer,
        bool stopAtNextPoint,
        bool forceFacingUpdate
    )
    {
        if (maximumStepLength < 1 || PointCount < 1 || PointIndex >= PointCount)
            return;

        var oldX = photographer.PositionX;
        var oldY = photographer.PositionY;
        var remaining = maximumStepLength;
        var point = GetPoint(PointIndex, gridWidth);

        while (PointIndex < PointCount - 1)
        {
            var nextPoint = GetPoint(PointIndex + 1, gridWidth);
            var deltaX = long.CreateChecked(nextPoint.First) - point.First;
            var deltaY = long.CreateChecked(nextPoint.Second) - point.Second;
            var segmentLength = checked(
                long.CreateChecked(
                    IntegerMath.GetSquareRoot64(
                        checked(ulong.CreateChecked(deltaX * deltaX + deltaY * deltaY))
                    )
                )
            );
            var candidateDistance = checked(long.CreateChecked(DistanceOnSegment) + remaining);

            if (candidateDistance < segmentLength)
            {
                DistanceOnSegment = checked(int.CreateChecked(candidateDistance));
                point = Interpolate(PointIndex, DistanceOnSegment, segmentLength, gridWidth);
                remaining = 0;
                break;
            }

            PointIndex++;
            DistanceOnSegment = 0;
            remaining = checked(int.CreateChecked(candidateDistance - segmentLength));
            point = nextPoint;

            if (PointIndex >= PointCount - 1 || stopAtNextPoint)
            {
                ClearActivePath();
                break;
            }

            if (remaining <= 0)
                break;
        }

        photographer.MoveTo(point.First, point.Second);
        UpdateFacing(photographer, oldX, oldY, point, forceFacingUpdate);
    }

    private IntPair Interpolate(int index, int distance, long length, int gridWidth)
    {
        var point0 = GetPoint(index - 1, gridWidth);
        var point1 = GetPoint(index, gridWidth);
        var point2 = GetPoint(index + 1, gridWidth);
        var point3 = GetPoint(index + 2, gridWidth);
        return new IntPair(
            InterpolateCoordinate(
                point0.First,
                point1.First,
                point2.First,
                point3.First,
                distance,
                length
            ),
            InterpolateCoordinate(
                point0.Second,
                point1.Second,
                point2.Second,
                point3.Second,
                distance,
                length
            )
        );
    }

    private static int InterpolateCoordinate(
        int point0,
        int point1,
        int point2,
        int point3,
        long distance,
        long length
    )
    {
        var term0 = checked(2L * point1);
        var term1 = checked((long.CreateChecked(point2) - point0) * distance / length);
        var term2 = checked(
            checked(checked(2L * point0 - 5L * point1 + 4L * point2 - point3) * distance / length)
            * distance
            / length
        );
        var term3 = checked(
            checked(
                checked(
                    checked(-1L * point0 + 3L * point1 - 3L * point2 + point3) * distance / length
                )
                * distance
                / length
            )
            * distance
            / length
        );
        return checked(int.CreateChecked((term0 + term1 + term2 + term3) / 2));
    }

    private IntPair GetPoint(int index, int gridWidth)
    {
        var lastIndex = PointCount - 1;
        var clampedIndex = Math.Clamp(index, 0, lastIndex);

        if (clampedIndex is 0)
            return new IntPair(StartX, StartY);
        if (clampedIndex == lastIndex)
            return new IntPair(TargetX, TargetY);

        var encodedIndex = _tileIndices[clampedIndex];
        var y = encodedIndex / gridWidth;
        var x = encodedIndex - y * gridWidth;
        return new IntPair(
            checked(x * GameObjectState.TileSize + GameObjectState.TileCenter),
            checked(y * GameObjectState.TileSize + GameObjectState.TileCenter)
        );
    }

    private void ClearActivePath()
    {
        _tileIndices.AsSpan(0, PointCount).Clear();
        PointCount = 0;
        PointIndex = 0;
        StartX = 0;
        StartY = 0;
        TargetX = 0;
        TargetY = 0;
        DistanceOnSegment = 0;
    }

    private static void UpdateFacing(
        GameObjectState photographer,
        int oldX,
        int oldY,
        IntPair position,
        bool forceFacingUpdate
    )
    {
        var facingMetric = checked(position.First - oldX - (position.Second - oldY));

        if (facingMetric is 0 || (!forceFacingUpdate && Math.Abs(facingMetric) < 11))
            return;

        photographer.SetMirrored(facingMetric < 0);
    }
}
