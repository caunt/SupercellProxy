using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed partial class AmbientAnimalState
{
    private void UpdateMirroring()
    {
        if (MovementX == MovementY || Behavior is 0 || (Behavior is not 1 && MirrorTimer >= 1))
            return;

        GameObject.SetMirrored(MovementX <= MovementY);
        MirrorTimer = 20;
    }

    private void RefreshAvoidanceTarget()
    {
        var previousCounter = _avoidanceScanCounter;
        _avoidanceScanCounter++;

        if (previousCounter < 5)
            return;

        _avoidanceScanCounter = 0;

        if (CachedAvoidanceIndex >= 0 && CachedAvoidanceIndex < _avoidancePoints.Count)
        {
            var selected = _avoidancePoints[CachedAvoidanceIndex];
            var selectedX = _effectivePositionX - selected.X;
            var selectedY = _effectivePositionY - selected.Y;

            if (
                long.CreateTruncating(selectedX) * selectedX
                    + long.CreateTruncating(selectedY) * selectedY
                < selected.RadiusSquared
            )
            {
                AvoidanceX = selectedX;
                AvoidanceY = selectedY;
                AvoidanceLinger = 8;
                _hasAvoidanceTarget = true;
                return;
            }

            CachedAvoidanceIndex = -1;
        }

        var nearestDistance = 0x40000000L;
        var nearestIndex = -1;

        for (var index = 0; index < _avoidancePoints.Count; index++)
        {
            var point = _avoidancePoints[index];
            var x = _effectivePositionX - point.X;
            var y = _effectivePositionY - point.Y;
            var distance = long.CreateTruncating(x) * x + long.CreateTruncating(y) * y;

            if (distance >= point.RadiusSquared || distance >= nearestDistance)
                continue;

            nearestDistance = distance;
            nearestIndex = index;
            AvoidanceX = x;
            AvoidanceY = y;
            AvoidanceLinger = 8;
        }

        CachedAvoidanceIndex = nearestIndex;
        _hasAvoidanceTarget = nearestIndex >= 0;
    }

    private void RefreshAttractionTarget()
    {
        var previousCounter = _attractionScanCounter;
        _attractionScanCounter++;

        if (previousCounter < 5)
            return;

        _attractionScanCounter = 0;
        HasAttractionTarget = false;
        _isInsideAttractionTarget = false;
        const long selectionDistanceThreshold = 0x40000000;
        var nearestDistance = selectionDistanceThreshold;

        foreach (var point in _attractionPoints)
        {
            var x = point.X - _effectivePositionX;
            var y = point.Y - _effectivePositionY;
            var selectionDistance = long.CreateTruncating(x) * x + long.CreateTruncating(y) * y;

            if (selectionDistance < nearestDistance)
            {
                nearestDistance = selectionDistance;
                AttractionX = x;
                AttractionY = y;
                HasAttractionTarget = true;
            }

            if (selectionDistance < point.RadiusSquared)
                _isInsideAttractionTarget = true;
        }
    }

    private bool ApplyPrimarySourceAvoidance()
    {
        if (_primarySource is null || _primarySource.Snapshot.State is not 2 and not 3)
            return false;

        var animalPosition = ResolveAbsolutePosition(GameObject);
        var sourcePosition = ResolveAbsolutePosition(_primarySource);
        var x = unchecked(animalPosition.X - sourcePosition.X);
        var y = unchecked(animalPosition.Y - sourcePosition.Y);

        if (long.CreateTruncating(x) * x + long.CreateTruncating(y) * y >= 0x400000)
            return false;

        AvoidanceX = x;
        AvoidanceY = y;
        AvoidanceLinger = 8;
        return true;
    }

    private void RefreshLandingTarget()
    {
        var previousCounter = _landingScanCounter;
        _landingScanCounter++;

        if (previousCounter < 5)
            return;

        _landingScanCounter = 0;
        _isInsideLandingTarget = false;

        foreach (var point in _landingPoints)
        {
            if (DestinationX is not 0 || DestinationY is not 0)
            {
                if (point.X != DestinationX || point.Y != DestinationY)
                    continue;
            }

            var x = point.X - (_effectivePositionX + MovementX);
            var y = point.Y - (_effectivePositionY + MovementY);

            if (long.CreateTruncating(x) * x + long.CreateTruncating(y) * y >= point.RadiusSquared)
                continue;

            LandingX = x + 60;
            LandingY = y + 60;
            _isInsideLandingTarget = true;
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

    private static (int X, int Y) ResolveAbsolutePosition(GameObjectState gameObject)
    {
        var x = 0;
        var y = 0;

        for (var current = gameObject; current is not null; current = current.Parent)
        {
            x = unchecked(x + current.PositionX);
            y = unchecked(y + current.PositionY);
        }

        return (x, y);
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
