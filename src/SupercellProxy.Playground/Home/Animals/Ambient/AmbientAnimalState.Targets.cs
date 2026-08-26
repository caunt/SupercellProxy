using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed partial class AmbientAnimalState
{
    private void UpdateMirroring()
    {
        if (MovementX == MovementY || Behavior is 0 || (Behavior is not 1 && ChecksumState16 >= 1))
            return;

        GameObject.SetMirrored(MovementX <= MovementY);
        ChecksumState16 = 20;
    }

    private void RefreshAvoidanceTarget()
    {
        var previousCounter = avoidanceScanCounter;
        avoidanceScanCounter++;

        if (previousCounter < 5)
            return;

        avoidanceScanCounter = 0;

        if (ChecksumState14 >= 0 && ChecksumState14 < avoidancePoints.Count)
        {
            var selected = avoidancePoints[ChecksumState14];
            var selectedX = GameObject.PositionX - selected.X;
            var selectedY = GameObject.PositionY - selected.Y;

            if (
                long.CreateTruncating(selectedX) * selectedX
                    + long.CreateTruncating(selectedY) * selectedY
                < selected.RadiusSquared
            )
            {
                ChecksumState7 = selectedX;
                ChecksumState8 = selectedY;
                ChecksumState13 = 8;
                hasAvoidanceTarget = true;
                return;
            }

            ChecksumState14 = -1;
        }

        var nearestDistance = 0x40000000L;
        var nearestIndex = -1;

        for (var index = 0; index < avoidancePoints.Count; index++)
        {
            var point = avoidancePoints[index];
            var x = GameObject.PositionX - point.X;
            var y = GameObject.PositionY - point.Y;
            var distance = long.CreateTruncating(x) * x + long.CreateTruncating(y) * y;

            if (distance >= point.RadiusSquared || distance >= nearestDistance)
                continue;

            nearestDistance = distance;
            nearestIndex = index;
            ChecksumState7 = x;
            ChecksumState8 = y;
            ChecksumState13 = 8;
        }

        ChecksumState14 = nearestIndex;
        hasAvoidanceTarget = nearestIndex >= 0;
    }

    private void RefreshAttractionTarget()
    {
        var previousCounter = attractionScanCounter;
        attractionScanCounter++;

        if (previousCounter < 5)
            return;

        attractionScanCounter = 0;
        hasAttractionTarget = false;
        ChecksumFlag2 = false;
        isInsideAttractionTarget = false;
        const long SelectionDistanceThreshold = 0x40000000;
        var nearestDistance = SelectionDistanceThreshold;

        foreach (var point in attractionPoints)
        {
            var x = point.X - GameObject.PositionX;
            var y = point.Y - GameObject.PositionY;
            var selectionDistance = long.CreateTruncating(x) * x + long.CreateTruncating(y) * y;

            if (selectionDistance < nearestDistance)
            {
                nearestDistance = selectionDistance;
                TargetY = x;
                ChecksumState10 = y;
                hasAttractionTarget = true;
                ChecksumFlag2 = true;
            }

            if (selectionDistance < point.RadiusSquared)
                isInsideAttractionTarget = true;
        }
    }

    private void RefreshLandingTarget()
    {
        var previousCounter = landingScanCounter;
        landingScanCounter++;

        if (previousCounter < 5)
            return;

        landingScanCounter = 0;
        isInsideLandingTarget = false;

        foreach (var point in landingPoints)
        {
            if (DestinationX is not 0 || DestinationY is not 0)
            {
                if (point.X != DestinationX || point.Y != DestinationY)
                    continue;
            }

            var x = point.X - (GameObject.PositionX + MovementX);
            var y = point.Y - (GameObject.PositionY + MovementY);

            if (long.CreateTruncating(x) * x + long.CreateTruncating(y) * y >= point.RadiusSquared)
                continue;

            ChecksumState9 = x + 60;
            TargetX = y + 60;
            isInsideLandingTarget = true;
            return;
        }
    }

    private void AddNormalizedMovement(int x, int y, int shift)
    {
        var length = IntegerMath.GetVectorLength(x, y);

        if (length is 0)
            return;

        MovementX += (x << shift) / length;
        MovementY += (y << shift) / length;
    }

    private static int ResolveSpawnerCoordinate(
        DataTableResolver dataTableResolver,
        int globalId,
        string fieldName,
        int valueIndex
    )
    {
        if (!dataTableResolver.TryResolveInt(globalId, fieldName, valueIndex, out var value))
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"AmbientAnimalSpawner has no {fieldName}[{valueIndex}] value."
                )
            );

        return checked(value << 9);
    }
}
