using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed partial class AmbientAnimalState
{
    private readonly int speedMultiplier;
    private readonly int minimumX;
    private readonly int maximumX;
    private readonly int minimumY;
    private readonly int maximumY;
    private readonly int birdExtraTiles;
    private IReadOnlyList<AmbientAnimalSpawnerPoint> avoidancePoints = [];
    private IReadOnlyList<AmbientAnimalSpawnerPoint> attractionPoints = [];
    private IReadOnlyList<AmbientAnimalSpawnerPoint> landingPoints = [];
    private int avoidanceScanCounter = 2;
    private int attractionScanCounter = 4;
    private int landingScanCounter;
    private bool hasAvoidanceTarget;
    private bool hasAttractionTarget;
    private bool isInsideAttractionTarget;
    private bool isInsideLandingTarget;
    private bool redirectRefreshPending;

    private AmbientAnimalState(
        GameObjectState gameObject,
        int behavior,
        int speedMultiplier,
        int minimumX,
        int maximumX,
        int minimumY,
        int maximumY,
        int birdExtraTiles,
        GameRandom constructorRandom
    )
    {
        GameObject = gameObject;
        Behavior = behavior;
        this.speedMultiplier = speedMultiplier;
        this.minimumX = minimumX;
        this.maximumX = maximumX;
        this.minimumY = minimumY;
        this.maximumY = maximumY;
        this.birdExtraTiles = birdExtraTiles;

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
        ChecksumState1 = behavior is 0 ? 160 : 8;
        Speed = initialSpeed * speedMultiplier / 100;
        ChecksumState14 = -1;
    }

    public GameObjectState GameObject { get; }
    public int Behavior { get; }
    public int Heading { get; private set; }
    public int ChecksumState0 { get; private set; }
    public int ChecksumState1 { get; private set; }
    public int Speed { get; private set; }
    public int ChecksumState2 { get; private set; }
    public int ChecksumState3 { get; private set; }
    public int ChecksumState4 { get; private set; }
    public int ChecksumState5 { get; private set; }
    public int HeadingStep { get; private set; }
    public int ChecksumState6 { get; private set; }
    public int ChecksumState7 { get; private set; }
    public int ChecksumState8 { get; private set; }
    public int ChecksumState9 { get; private set; }
    public int TargetX { get; private set; }
    public int TargetY { get; private set; }
    public int ChecksumState10 { get; private set; }
    public int ChecksumState11 { get; private set; }
    public int ChecksumState12 { get; private set; }
    public int ChecksumState13 { get; private set; }
    public int ChecksumState14 { get; private set; }
    public int ChecksumState15 { get; private set; }
    public int ChecksumState16 { get; private set; }
    public int MovementX { get; private set; }
    public int MovementY { get; private set; }
    public bool ChecksumFlag0 { get; private set; }
    public bool ChecksumFlag1 { get; private set; }
    public bool ChecksumFlag2 { get; private set; }
    public bool ChecksumFlag3 { get; private set; }
    public sbyte ChecksumByte0 { get; private set; }
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
                "data/ambient_animal_spawners.csv",
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

    public static void Update(AmbientAnimalState[] animals, GameRandom random)
    {
        foreach (var animal in animals)
            animal.Update(random);
    }

    internal static AmbientAnimalState CreateSpawned(
        int globalId,
        int behavior,
        int x,
        int y,
        int homeTileMapWidth,
        int homeTileMapHeight,
        AmbientAnimalState template,
        DataTableResolver dataTableResolver,
        GameRandom random,
        bool headTowardHomeCenter,
        IReadOnlyList<AmbientAnimalSpawnerPoint> avoidancePoints,
        IReadOnlyList<AmbientAnimalSpawnerPoint> attractionPoints,
        IReadOnlyList<AmbientAnimalSpawnerPoint> landingPoints
    )
    {
        const string ambientAnimalsFile = "data/ambient_animals.csv";

        if (
            !dataTableResolver.TryGetTableId(ambientAnimalsFile, out var tableId)
            || !dataTableResolver.TryGetTableEntryCount(ambientAnimalsFile, out var dataCount)
            || dataCount is 0
        )
        {
            throw new InvalidDataException("Ambient-animal data is unavailable.");
        }

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
                    homeTileMapWidth,
                    homeTileMapHeight,
                    template,
                    dataTableResolver,
                    random,
                    headTowardHomeCenter,
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

    private static AmbientAnimalState CreateSpawnedFromData(
        int globalId,
        int behavior,
        int x,
        int y,
        int homeTileMapWidth,
        int homeTileMapHeight,
        AmbientAnimalState template,
        DataTableResolver dataTableResolver,
        GameRandom random,
        bool headTowardHomeCenter,
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
            template.minimumX,
            template.maximumX,
            template.minimumY,
            template.maximumY,
            template.birdExtraTiles,
            random
        );

        if (headTowardHomeCenter)
        {
            spawned.Heading =
                IntegerMath.GetVectorAngle(
                    homeTileMapWidth * 0x100 - x,
                    homeTileMapHeight * 0x100 - y
                ) << 3;
        }

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
        avoidancePoints = avoidance;
        attractionPoints = attraction;
        landingPoints = landing;
    }

    public void ResetSpawnerPointCache()
    {
        ChecksumState14 = -1;
    }

    internal void ApplySpawnerZoneCleanup(int homeTileMapWidth, int homeTileMapHeight)
    {
        if (Behavior is 3 or 4)
        {
            ChecksumFlag3 = true;
            return;
        }

        attractionPoints = [];

        if (Behavior is not 2)
            avoidancePoints = [];

        if ((ChecksumByte0 is 3 && !redirectRefreshPending) || Behavior is 1 || ChecksumByte0 is 4)
        {
            return;
        }

        int x;
        int y;

        if (Behavior is 2)
        {
            x = unchecked(homeTileMapWidth * 0x100 - GameObject.PositionX);
            y = unchecked(homeTileMapHeight * 0x200 - GameObject.PositionY + 0x1e00);
        }
        else
        {
            x = unchecked(GameObject.PositionX - homeTileMapWidth * 0x100);
            y = unchecked(GameObject.PositionY - homeTileMapHeight * 0x100);
        }

        var length = IntegerMath.GetVectorLength(x, y);

        if (length is 0)
        {
            ChecksumState11 = 0x200;
            ChecksumState12 = 0x200;
            return;
        }

        var behaviorShift = Behavior is 2 ? 1 : 0;
        ChecksumState11 = unchecked(((x << 4) / length) << behaviorShift);
        ChecksumState12 = unchecked(((y << 4) / length) << behaviorShift);
    }

    private static AmbientAnimalState Create(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        int minimumX,
        int maximumX,
        int minimumY,
        int maximumY,
        int birdExtraTiles,
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
            constructorRandom
        );
    }
}
