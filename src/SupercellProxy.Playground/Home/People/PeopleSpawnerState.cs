using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record PeopleSpawnerState(
    GameObjectState GameObject,
    int[] RemovedPersonGlobalIds,
    int[] SpawnTimes,
    int[] PersonGlobalIds,
    int MinimumSpawnTime,
    int MaximumSpawnTime,
    int ExperienceMultiplier,
    int ConstantExperience
)
{
    private const string PeopleFile = "data/people.csv";
    private const string PeopleSpawnersFile = "data/people_spawners.csv";
    private int legacySpawnTimer;
    private int legacySpawnInterval;
    public bool CleanupCompleted { get; private set; }
    public bool RemovalPassCompleted { get; private set; }

    public bool IsPersonRegistered(int globalId)
    {
        return !RemovalPassCompleted || !RemovedPersonGlobalIds.Contains(globalId);
    }

    public void UpdatePeople(
        PersonState[] people,
        PersonRouteState routes,
        GameRandom random,
        bool completeRemovalPass = true
    )
    {
        for (var i = 0; i < people.Length; i++)
        {
            if (IsPersonRegistered(people[i].GameObject.GlobalId))
                people[i] = people[i].UpdateOne(people, routes, random);
        }

        if (completeRemovalPass && CleanupCompleted)
            RemovalPassCompleted = true;
    }

    public void Update(
        int currentTime,
        GameRandom random,
        PersonState[] people,
        bool completeRemovalPass = true
    )
    {
        if (!CleanupCompleted)
        {
            CleanupCompleted = true;

            for (var i = 0; i < people.Length; i++)
            {
                if (IsPersonRegistered(people[i].GameObject.GlobalId))
                {
                    people[i] = people[i]
                        .CompletePostLoadSetup(ExperienceMultiplier, ConstantExperience);
                }
            }

            RemovalPassCompleted = completeRemovalPass;
        }

        AdvanceLegacySpawnInterval(random);
        Initialize(currentTime, random);
    }

    public static PeopleSpawnerState Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        var peopleSpawnerTableId = ResolveTableId(dataTableResolver, PeopleSpawnersFile);
        var peopleTableId = ResolveTableId(dataTableResolver, PeopleFile);

        var gameObject = gameObjects.Single(gameObject =>
            gameObject.Data.TableId == peopleSpawnerTableId
        );
        var snapshot = gameObject.Snapshot;
        ValidateSlotPeople(gameObjects, snapshot, peopleTableId);
        var (
            useV2Logic,
            slotCount,
            minimumSpawnTime,
            maximumSpawnTime,
            experienceMultiplier,
            constantExperience
        ) = ResolveConfiguration(gameObject, dataTableResolver);

        ValidateConfiguration(gameObject, slotCount, minimumSpawnTime, maximumSpawnTime);

        var people = gameObjects
            .Where(gameObject =>
                gameObject.Data.TableId == peopleTableId && !gameObject.Snapshot.SpawnedFromTutorial
            )
            .ToArray();
        var (spawnTimes, personGlobalIds, removedPersonGlobalIds) = ResolveSlotState(
            gameObject,
            people,
            useV2Logic,
            slotCount
        );

        var state = new PeopleSpawnerState(
            gameObject,
            removedPersonGlobalIds,
            spawnTimes,
            personGlobalIds,
            minimumSpawnTime / GameTick.UpdatesPerSecond,
            maximumSpawnTime / GameTick.UpdatesPerSecond,
            experienceMultiplier,
            constantExperience
        );
        state.legacySpawnTimer = snapshot.Timer.ValueKind switch
        {
            System.Text.Json.JsonValueKind.Number when snapshot.Timer.TryGetInt32(out var timer) =>
                timer,
            _ => 0,
        };
        state.legacySpawnInterval = minimumSpawnTime;
        return state;
    }

    private static int ResolveTableId(DataTableResolver resolver, string file)
    {
        if (resolver.TryGetTableId(file, out var tableId))
            return tableId;

        throw new InvalidOperationException($"{file} is not registered as a native data table.");
    }

    private static void ValidateConfiguration(
        GameObjectState gameObject,
        int slotCount,
        int minimumSpawnTime,
        int maximumSpawnTime
    )
    {
        if (
            slotCount >= gameObject.Snapshot.SlotStates.Length
            && minimumSpawnTime >= 0
            && maximumSpawnTime >= minimumSpawnTime
        )
            return;

        throw new InvalidDataException(
            $"People spawner {gameObject.Data.Name} has invalid native configuration."
        );
    }

    private static (
        bool UseV2,
        int SlotCount,
        int Minimum,
        int Maximum,
        int Multiplier,
        int Constant
    ) ResolveConfiguration(GameObjectState gameObject, DataTableResolver resolver)
    {
        if (
            !resolver.TryResolveBoolean(gameObject.Data.GlobalId, "UseV2Logic", out var useV2Logic)
            || !resolver.TryResolveInt(
                gameObject.Data.GlobalId,
                useV2Logic ? "NumSlotsV2" : "MaxPeople",
                out var slotCount
            )
            || !resolver.TryResolveInt(gameObject.Data.GlobalId, "MinSpawnTime", out var minimum)
            || !resolver.TryResolveInt(gameObject.Data.GlobalId, "MaxSpawnTime", out var maximum)
            || !resolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "ExpMultiplier",
                out var multiplier
            )
            || !resolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "ConstantExp",
                out var constantValue
            )
        )
            throw new InvalidDataException(
                $"People spawner {gameObject.Data.Name} has incomplete native configuration."
            );
        return (useV2Logic, slotCount, minimum, maximum, multiplier, constantValue);
    }

    private static void ValidateSlotPeople(
        IReadOnlyList<GameObjectState> gameObjects,
        GameObjectSnapshot snapshot,
        int peopleTableId
    )
    {
        foreach (
            var personGlobalId in snapshot
                .SlotStates.Select(static slot => slot.PersonGlobalId)
                .Where(static globalId => globalId is not 0)
        )
        {
            if (
                !gameObjects.Any(gameObject =>
                    gameObject.Data.GlobalId == personGlobalId
                    && gameObject.Data.TableId == peopleTableId
                )
            )
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"People-spawner slot refers to missing person {personGlobalId}."
                    )
                );
        }
    }

    private static (
        int[] SpawnTimes,
        int[] PersonGlobalIds,
        int[] RemovedPersonGlobalIds
    ) ResolveSlotState(
        GameObjectState spawner,
        GameObjectState[] people,
        bool useV2Logic,
        int slotCount
    )
    {
        if (!useV2Logic)
        {
            return (
                [],
                [],
                people
                    .Where(static person => person.Snapshot.SpawnedFromV2)
                    .Select(static person => person.GlobalId)
                    .ToArray()
            );
        }

        var slotStates = spawner.Snapshot.SlotStates;
        if (slotStates.Length is 0)
        {
            if (
                people.Length > slotCount
                || people.Any(static person => person.Snapshot.State is not 1)
            )
                throw new InvalidDataException(
                    $"People spawner {spawner.Data.Name} has no saved slots and its retained people cannot be reconstructed."
                );

            return ([], [], people.Select(static person => person.GlobalId).ToArray());
        }

        var spawnTimes = new int[slotCount];
        var personGlobalIds = new int[slotCount];
        for (var i = 0; i < slotStates.Length; i++)
        {
            spawnTimes[i] = slotStates[i].SpawnTime;
            personGlobalIds[i] = slotStates[i].PersonGlobalId;
        }

        var populatedSlotIds = personGlobalIds
            .Where(static globalId => globalId is not 0)
            .ToHashSet();
        var removedPersonGlobalIds = people
            .Where(person => !populatedSlotIds.Contains(person.Data.GlobalId))
            .Select(static person => person.GlobalId)
            .ToArray();
        return (spawnTimes, personGlobalIds, removedPersonGlobalIds);
    }

    public void Initialize(int currentTime, GameRandom random)
    {
        var spawnTimeRange = MaximumSpawnTime - MinimumSpawnTime;

        for (var i = 0; i < SpawnTimes.Length; i++)
        {
            if (PersonGlobalIds[i] is 0 && SpawnTimes[i] is 0)
                SpawnTimes[i] = checked(
                    currentTime + MinimumSpawnTime + random.NextInt(spawnTimeRange)
                );
        }
    }

    private void AdvanceLegacySpawnInterval(GameRandom random)
    {
        legacySpawnTimer = unchecked(legacySpawnTimer + 1);

        if (legacySpawnTimer < legacySpawnInterval)
            return;

        legacySpawnTimer = 0;
        var minimum = checked(MinimumSpawnTime * GameTick.UpdatesPerSecond);
        var maximum = checked(MaximumSpawnTime * GameTick.UpdatesPerSecond);
        legacySpawnInterval = minimum + random.NextInt(maximum - minimum);
    }
}
