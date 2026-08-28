using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed partial class AmbientAnimalState
{
    private readonly int _speedMultiplier;
    private readonly int _configuredMinimumX;
    private readonly int _configuredMaximumX;
    private readonly int _configuredMinimumY;
    private readonly int _configuredMaximumY;
    private readonly int _birdExtraTiles;
    private readonly GameObjectState? _primarySource;
    private IReadOnlyList<AmbientAnimalSpawnerPoint> _avoidancePoints = [];
    private IReadOnlyList<AmbientAnimalSpawnerPoint> _attractionPoints = [];
    private IReadOnlyList<AmbientAnimalSpawnerPoint> _landingPoints = [];
    private int _avoidanceScanCounter = 2;
    private int _attractionScanCounter = 4;
    private int _landingScanCounter;
    private int _minimumX;
    private int _maximumX;
    private int _minimumY;
    private int _maximumY;
    private int _effectivePositionX;
    private int _effectivePositionY;
    private bool _initialized;
    private bool _hasAvoidanceTarget;
    private bool _isInsideAttractionTarget;
    private bool _isInsideLandingTarget;
    private bool _redirectRefreshPending;

    private AmbientAnimalState(
        GameObjectState gameObject,
        int behavior,
        int speedMultiplier,
        int minimumX,
        int maximumX,
        int minimumY,
        int maximumY,
        int birdExtraTiles,
        GameObjectState? primarySource,
        GameRandom constructorRandom
    )
    {
        GameObject = gameObject;
        Behavior = behavior;
        this._speedMultiplier = speedMultiplier;
        _configuredMinimumX = minimumX;
        _configuredMaximumX = maximumX;
        _configuredMinimumY = minimumY;
        _configuredMaximumY = maximumY;
        this._birdExtraTiles = birdExtraTiles;
        this._primarySource = primarySource;

        var initialSpeed = behavior switch
        {
            0 or 2 => constructorRandom.NextInt(12) + 20,
            1 => constructorRandom.NextInt(12) + 80,
            3 or 4 => constructorRandom.NextInt(12) + 26,
            _ => throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {behavior}."
                )
            ),
        };

        Heading = constructorRandom.NextInt(360) << 3;
        Altitude = behavior is 0 ? 160 : 8;
        Speed = initialSpeed * speedMultiplier / 100;
        CachedAvoidanceIndex = -1;
    }

    public GameObjectState GameObject { get; }
    public int Behavior { get; }
    public int Heading { get; private set; }
    public int SteeringState { get; private set; }
    public int Altitude { get; private set; }
    public int Speed { get; private set; }
    public int MovementTimer { get; private set; }
    public int SpeedChangeTimer { get; private set; }
    public int AltitudeStepChangeTimer { get; private set; }
    public int PhaseTimer { get; private set; }
    public int HeadingStep { get; private set; }
    public int AltitudeStep { get; private set; }
    public int AvoidanceX { get; private set; }
    public int AvoidanceY { get; private set; }
    public int LandingX { get; private set; }
    public int LandingY { get; private set; }
    public int AttractionX { get; private set; }
    public int AttractionY { get; private set; }
    public int CleanupDriftX { get; private set; }
    public int CleanupDriftY { get; private set; }
    public int AvoidanceLinger { get; private set; }
    public int CachedAvoidanceIndex { get; private set; }
    public int RedirectCount { get; private set; }
    public int MirrorTimer { get; private set; }
    public int MovementX { get; private set; }
    public int MovementY { get; private set; }
    public bool IsRemoved { get; private set; }
    public bool WasInsideLandingTarget { get; private set; }
    public bool HasAttractionTarget { get; private set; }
    public bool ZoneCleanup { get; private set; }
    public sbyte MovementState { get; private set; }
    public int DestinationX { get; private set; }
    public int DestinationY { get; private set; }

    public static AmbientAnimalState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom
    )
    {
        if (
            !dataTableResolver.TryResolve(
                GameAssetFiles.AmbientAnimalSpawners,
                "AmbientAnimalSpawner",
                out var spawnerData
            )
        )
            throw new InvalidDataException("Unable to resolve AmbientAnimalSpawner.");

        var (spawnMinimumX, spawnMaximumX, spawnMinimumY, spawnMaximumY) = ResolveSpawnerBounds(
            dataTableResolver,
            spawnerData.GlobalId
        );

        if (
            !dataTableResolver.TryResolveInt(
                spawnerData.GlobalId,
                "BirdExtraTiles",
                out var birdExtraTiles
            )
        )
            throw new InvalidDataException("AmbientAnimalSpawner has no BirdExtraTiles value.");

        var animals = gameObjects
            .Where(static gameObject => gameObject.Data.TableId is 45)
            .Select(gameObject =>
                Create(
                    gameObject,
                    dataTableResolver,
                    spawnMinimumX,
                    spawnMaximumX,
                    spawnMinimumY,
                    spawnMaximumY,
                    birdExtraTiles,
                    gameObjects.FirstOrDefault(static candidate => candidate.Data.TableId is 2),
                    constructorRandom
                )
            )
            .ToArray();

        return animals;
    }

    private static (int MinimumX, int MaximumX, int MinimumY, int MaximumY) ResolveSpawnerBounds(
        DataTableResolver dataTableResolver,
        int spawnerGlobalId
    )
    {
        return (
            ResolveSpawnerCoordinate(dataTableResolver, spawnerGlobalId, "EdgeSpawnTileX", 0),
            ResolveSpawnerCoordinate(dataTableResolver, spawnerGlobalId, "EdgeSpawnTileX", 1),
            ResolveSpawnerCoordinate(dataTableResolver, spawnerGlobalId, "EdgeSpawnTileY", 2),
            ResolveSpawnerCoordinate(dataTableResolver, spawnerGlobalId, "EdgeSpawnTileY", 3)
        );
    }

    public static void Update(
        AmbientAnimalState[] animals,
        GameRandom random,
        Action<AmbientAnimalState, int, int, int[]>? recordUpdate = null
    )
    {
        foreach (var animal in animals)
        {
            var callsBefore = random.Calls;
            List<int>? upperBounds = recordUpdate is null ? null : [];
            random.NextIntObserved = upperBounds is null
                ? null
                : (upperBound, _) => upperBounds.Add(upperBound);
            try
            {
                animal.Update(random);
            }
            finally
            {
                random.NextIntObserved = null;
            }

            recordUpdate?.Invoke(animal, callsBefore, random.Calls, upperBounds?.ToArray() ?? []);
        }
    }

    internal static AmbientAnimalState CreateSpawned(
        int globalId,
        int behavior,
        int x,
        int y,
        AmbientAnimalState template,
        DataTableResolver dataTableResolver,
        GameRandom random,
        int destinationX,
        int destinationY,
        IReadOnlyList<AmbientAnimalSpawnerPoint> avoidancePoints,
        IReadOnlyList<AmbientAnimalSpawnerPoint> attractionPoints,
        IReadOnlyList<AmbientAnimalSpawnerPoint> landingPoints
    )
    {
        var (tableId, dataCount) = ResolveSpawnDataTable(dataTableResolver);
        var dataIndex = random.NextInt(dataCount);

        for (var checkedData = 0; checkedData < dataCount; checkedData++)
        {
            var dataGlobalId = tableId * DataTableResolver.GlobalIdTableSize + dataIndex;
            dataTableResolver.TryResolveInt(dataGlobalId, "Behavior", out var candidateBehavior);

            if (candidateBehavior == behavior)
            {
                return CreateSpawnedFromData(
                    globalId,
                    behavior,
                    x,
                    y,
                    template,
                    dataTableResolver,
                    random,
                    destinationX,
                    destinationY,
                    avoidancePoints,
                    attractionPoints,
                    landingPoints,
                    dataGlobalId
                );
            }

            dataIndex = (dataIndex + 1) % dataCount;
        }

        throw new InvalidDataException(
            string.Create(
                CultureInfo.InvariantCulture,
                $"No ambient-animal data supports behavior {behavior}."
            )
        );
    }

    private static (int TableId, int DataCount) ResolveSpawnDataTable(
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryGetTableId(GameAssetFiles.AmbientAnimals, out var tableId)
            || !dataTableResolver.TryGetTableEntryCount(
                GameAssetFiles.AmbientAnimals,
                out var dataCount
            )
            || dataCount is 0
        )
            throw new InvalidDataException("Ambient-animal data is unavailable.");

        return (tableId, dataCount);
    }

    private static AmbientAnimalState CreateSpawnedFromData(
        int globalId,
        int behavior,
        int x,
        int y,
        AmbientAnimalState template,
        DataTableResolver dataTableResolver,
        GameRandom random,
        int destinationX,
        int destinationY,
        IReadOnlyList<AmbientAnimalSpawnerPoint> avoidancePoints,
        IReadOnlyList<AmbientAnimalSpawnerPoint> attractionPoints,
        IReadOnlyList<AmbientAnimalSpawnerPoint> landingPoints,
        int dataGlobalId
    )
    {
        if (
            !dataTableResolver.TryResolve(dataGlobalId, out var data)
            || !dataTableResolver.TryResolveInt(
                dataGlobalId,
                "SpeedMultiplier",
                out var speedMultiplier
            )
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Ambient-animal data {dataGlobalId} is incomplete."
                )
            );
        }

        var snapshot = new GameObjectSnapshot
        {
            DataGlobalId = dataGlobalId,
            AccurateX = x,
            AccurateY = y,
            Mirrored = false,
        };
        var dimensions = GameObjectDimensionsResolver.Resolve(data, dataTableResolver);
        var gameObject = new GameObjectState(
            globalId,
            snapshot,
            data,
            dimensions.Width,
            dimensions.Height
        );
        var spawned = new AmbientAnimalState(
            gameObject,
            behavior,
            speedMultiplier,
            template._minimumX,
            template._maximumX,
            template._minimumY,
            template._maximumY,
            template._birdExtraTiles,
            template._primarySource,
            random
        );

        spawned.DestinationX = destinationX;
        spawned.DestinationY = destinationY;
        spawned.Heading = IntegerMath.GetVectorAngle(destinationX - x, destinationY - y) << 3;

        spawned.ConfigureSpawnerPoints(avoidancePoints, attractionPoints, landingPoints);
        return spawned;
    }

    public void ConfigureSpawnerPoints(
        IReadOnlyList<AmbientAnimalSpawnerPoint> avoidance,
        IReadOnlyList<AmbientAnimalSpawnerPoint> attraction,
        IReadOnlyList<AmbientAnimalSpawnerPoint> landing
    )
    {
        ArgumentNullException.ThrowIfNull(avoidance);
        ArgumentNullException.ThrowIfNull(attraction);
        ArgumentNullException.ThrowIfNull(landing);
        _avoidancePoints = avoidance;
        _attractionPoints = attraction;
        _landingPoints = landing;
    }

    public void ResetSpawnerPointCache()
    {
        CachedAvoidanceIndex = -1;
    }

    private void InitializeSpawnerBounds()
    {
        if (_initialized)
            return;

        _minimumX = _configuredMinimumX;
        _maximumX = _configuredMaximumX;
        _minimumY = _configuredMinimumY;
        _maximumY = _configuredMaximumY;
        _initialized = true;
    }

    internal void ApplySpawnerZoneCleanup(int horizontalTileExtent, int verticalTileExtent)
    {
        if (Behavior is < 2 or > 4)
        {
            ZoneCleanup = true;
            return;
        }

        _attractionPoints = [];

        if (Behavior is not 2)
            _avoidancePoints = [];

        if (MovementState is 3 && !_redirectRefreshPending)
            return;

        if (Behavior is 1 || MovementState is 4)
            return;

        var position = ResolveAbsolutePosition(GameObject);
        var cleanupX = Behavior is 2
            ? unchecked((horizontalTileExtent << 8) - position.X)
            : unchecked(position.X - (horizontalTileExtent << 8));
        var cleanupY = Behavior is 2
            ? unchecked((verticalTileExtent << 9) - position.Y + 0x1e00)
            : unchecked(position.Y - (verticalTileExtent << 8));
        var length = IntegerMath.GetVectorLength(cleanupX, cleanupY);

        if (length is 0)
        {
            CleanupDriftX = 0x200;
            CleanupDriftY = 0x200;
            return;
        }

        CleanupDriftX = unchecked(cleanupX << 4) / length;
        CleanupDriftY = unchecked(cleanupY << 4) / length;

        if (Behavior is 2)
        {
            CleanupDriftX = unchecked(CleanupDriftX << 1);
            CleanupDriftY = unchecked(CleanupDriftY << 1);
        }
    }

    private static AmbientAnimalState Create(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        int minimumX,
        int maximumX,
        int minimumY,
        int maximumY,
        int birdExtraTiles,
        GameObjectState? primarySource,
        GameRandom constructorRandom
    )
    {
        dataTableResolver.TryResolveInt(gameObject.Data.GlobalId, "Behavior", out var behavior);

        if (
            !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "SpeedMultiplier",
                out var speedMultiplier
            )
        )
        {
            throw new InvalidDataException(
                $"Ambient animal {gameObject.Data.Name} has incomplete movement data."
            );
        }

        return new AmbientAnimalState(
            gameObject,
            behavior,
            speedMultiplier,
            minimumX,
            maximumX,
            minimumY,
            maximumY,
            birdExtraTiles,
            primarySource,
            constructorRandom
        );
    }
}
