using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class SpawnerState
{
    private readonly int minimumSpawnTime;
    private readonly int maximumSpawnTime;

    private SpawnerState(
        GameObjectState gameObject,
        int spawnTimer,
        int spawnInterval,
        int minimumSpawnTime,
        int maximumSpawnTime,
        IntPair[] points0,
        IntPair[] points1,
        BuilderState? builder,
        SpawnedPhotographerState? spawnedPhotographer
    )
    {
        GameObject = gameObject;
        SpawnTimer = spawnTimer;
        SpawnInterval = spawnInterval;
        this.minimumSpawnTime = minimumSpawnTime;
        this.maximumSpawnTime = maximumSpawnTime;
        Points0 = points0;
        Points1 = points1;
        Builder = builder;
        SpawnedPhotographer = spawnedPhotographer;
    }

    public GameObjectState GameObject { get; }
    public bool PointsInitialized { get; private set; }
    public int SpawnTimer { get; private set; }
    public int SpawnInterval { get; private set; }
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
        PhotographerState[] photographers
    )
    {
        return gameObjects
            .Where(static gameObject => gameObject.Data.TableId is 208 or 296)
            .Select(gameObject =>
                Create(home, gameObject, dataTableResolver, constructorRandom, photographers)
            )
            .ToArray();
    }

    private static SpawnerState Create(
        HomeSnapshot home,
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom,
        PhotographerState[] photographers
    )
    {
        var (spawnTimer, spawnInterval, minimumSpawnTime, maximumSpawnTime, dynamicPointY) =
            ResolveConfiguration(home, gameObject, dataTableResolver, constructorRandom);

        return gameObject.Data.TableId switch
        {
            208 => new SpawnerState(
                gameObject,
                spawnTimer,
                spawnInterval,
                minimumSpawnTime,
                maximumSpawnTime,
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
                [new IntPair(0x3c00, dynamicPointY), new IntPair(0x3a00, 0x5800)],
                [new IntPair(0x3c00, 0x5800), new IntPair(0x3c00, dynamicPointY)],
                builder: null,
                SpawnedPhotographerState.Create(
                    dataTableResolver,
                    photographers,
                    [new IntPair(0x3c00, dynamicPointY), new IntPair(0x3a00, 0x5800)],
                    [new IntPair(0x3c00, 0x5800), new IntPair(0x3c00, dynamicPointY)]
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

        var timer = gameObject.Snapshot.Timer.ValueKind switch
        {
            JsonValueKind.Undefined or JsonValueKind.Null => 0,
            JsonValueKind.Number when gameObject.Snapshot.Timer.TryGetInt32(out var value) => value,
            _ => throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Spawner {gameObject.GlobalId} has an invalid Timer value."
                )
            ),
        };
        return (
            timer,
            minimum + constructorRandom.NextInt(maximum - minimum),
            minimum,
            maximum,
            checked(home.TileMapHeight * 0x200 + 0x600)
        );
    }

    public void Update(GameRandom random) => Update(random, builderAvailable: true);

    public void Update(GameRandom random, bool builderAvailable)
    {
        var updateExistingBuilder = Builder?.Exists is true;
        var updateExistingPhotographer = SpawnedPhotographer?.Exists is true;

        PointsInitialized = true;
        SpawnTimer++;

        if (SpawnTimer >= SpawnInterval)
        {
            SpawnTimer = 0;
            SpawnInterval = minimumSpawnTime + random.NextInt(maximumSpawnTime - minimumSpawnTime);
            if (builderAvailable)
                Builder?.Spawn(random, Points0);

            SpawnedPhotographer?.Spawn(random, Points0);
        }

        if (updateExistingBuilder)
            Builder!.Update(random);

        if (updateExistingPhotographer)
            SpawnedPhotographer!.Update(random);
    }
}
