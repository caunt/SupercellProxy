using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class SpawnedPhotographerState
{
    private readonly (
        int MovementSpeed,
        int IdleMinimum,
        int IdleMaximum,
        int PhotoMinimum,
        int PhotoMaximum
    )[] _configurations;
    private readonly int _gridHeight;
    private readonly int _gridWidth;
    private readonly PhotographerGridPathFinder _pathFinder;
    private readonly PhotographerTargetResolver _targetResolver;
    private bool _exiting;
    private bool _lifecycleEnabled;
    private PhotographerState? _photographer;

    private SpawnedPhotographerState(
        (
            int MovementSpeed,
            int IdleMinimum,
            int IdleMaximum,
            int PhotoMinimum,
            int PhotoMaximum
        )[] configurations,
        int gridWidth,
        int gridHeight,
        PhotographerGridPathFinder pathFinder,
        PhotographerTargetResolver targetResolver
    )
    {
        this._configurations = configurations;
        _gridWidth = gridWidth;
        _gridHeight = gridHeight;
        _pathFinder = pathFinder;
        _targetResolver = targetResolver;
    }

    public bool Exists { get; private set; }
    public int DataRow { get; private set; } = -1;
    public int TransitUpdatesRemaining { get; private set; }
    public PhotographerStateCode State { get; private set; }
    public int StateUpdatesRemaining { get; private set; }

    public static SpawnedPhotographerState Create(
        DataTableResolver dataTableResolver,
        PhotographerState[] loadedPhotographers,
        IReadOnlyList<IntPair> entryRoute,
        IReadOnlyList<IntPair> exitRoute,
        int gridWidth,
        int gridHeight,
        GameObjectState[] gameObjects,
        InventoryState inventory
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gridWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gridHeight);
        var resolvedConfigurations = ResolveConfigurations(dataTableResolver);
        var state = new SpawnedPhotographerState(
            resolvedConfigurations,
            gridWidth,
            gridHeight,
            new PhotographerGridPathFinder(gameObjects, gridWidth, gridHeight),
            new PhotographerTargetResolver(
                gameObjects,
                gridWidth,
                gridHeight,
                inventory,
                dataTableResolver
            )
        );
        ApplyLoadedPhotographer(
            state,
            loadedPhotographers,
            entryRoute,
            exitRoute,
            resolvedConfigurations
        );
        return state;
    }

    private static (
        int MovementSpeed,
        int IdleMinimum,
        int IdleMaximum,
        int PhotoMinimum,
        int PhotoMaximum
    )[] ResolveConfigurations(DataTableResolver dataTableResolver)
    {
        if (
            !dataTableResolver.TryResolvePhysicalRowCount(
                GameAssetFiles.Photographer,
                out var rowCount
            )
            || rowCount < 1
        )
            throw new InvalidDataException(
                $"{GameAssetFiles.Photographer} contains no photographer configurations."
            );

        var photographerConfigurations = new (
            int MovementSpeed,
            int IdleMinimum,
            int IdleMaximum,
            int PhotoMinimum,
            int PhotoMaximum
        )[rowCount];

        for (var row = 0; row < rowCount; row++)
            photographerConfigurations[row] = ResolveConfiguration(dataTableResolver, row);

        return photographerConfigurations;
    }

    private static (
        int MovementSpeed,
        int IdleMinimum,
        int IdleMaximum,
        int PhotoMinimum,
        int PhotoMaximum
    ) ResolveConfiguration(DataTableResolver dataTableResolver, int row)
    {
        if (
            !dataTableResolver.TryResolveInt(
                GameAssetFiles.Photographer,
                row,
                "WalkSpeed",
                out var movementSpeed
            )
            || !dataTableResolver.TryResolveInt(
                GameAssetFiles.Photographer,
                row,
                "IdleTimeMinMS",
                out var idleMinimum
            )
            || !dataTableResolver.TryResolveInt(
                GameAssetFiles.Photographer,
                row,
                "IdleTimeMaxMS",
                out var idleMaximum
            )
            || !dataTableResolver.TryResolveInt(
                GameAssetFiles.Photographer,
                row,
                "PhotoTimeMinMS",
                out var photoMinimum
            )
            || !dataTableResolver.TryResolveInt(
                GameAssetFiles.Photographer,
                row,
                "PhotoTimeMaxMS",
                out var photoMaximum
            )
            || movementSpeed < 1
            || idleMaximum < idleMinimum
            || photoMaximum < photoMinimum
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Photographer row {row} has invalid native lifecycle data."
                )
            );

        return (movementSpeed, idleMinimum, idleMaximum, photoMinimum, photoMaximum);
    }

    private static void ApplyLoadedPhotographer(
        SpawnedPhotographerState state,
        PhotographerState[] loadedPhotographers,
        IReadOnlyList<IntPair> entryRoute,
        IReadOnlyList<IntPair> exitRoute,
        (
            int MovementSpeed,
            int IdleMinimum,
            int IdleMaximum,
            int PhotoMinimum,
            int PhotoMaximum
        )[] configurations
    )
    {
        if (loadedPhotographers.Length is 0)
            return;

        if (loadedPhotographers.Length is not 1)
            throw new InvalidDataException(
                "The native photographer spawner has multiple loaded photographers."
            );

        var loaded = loadedPhotographers[0];
        state._photographer = loaded;

        if (
            loaded.GameObject.Data.RowIndex < 0
            || loaded.GameObject.Data.RowIndex >= configurations.Length
        )
            throw new InvalidDataException(
                "The loaded photographer has an invalid native data row."
            );

        state.DataRow = loaded.GameObject.Data.RowIndex;
        state.State = loaded.State;
        state.Exists = true;

        if (loaded.State is PhotographerStateCode.StationaryFour)
            return;

        state._lifecycleEnabled = true;

        if (loaded.State is PhotographerStateCode.EntryPath)
        {
            state.ValidateRoute(entryRoute);
            loaded.LifecycleEnabled = true;
            state.BeginPathTo(entryRoute[^1]);
            return;
        }

        if (loaded.State is PhotographerStateCode.TargetPath)
        {
            loaded.LifecycleEnabled = true;
            return;
        }

        if (loaded.State is not PhotographerStateCode.ExitRoute)
            throw new NotSupportedException(
                $"Loaded photographer state {loaded.State} is not implemented yet."
            );

        state.ValidateRoute(exitRoute);
        loaded.LifecycleEnabled = true;
        state.BeginPathTo(exitRoute[^1]);
        state._exiting = true;
    }

    public void Spawn(GameRandom random, IReadOnlyList<IntPair> route)
    {
        if (Exists)
            return;

        if (route.Count < 2)
            throw new InvalidDataException(
                "The native photographer spawn route contains fewer than two points."
            );

        DataRow = random.NextInt(_configurations.Length);
        var distance = 0;

        for (var i = 1; i < route.Count; i++)
        {
            distance = checked(
                distance
                + IntegerMath.GetVectorLength(
                    checked(route[i].First - route[i - 1].First),
                    checked(route[i].Second - route[i - 1].Second)
                )
            );
        }

        TransitUpdatesRemaining = checked(
            (distance + _configurations[DataRow].MovementSpeed - 1)
            / _configurations[DataRow].MovementSpeed
        );
        Exists = true;
        _lifecycleEnabled = true;
    }

    public void Update(GameRandom random)
    {
        if (!Exists || !_lifecycleEnabled)
            return;

        if (_photographer is null)
        {
            UpdateSpawnTransit(random);
            return;
        }

        LifecycleUpdate();
        NormalUpdate(random);
    }

    private void LifecycleUpdate()
    {
        if (_photographer is null)
            return;

        _photographer.LifecycleTimer--;

        if (State is PhotographerStateCode.StationaryThree or PhotographerStateCode.StationaryFour)
            SetStateTimer(StateUpdatesRemaining - 1);
    }

    private void NormalUpdate(GameRandom random)
    {
        if (_photographer is null)
            return;

        _photographer.MovementVector = default;

        switch (State)
        {
            case PhotographerStateCode.EntryPath:
            case PhotographerStateCode.PathStateOne:
            case PhotographerStateCode.TargetPath:
            case PhotographerStateCode.ExitRoute:
                AdvancePathState(random);
                return;
            case PhotographerStateCode.StationaryThree:
            case PhotographerStateCode.StationaryFour:
                if (StateUpdatesRemaining <= 0)
                    EnterTargetPath(random);

                return;
            case PhotographerStateCode.DeparturePath:
                throw new NotSupportedException(
                    "The photographer's departure-path transition is not implemented yet."
                );
            default:
                throw new InvalidOperationException($"Unsupported photographer state {State}.");
        }
    }

    private void AdvancePathState(GameRandom random)
    {
        if (_photographer is null)
            return;

        if (_photographer.Path.HasCurrentPoint)
        {
            var oldFacingRefreshTimer = _photographer.FacingRefreshTimer;
            _photographer.FacingRefreshTimer--;
            _photographer.Path.Advance(
                _configurations[DataRow].MovementSpeed,
                _gridWidth,
                _photographer.GameObject,
                stopAtNextPoint: false,
                forceFacingUpdate: oldFacingRefreshTimer < 1
            );

            if (oldFacingRefreshTimer <= 0)
                _photographer.FacingRefreshTimer = 20;

            if (
                _photographer.Path.HasCurrentPoint
                && _photographer.Path.IsDirectPath
                && _photographer.LifecycleTimer <= 0
            )
                BeginPathTo(
                    new IntPair(_photographer.PendingTargetX, _photographer.PendingTargetY)
                );

            return;
        }

        _photographer.RuntimeStateA = -1;
        _photographer.RuntimeStateB = -1;
        _photographer.MovementVector = default;
        _photographer.CandidateMovementVector = default;
        _photographer.PersistentMovementVector = default;

        if (_exiting)
        {
            _photographer.NextPoint = 0;
            _photographer.PathComplete = true;
            Exists = false;
            return;
        }

        BeginStationaryState(random);
    }

    private void UpdateSpawnTransit(GameRandom random)
    {
        if (TransitUpdatesRemaining <= 0)
            return;

        TransitUpdatesRemaining--;

        if (TransitUpdatesRemaining is not 0)
            return;

        if (_exiting)
        {
            Exists = false;
            return;
        }

        BeginStationaryState(random);
    }

    private void BeginStationaryState(GameRandom random)
    {
        var photograph = random.NextInt(2) is not 0;
        State = photograph
            ? PhotographerStateCode.StationaryFour
            : PhotographerStateCode.StationaryThree;
        if (_photographer is not null)
            _photographer.State = State;
        var configuration = _configurations[DataRow];
        var minimum = photograph ? configuration.PhotoMinimum : configuration.IdleMinimum;
        var maximum = photograph ? configuration.PhotoMaximum : configuration.IdleMaximum;
        var milliseconds = checked(minimum + random.NextInt(maximum - minimum));
        SetStateTimer(checked(checked(milliseconds / 10 * GameTick.TimerUpdatesPerSecond) / 100));

        if (StateUpdatesRemaining < 1)
            throw new InvalidDataException(
                "The photographer's native stationary duration is empty."
            );
    }

    private void EnterTargetPath(GameRandom random)
    {
        State = PhotographerStateCode.TargetPath;

        if (_photographer is null)
            return;

        _photographer.State = State;
        var target = _targetResolver.Resolve(random);

        if (target is IntPair position)
            BeginPathTo(position);
    }

    private void BeginPathTo(IntPair target)
    {
        if (_photographer is null)
            throw new InvalidOperationException("A loaded photographer is required for pathing.");

        var maximumX = checked((_gridWidth - 1) * GameObjectState.TileSize);
        var maximumY = checked((_gridHeight - 1) * GameObjectState.TileSize);
        var targetX = Math.Clamp(target.First, 0, maximumX);
        var targetY = Math.Clamp(target.Second, 0, maximumY);
        var startTileX = _photographer.GameObject.PositionX >> 9;
        var startTileY = _photographer.GameObject.PositionY >> 9;
        var targetTileX = targetX >> 9;
        var targetTileY = targetY >> 9;
        var startGridIndex = checked(_gridWidth * startTileY + startTileX);
        var targetGridIndex = checked(_gridWidth * targetTileY + targetTileX);

        var bothEndpointsInBounds =
            startTileX >= 0
            && startTileY >= 0
            && startTileX < _gridWidth
            && startTileY < _gridHeight
            && targetTileX >= 0
            && targetTileY >= 0
            && targetTileX < _gridWidth
            && targetTileY < _gridHeight;
        var points = bothEndpointsInBounds
            ? _pathFinder.Find(
                startTileX,
                startTileY,
                targetTileX,
                targetTileY,
                _photographer.Path.PointCapacity
            )
            : [ushort.CreateTruncating(startGridIndex), ushort.CreateTruncating(targetGridIndex)];

        _photographer.PendingTargetX = targetX;
        _photographer.PendingTargetY = targetY;
        _photographer.Path.Set(
            startGridIndex,
            targetGridIndex,
            points,
            _photographer.GameObject.PositionX,
            _photographer.GameObject.PositionY,
            targetX,
            targetY
        );

        if (_photographer.Path.IsDirectPath)
            _photographer.LifecycleTimer =
                GameTick.TimerUpdatesPerSecond * _photographer.Path.PointCount;
    }

    private void ValidateRoute(IReadOnlyList<IntPair> route)
    {
        if (_photographer is null || route.Count < 1)
            throw new InvalidDataException("The loaded photographer has an empty native route.");

        if (_photographer.NextPoint < 0 || _photographer.NextPoint > route.Count)
            throw new InvalidDataException("The loaded photographer has an invalid route point.");
    }

    private void SetStateTimer(int value)
    {
        StateUpdatesRemaining = value;

        if (_photographer is not null)
            _photographer.StateTimer = value;
    }
}
