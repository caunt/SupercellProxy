using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class AmbientAnimalSpawnerState
{
    private const string DataName = "AmbientAnimalSpawner";
    private static readonly int[] CommonTableIds =
    [
        6,
        10,
        11,
        14,
        18,
        21,
        4,
        35,
        102,
        137,
        138,
        134,
        139,
        135,
        144,
        158,
        161,
    ];

    private static readonly int[] ExtendedTableIds =
    [
        6,
        10,
        11,
        14,
        18,
        21,
        4,
        35,
        32,
        57,
        102,
        137,
        138,
        134,
        139,
        135,
        144,
        158,
        161,
    ];

    private static readonly int[][] ZoneConfiguration =
    [
        [50, 100, 3000, 4500, 130, 12, 0, -1, -1, -1, 25, 60],
        [50, 100, 3000, 4500, 40, 15, 1, -1, -1, -1, 25, 60],
        [50, 100, 1500, 2500, 200, 1, 2, -1, -1, -1, 25, 60],
        [50, 100, 1500, 2500, 40, 12, 3, -1, -1, -1, 25, 60],
        [50, 100, 3000, 4500, 50, 50, 4, -1, -1, -1, 25, 60],
        [50, 100, 3000, 4500, 30, 20, 0, 1, 3, 4, 25, 60],
    ];

    private readonly GameObjectState[] _gameObjects;
    private readonly DataTableResolver _dataTableResolver;
    private readonly AmbientAnimalState[] _ambientAnimals;
    private readonly List<AmbientAnimalState> _spawnedAmbientAnimals = [];
    private readonly HashSet<int> _removedAmbientAnimalGlobalIds = [];
    private readonly int _homeTileMapWidth;
    private readonly int _homeTileMapHeight;
    private int _nextAmbientAnimalGlobalId;
    private int _periodicSpawnCounter;

    private AmbientAnimalSpawnerState(
        GameObjectState gameObject,
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        AmbientAnimalState[] ambientAnimals,
        int homeTileMapWidth,
        int homeTileMapHeight,
        int constructorRandomValue
    )
    {
        GameObject = gameObject;
        this._gameObjects = gameObjects;
        this._dataTableResolver = dataTableResolver;
        this._ambientAnimals = ambientAnimals;
        this._homeTileMapWidth = homeTileMapWidth;
        this._homeTileMapHeight = homeTileMapHeight;
        _nextAmbientAnimalGlobalId =
            45 * DataTableResolver.GlobalIdTableSize
            + ambientAnimals.Max(static animal =>
                animal.GameObject.GlobalId % DataTableResolver.GlobalIdTableSize
            )
            + 1;
        ConstructorRandomValue = constructorRandomValue;
        Points0 = CreatePointRows();
        Points1 = CreatePointRows();
        Points2 = CreatePointRows();
        ActiveZones = Enumerable
            .Range(0, ZoneConfiguration.Length)
            .Select(static _ => new AmbientAnimalSpawnerZoneState())
            .ToArray();
        TemplateZones = Enumerable
            .Range(0, ZoneConfiguration.Length)
            .Select(static _ => new AmbientAnimalSpawnerZoneState())
            .ToArray();
    }

    public GameObjectState GameObject { get; }
    public int SelectedZone { get; private set; }
    public int ConstructorRandomValue { get; private set; }
    public static int ZoneCount => ZoneConfiguration.Length;
    public bool Initialized { get; private set; }
    public bool RefreshPending { get; private set; }
    public int ChecksumState0 { get; private set; }
    public List<AmbientAnimalSpawnerPoint>[] Points0 { get; private set; }
    public List<AmbientAnimalSpawnerPoint>[] Points1 { get; private set; }
    public List<AmbientAnimalSpawnerPoint>[] Points2 { get; private set; }
    public AmbientAnimalSpawnerZoneState[] ActiveZones { get; }
    public AmbientAnimalSpawnerZoneState[] TemplateZones { get; }
    public static bool HasParent => true;
    public static bool HasObjectManager => true;

    public static AmbientAnimalSpawnerState Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom,
        AmbientAnimalState[] ambientAnimals,
        int homeTileMapWidth,
        int homeTileMapHeight
    )
    {
        ArgumentNullException.ThrowIfNull(ambientAnimals);
        var spawners = gameObjects
            .Where(static gameObject => gameObject.Data.TableId is 46)
            .ToArray();

        if (spawners.Length is not 1)
            throw new InvalidDataException(
                $"Expected one ambient-animal spawner, found {spawners.Length}."
            );

        var state = new AmbientAnimalSpawnerState(
            spawners[0],
            gameObjects,
            dataTableResolver,
            ambientAnimals,
            homeTileMapWidth,
            homeTileMapHeight,
            constructorRandom.NextInt(100)
        );
        state.RebuildPoints(constructorRandom);
        return state;
    }

    public void Initialize(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);
        SelectedZone = random.NextInt(ZoneCount);
        CopyAndScaleSelectedZone(random);
        StartSelectedZone(random);
        Initialized = true;
        RefreshPoints(random);

        ActiveZones[SelectedZone].SpawnDelayCounter++;
        ChecksumState0++;
    }

    public void Update(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (!Initialized)
        {
            Initialize(random);
            _periodicSpawnCounter = 1;
            return;
        }

        RefreshPoints(random);

        var registeredAmbientAnimals = _ambientAnimals
            .Where(animal => !_removedAmbientAnimalGlobalIds.Contains(animal.GameObject.GlobalId))
            .Concat(_spawnedAmbientAnimals)
            .ToArray();
        var lifecycle = ActiveZones[SelectedZone]
            .AdvanceLifecycle(SelectedConfiguration, registeredAmbientAnimals.Length);

        if (lifecycle.SpawnRequired)
            SpawnAmbientAnimal(random);

        if (lifecycle.CleanupRequired)
        {
            foreach (var animal in registeredAmbientAnimals)
                animal.ApplySpawnerZoneCleanup(_homeTileMapWidth, _homeTileMapHeight);
        }

        if (lifecycle.Complete)
            TransitionZone(random);

        ChecksumState0 = unchecked(ChecksumState0 + 1);
        UpdatePeriodicSpawning(
            random,
            _ambientAnimals.Count(animal =>
                !_removedAmbientAnimalGlobalIds.Contains(animal.GameObject.GlobalId)
            ) + _spawnedAmbientAnimals.Count
        );
    }

    internal void UpdateRegisteredAnimals(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        foreach (
            var animal in _ambientAnimals.Where(animal =>
                !_removedAmbientAnimalGlobalIds.Contains(animal.GameObject.GlobalId)
            )
        )
        {
            AmbientAnimalState.Update([animal], random);
        }

        foreach (var animal in _spawnedAmbientAnimals)
            AmbientAnimalState.Update([animal], random);
    }

    internal void CompleteRegisteredAnimalRemoval()
    {
        foreach (var animal in _ambientAnimals.Where(static animal => animal.IsRemoved))
            _removedAmbientAnimalGlobalIds.Add(animal.GameObject.GlobalId);

        _spawnedAmbientAnimals.RemoveAll(static animal => animal.IsRemoved);
    }

    private void SpawnAmbientAnimal(GameRandom random)
    {
        var configuredIndex = random.NextInt(4);
        var behavior = -1;

        for (var offset = 0; offset < 4; offset++)
        {
            behavior = SelectedConfiguration[6 + (configuredIndex + offset) % 4];

            if (behavior is not -1)
                break;
        }

        if (behavior is 3)
        {
            _ = ResolveBehaviorThreeSpawnPosition(random);
            return;
        }

        if (behavior is 1)
        {
            SpawnBehaviorOne(random);
            return;
        }

        if (behavior is 2)
        {
            SpawnBehaviorTwo(random);
            return;
        }

        if (behavior is 4)
        {
            SpawnBehaviorFour(random);
            return;
        }

        if (behavior is not 0)
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Ambient-animal behavior {behavior} spawning is not implemented."
                )
            );

        SpawnBehaviorZero(random);
    }

    private void SpawnBehaviorOne(GameRandom random)
    {
        var position = ResolveBehaviorOneSpawnPosition(random);
        CreateSpawnedAmbientAnimal(1, position.X, position.Y, random, 0, 0);
    }

    private void SpawnBehaviorTwo(GameRandom random)
    {
        var position = ResolveBehaviorTwoSpawnPosition(random);
        CreateSpawnedAmbientAnimal(2, position.X, position.Y, random, 0, 0);
    }

    private void SpawnBehaviorFour(GameRandom random)
    {
        var edgePosition = ResolveBehaviorFourSpawnPosition(random);
        var boatBlocksSpawn =
            _gameObjects
                .FirstOrDefault(static gameObject => gameObject.Data.TableId is 58)
                ?.Snapshot.State
            is 3
                or 5;
        if (!boatBlocksSpawn)
        {
            CreateSpawnedAmbientAnimal(4, edgePosition.X, edgePosition.Y, random, 0, 0);
        }
    }

    private void SpawnBehaviorZero(GameRandom random)
    {
        var forests = _gameObjects
            .Where(static gameObject => gameObject.Data.TableId is 5)
            .ToArray();
        if (forests.Length is 0)
            throw new InvalidDataException("The home has no ambient-animal forest spawn source.");

        var forestIndex = random.NextInt(forests.Length);
        GameObjectState? forest = null;

        for (var checkedForest = 0; checkedForest < forests.Length; checkedForest++, forestIndex++)
        {
            var candidate = forests[forestIndex % forests.Length];
            _dataTableResolver.TryResolveInt(candidate.Data.GlobalId, "Type", out var type);

            if (type is 0)
            {
                forest = candidate;
                break;
            }
        }

        if (forest is null)
            throw new InvalidDataException("The home has no native type-0 forest spawn source.");

        var x = -0x1e;
        var y = -0x1e;

        for (var current = forest; current is not null; current = current.Parent)
        {
            x = unchecked(x + current.PositionX);
            y = unchecked(y + current.PositionY);
        }

        CreateSpawnedAmbientAnimal(0, x, y, random, 0, 0);
    }

    private (int X, int Y) ResolveBehaviorOneSpawnPosition(GameRandom random)
    {
        if (_homeTileMapWidth <= 12 || _homeTileMapHeight <= 12)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The {_homeTileMapWidth}x{_homeTileMapHeight} home is too small for an ambient-animal edge spawn."
                )
            );

        if (
            !_dataTableResolver.TryResolve(
                GameAssetFiles.AmbientAnimalSpawners,
                DataName,
                out var spawnerData
            )
        )
            throw new InvalidDataException("Unable to resolve AmbientAnimalSpawner.");

        var edge = random.NextInt(4);

        if (
            !_dataTableResolver.TryResolveInt(
                spawnerData.GlobalId,
                "EdgeSpawnTileX",
                edge,
                out var x
            )
            || !_dataTableResolver.TryResolveInt(
                spawnerData.GlobalId,
                "EdgeSpawnTileY",
                edge,
                out var y
            )
            || !_dataTableResolver.TryResolveInt(
                spawnerData.GlobalId,
                "BirdExtraTiles",
                out var birdExtraTiles
            )
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"AmbientAnimalSpawner edge {edge} has incomplete spawn coordinates."
                )
            );
        }

        if (edge is 2 or 3)
            x = random.NextInt(_homeTileMapWidth - 12) + 6;
        else
            y = random.NextInt(_homeTileMapHeight - 12) + 6;

        if (edge is 1)
            x = checked(x + birdExtraTiles);
        else if (edge is 3)
            y = checked(y + birdExtraTiles);

        return (checked(x << 9), checked(y << 9));
    }

    private (int X, int Y) ResolveBehaviorFourSpawnPosition(GameRandom random)
    {
        var upperHalf = random.NextInt(2) is not 0;
        var x = -0x300 - ((random.NextInt(11) * 0x200 + 0x400) >> 1);
        var y =
            random.NextInt(_homeTileMapHeight << 7)
            + _homeTileMapHeight * (upperHalf ? 0x140 : 0x40);
        return (x, y);
    }

    private (int X, int Y) ResolveBehaviorTwoSpawnPosition(GameRandom random)
    {
        const int edge = 3;

        if (_homeTileMapWidth <= 12)
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The {_homeTileMapWidth}-tile-wide home is too small for an ambient-animal edge spawn."
                )
            );
        }

        if (
            !_dataTableResolver.TryResolve(
                GameAssetFiles.AmbientAnimalSpawners,
                DataName,
                out var spawnerData
            )
            || !_dataTableResolver.TryResolveInt(
                spawnerData.GlobalId,
                "EdgeSpawnTileY",
                edge,
                out var y
            )
        )
        {
            throw new InvalidDataException(
                $"AmbientAnimalSpawner edge {edge} has incomplete behavior-2 spawn coordinates."
            );
        }

        var x = random.NextInt(_homeTileMapWidth - 12) + 6;
        return (checked(x << 9), checked((y - 2) << 9));
    }

    private (int X, int Y) ResolveBehaviorThreeSpawnPosition(GameRandom random)
    {
        var forestSources = _gameObjects
            .Where(gameObject => gameObject.Data.TableId is 5 && ResolveForestType(gameObject) is 2)
            .ToArray();
        var decorationSources = _gameObjects
            .Where(gameObject =>
                gameObject.Data.TableId is 3
                && _dataTableResolver.TryResolveBoolean(
                    gameObject.Data.GlobalId,
                    "SpawnsFrogs",
                    out var spawnsFrogs
                )
                && spawnsFrogs
            )
            .ToArray();

        GameObjectState? source = null;

        if (
            (forestSources.Length is not 0 || decorationSources.Length is not 0)
            && random.NextInt(2) is 0
        )
        {
            var useDecoration =
                forestSources.Length is 0
                || random.NextInt(2) is not 0 && decorationSources.Length is not 0;
            var sources = useDecoration ? decorationSources : forestSources;
            source = sources[random.NextInt(sources.Length)];
        }

        int x;
        int y;

        if (source is null)
        {
            x = -0x300;
            var upperHalf = random.NextInt(2) is not 0;
            y =
                random.NextInt(_homeTileMapHeight << 7)
                + _homeTileMapHeight * (upperHalf ? 0x140 : 0x40);
        }
        else
        {
            var width = source.Mirrored ? source.TileHeight : source.TileWidth;
            var height = source.Mirrored ? source.TileWidth : source.TileHeight;

            if (width is null or < 1 || height is null or < 1)
                throw new InvalidDataException(
                    "An ambient-animal behavior-3 spawn source has unresolved dimensions."
                );

            var position = ResolveAbsolutePosition(source);
            x = position.X + width.Value * 0x80 + random.NextInt(width.Value << 8);
            y = position.Y + height.Value * 0x80 + random.NextInt(height.Value << 8);
        }

        return (x, y);
    }

    private void UpdatePeriodicSpawning(GameRandom random, int ambientAnimalCount)
    {
        var previousCounter = _periodicSpawnCounter;
        _periodicSpawnCounter = unchecked(_periodicSpawnCounter + 1);

        if (previousCounter < 500)
            return;

        _periodicSpawnCounter = 0;

        if (ambientAnimalCount >= 60 || SelectedZone is not 0)
            return;

        var zone = ActiveZones[SelectedZone];

        if (
            zone.CleanupDelayThreshold is not 0
            && zone.CleanupDelayCounter >= zone.CleanupDelayThreshold
        )
            return;

        var decorations = _gameObjects
            .Where(static gameObject => gameObject.Data.TableId is 3)
            .ToArray();
        var attractorCount = decorations.Count(gameObject =>
            _dataTableResolver.TryResolveBoolean(
                gameObject.Data.GlobalId,
                "AttractsButterflies",
                out var attractsButterflies
            ) && attractsButterflies
        );

        if (attractorCount is 0)
            return;

        var spawnCount = Math.Max(1, IntegerMath.GetSquareRoot(attractorCount));

        for (var spawn = 0; spawn < spawnCount && ambientAnimalCount <= 1_000; spawn++)
        {
            var decoration = FindAttractingDecoration(decorations, random);

            if (decoration is null)
                continue;

            var (x, y) = ResolveAbsolutePosition(decoration);
            CreateSpawnedAmbientAnimal(0, unchecked(x + 100), unchecked(y + 100), random, 0, 0);
            ambientAnimalCount++;
        }
    }

    private GameObjectState? FindAttractingDecoration(
        GameObjectState[] decorations,
        GameRandom random
    )
    {
        var decorationIndex = random.NextInt(decorations.Length);
        for (
            var checkedDecoration = 0;
            checkedDecoration < decorations.Length;
            checkedDecoration++, decorationIndex++
        )
        {
            var candidate = decorations[decorationIndex % decorations.Length];
            if (
                _dataTableResolver.TryResolveBoolean(
                    candidate.Data.GlobalId,
                    "AttractsButterflies",
                    out var attractsButterflies
                ) && attractsButterflies
            )
            {
                return candidate;
            }
        }

        return null;
    }

    private void CreateSpawnedAmbientAnimal(
        int behavior,
        int x,
        int y,
        GameRandom random,
        int destinationX,
        int destinationY
    )
    {
        if (_nextAmbientAnimalGlobalId % DataTableResolver.GlobalIdTableSize is 0)
            throw new InvalidOperationException("Ambient-animal instance IDs are exhausted.");

        var template =
            _ambientAnimals.FirstOrDefault(static animal => animal.Behavior is 0)
            ?? throw new InvalidDataException(
                "The home has no behavior-0 ambient-animal template."
            );
        var spawned = AmbientAnimalState.CreateSpawned(
            _nextAmbientAnimalGlobalId++,
            behavior,
            x,
            y,
            template,
            _dataTableResolver,
            random,
            destinationX,
            destinationY,
            Points0[behavior],
            Points1[behavior],
            Points2[behavior]
        );
        _spawnedAmbientAnimals.Add(spawned);
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

    private void CopyAndScaleSelectedZone(GameRandom random)
    {
        var configuration = ZoneConfiguration[SelectedZone].ToArray();
        var divisor = random.NextInt(100) switch
        {
            < 5 => 10,
            < 15 => 6,
            < 25 => 2,
            _ => 1,
        };

        for (var i = 0; i <= 4; i++)
            configuration[i] /= divisor;

        var factor = random.NextInt(100) switch
        {
            < 2 => 4,
            < 15 => 3,
            < 25 => 2,
            _ => 1,
        };

        configuration[4] /= factor;
        configuration[5] *= factor;
        configuration[11] *= factor;
        SelectedConfiguration = configuration;
    }

    private int[] SelectedConfiguration { get; set; } = [];

    private void StartSelectedZone(GameRandom random)
    {
        var zone = ActiveZones[SelectedZone];
        zone.SpawnDelayThreshold =
            SelectedConfiguration[0]
            + random.NextInt(SelectedConfiguration[1] - SelectedConfiguration[0]);

        if ((SelectedConfiguration[2] | SelectedConfiguration[3]) is not 0)
        {
            zone.CleanupDelayThreshold =
                SelectedConfiguration[2]
                + random.NextInt(SelectedConfiguration[3] - SelectedConfiguration[2]);
        }

        BindAnimalPoints();
        ConstructorRandomValue = random.NextInt(100);
        RefreshPending = true;
    }

    private void TransitionZone(GameRandom random)
    {
        var previousZone = SelectedZone;
        var nextZone = random.NextInt(ZoneCount);

        if (ZoneCount > 1 && nextZone == previousZone)
            nextZone = (nextZone + 1) % ZoneCount;

        SelectedZone = nextZone;
        CopyAndScaleSelectedZone(random);
        StartSelectedZone(random);
    }

    private void RefreshPoints(GameRandom random)
    {
        if (!RefreshPending)
            return;

        ClearPointRows(Points0);
        ClearPointRows(Points1);
        ClearPointRows(Points2);
        RebuildPoints(random);
        RefreshPending = false;

        foreach (var animal in _ambientAnimals)
            animal.ResetSpawnerPointCache();
    }

    private void BindAnimalPoints()
    {
        foreach (var animal in _ambientAnimals)
        {
            if (uint.CreateTruncating(animal.Behavior) >= Points0.Length)
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Unsupported ambient-animal behavior {animal.Behavior}."
                    )
                );

            animal.ConfigureSpawnerPoints(
                Points0[animal.Behavior],
                Points1[animal.Behavior],
                Points2[animal.Behavior]
            );
        }
    }

    private void RebuildPoints(GameRandom random)
    {
        AddPoints(Points0[0], CommonTableIds, 0x400000);

        AddDecorationPoints(Points1[0], "AttractsButterflies", expected: true, 0x400000);

        if (Points1[0].Count is 0 && ConstructorRandomValue < 60)
            AddFallbackForestPoint(random);

        AddForestPoints(Points2[1], 0, 0x400000);
        AddDecorationPoints(Points2[1], "BirdsLand", expected: true, 0x2400000);
        AddBooleanDataPoints(Points2[1], 62, "BirdCanLand", expected: true, 0x400000);

        AddPoints(Points0[2], ExtendedTableIds, 0x400000);
        AddForestPoints(Points0[2], 0, 0x10000);
        AddForestPoints(Points0[2], 1, 0x100000);
        AddForestPoints(Points0[2], 2, 0x100000);
        AddPoints(Points0[2], [48], 0x64000);
        AddPoints(Points0[2], [40], 0x40000);
        AddPoints(Points0[2], [3], 0x100000);
        AddPoints(Points0[2], [62], 0x10000);

        AddPoints(Points0[3], ExtendedTableIds, 0x1c6e39);
        AddPoints(Points0[3], [48], 0x64000);
        AddDecorationPoints(Points0[3], "SpawnsFrogs", expected: false, 0x100000);
        AddForestPoints(Points1[3], 2, 0x1c639);
        AddDecorationPoints(Points1[3], "SpawnsFrogs", expected: true, 0x1c639);
    }

    private void AddFallbackForestPoint(GameRandom random)
    {
        var forests = _gameObjects
            .Where(static gameObject => gameObject.Data.TableId is 5)
            .ToArray();

        if (forests.Length is 0)
            return;

        var index = random.NextInt(forests.Length);

        for (var attempts = 0; attempts < forests.Length; attempts++, index++)
        {
            var forest = forests[index % forests.Length];

            if (ResolveForestType(forest) is not 0)
                continue;

            Points1[0].Add(CreatePoint(forest, 0x400000));
            return;
        }
    }

    private void AddForestPoints(
        List<AmbientAnimalSpawnerPoint> destination,
        int mode,
        int radiusSquared
    )
    {
        AddFilteredPoints(
            destination,
            [5],
            radiusSquared,
            gameObject =>
            {
                var objectType = ResolveForestType(gameObject);
                return objectType == mode || objectType is -1;
            }
        );
    }

    private int ResolveForestType(GameObjectState gameObject)
    {
        // Native integer accessors map empty CSV cells to zero.
        return _dataTableResolver.TryResolveInt(gameObject.Data.GlobalId, "Type", out var type)
            ? type
            : 0;
    }

    private void AddDecorationPoints(
        List<AmbientAnimalSpawnerPoint> destination,
        string fieldName,
        bool expected,
        int radiusSquared
    )
    {
        AddBooleanDataPoints(destination, 3, fieldName, expected, radiusSquared);
    }

    private void AddBooleanDataPoints(
        List<AmbientAnimalSpawnerPoint> destination,
        int tableId,
        string fieldName,
        bool expected,
        int radiusSquared
    )
    {
        AddFilteredPoints(
            destination,
            [tableId],
            radiusSquared,
            gameObject =>
            {
                _dataTableResolver.TryResolveBoolean(
                    gameObject.Data.GlobalId,
                    fieldName,
                    out var value
                );
                return value == expected;
            }
        );
    }

    private void AddPoints(
        List<AmbientAnimalSpawnerPoint> destination,
        int[] tableIds,
        int radiusSquared
    )
    {
        AddFilteredPoints(destination, tableIds, radiusSquared, static _ => true);
    }

    private void AddFilteredPoints(
        List<AmbientAnimalSpawnerPoint> destination,
        int[] tableIds,
        int radiusSquared,
        Func<GameObjectState, bool> predicate
    )
    {
        foreach (var tableId in tableIds)
        {
            foreach (
                var gameObject in _gameObjects.Where(gameObject =>
                    gameObject.Data.TableId == tableId
                )
            )
            {
                if (predicate(gameObject))
                    destination.Add(CreatePoint(gameObject, radiusSquared));
            }
        }
    }

    private static AmbientAnimalSpawnerPoint CreatePoint(
        GameObjectState gameObject,
        int radiusSquared
    )
    {
        if (
            gameObject.TileWidth is not int tileWidth
            || gameObject.TileHeight is not int tileHeight
        )
            throw new InvalidDataException(
                $"Dimensions for {gameObject.Data.File} are not implemented."
            );

        var width = gameObject.Mirrored ? tileHeight : tileWidth;
        var height = gameObject.Mirrored ? tileWidth : tileHeight;
        var x = 0;
        var y = 0;

        for (var current = gameObject; current is not null; current = current.Parent)
        {
            x = unchecked(x + current.PositionX);
            y = unchecked(y + current.PositionY);
        }

        return new AmbientAnimalSpawnerPoint(
            unchecked(x + width * 0x100),
            unchecked(y + height * 0x100),
            radiusSquared
        );
    }

    private static List<AmbientAnimalSpawnerPoint>[] CreatePointRows()
    {
        return [new(), new(), new(), new(), new()];
    }

    private static void ClearPointRows(IEnumerable<List<AmbientAnimalSpawnerPoint>> rows)
    {
        foreach (var row in rows)
            row.Clear();
    }
}
