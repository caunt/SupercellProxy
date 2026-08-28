using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class SpawnerState
{
    private readonly int _minimumSpawnTime;
    private readonly int _maximumSpawnTime;

    private SpawnerState(
        GameObjectState gameObject,
        int spawnTimer,
        int spawnInterval,
        int minimumSpawnTime,
        int maximumSpawnTime,
        int constructorRandomCall,
        IntPair[] points0,
        IntPair[] points1,
        BuilderState? builder,
        SpawnedPhotographerState? spawnedPhotographer
    )
    {
        GameObject = gameObject;
        SpawnTimer = spawnTimer;
        SpawnInterval = spawnInterval;
        InitialSpawnTimer = spawnTimer;
        InitialSpawnInterval = spawnInterval;
        this._minimumSpawnTime = minimumSpawnTime;
        this._maximumSpawnTime = maximumSpawnTime;
        ConstructorRandomCall = constructorRandomCall;
        Points0 = points0;
        Points1 = points1;
        Builder = builder;
        SpawnedPhotographer = spawnedPhotographer;
    }

    public GameObjectState GameObject { get; }
    public bool PointsInitialized { get; private set; }
    public int SpawnTimer { get; private set; }
    public int SpawnInterval { get; private set; }
    public int InitialSpawnTimer { get; }
    public int InitialSpawnInterval { get; }
    public int FirstUpdateTimer { get; private set; }
    public int FirstUpdateInterval { get; private set; }
    public int FirstUpdateRandomCalls { get; private set; }
    public int ConstructorRandomCall { get; }
    public IntPair[] Points0 { get; }
    public IntPair[] Points1 { get; }
    public BuilderState? Builder { get; }
    public SpawnedPhotographerState? SpawnedPhotographer { get; }
    public static bool HasParent => false;

    public static SpawnerState[] Resolve(
        HomeSnapshot home,
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom,
        PhotographerState[] photographers,
        InventoryState inventory
    )
    {
        return gameObjects
            .Where(static gameObject => gameObject.Data.TableId is 208 or 296)
            .Select(gameObject =>
                Create(
                    home,
                    gameObject,
                    dataTableResolver,
                    constructorRandom,
                    photographers,
                    gameObjects,
                    inventory
                )
            )
            .ToArray();
    }

    private static SpawnerState Create(
        HomeSnapshot home,
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom,
        PhotographerState[] photographers,
        GameObjectState[] gameObjects,
        InventoryState inventory
    )
    {
        var (
            spawnTimer,
            spawnInterval,
            minimumSpawnTime,
            maximumSpawnTime,
            constructorRandomCall,
            dynamicPointY
        ) = ResolveConfiguration(home, gameObject, dataTableResolver, constructorRandom);

        return gameObject.Data.TableId switch
        {
            208 => new SpawnerState(
                gameObject,
                spawnTimer,
                spawnInterval,
                minimumSpawnTime,
                maximumSpawnTime,
                constructorRandomCall,
                [new IntPair(0x3a00, dynamicPointY), new IntPair(0x3a00, 0x5200)],
                [new IntPair(0x3c00, 0x5200), new IntPair(0x3a00, dynamicPointY)],
                BuilderState.Create(dataTableResolver),
                spawnedPhotographer: null
            ),
            296 => new SpawnerState(
                gameObject,
                spawnTimer,
                spawnInterval,
                minimumSpawnTime,
                maximumSpawnTime,
                constructorRandomCall,
                [new IntPair(0x3c00, dynamicPointY), new IntPair(0x3a00, 0x5800)],
                [new IntPair(0x3c00, 0x5800), new IntPair(0x3c00, dynamicPointY)],
                builder: null,
                SpawnedPhotographerState.Create(
                    dataTableResolver,
                    photographers,
                    [new IntPair(0x3c00, dynamicPointY), new IntPair(0x3a00, 0x5800)],
                    [new IntPair(0x3c00, 0x5800), new IntPair(0x3c00, dynamicPointY)],
                    home.TileMapWidth,
                    home.TileMapHeight,
                    gameObjects,
                    inventory
                )
            ),
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported spawner table {gameObject.Data.TableId}."
                )
            ),
        };
    }

    private static (
        int Timer,
        int Interval,
        int Minimum,
        int Maximum,
        int ConstructorRandomCall,
        int DynamicPointY
    ) ResolveConfiguration(
        HomeSnapshot home,
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom
    )
    {
        if (home.TileMapHeight <= 0)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid tile-map height {home.TileMapHeight}."
                )
            );

        if (
            !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "MinSpawnTime",
                out var minimum
            )
            || !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "MaxSpawnTime",
                out var maximum
            )
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Spawner {gameObject.GlobalId} has no native spawn-time range."
                )
            );

        if (maximum < minimum)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Spawner {gameObject.GlobalId} has an invalid native spawn-time range."
                )
            );

        var timer = ResolveTimer(gameObject);
        var constructorRandomCall = constructorRandom.Calls;
        return (
            timer,
            minimum + constructorRandom.NextInt(maximum - minimum),
            minimum,
            maximum,
            constructorRandomCall,
            checked((home.TileMapHeight + 3) * GameObjectState.TileSize)
        );
    }

    private static int ResolveTimer(GameObjectState gameObject)
    {
        if (gameObject.Snapshot.Timer.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
            return 0;

        if (gameObject.Snapshot.Timer.TryGetInt32(out var timer))
            return timer;

        throw new InvalidDataException(
            string.Create(
                CultureInfo.InvariantCulture,
                $"Spawner {gameObject.GlobalId} has an invalid Timer value."
            )
        );
    }

    public void Update(GameRandom random) => Update(random, builderAvailable: true);

    public void Update(GameRandom random, bool builderAvailable)
    {
        var initialRandomCalls = random.Calls;
        var updateExistingBuilder = Builder?.Exists is true;
        var updateExistingPhotographer = SpawnedPhotographer?.Exists is true;

        PointsInitialized = true;
        SpawnTimer++;

        if (SpawnTimer >= SpawnInterval)
        {
            SpawnTimer = 0;
            SpawnInterval =
                _minimumSpawnTime + random.NextInt(_maximumSpawnTime - _minimumSpawnTime);
            if (builderAvailable)
                Builder?.Spawn(random, Points0);

            SpawnedPhotographer?.Spawn(random, Points0);
        }

        if (updateExistingBuilder && Builder is { } builder)
            builder.Update(random);

        if (updateExistingPhotographer && SpawnedPhotographer is { } photographer)
            photographer.Update(random);

        if (FirstUpdateInterval is 0)
        {
            FirstUpdateTimer = SpawnTimer;
            FirstUpdateInterval = SpawnInterval;
            FirstUpdateRandomCalls = random.Calls - initialRandomCalls;
        }
    }
}
