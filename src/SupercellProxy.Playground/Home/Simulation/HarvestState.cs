using System.Globalization;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Messages.Clientbound;

namespace SupercellProxy.Playground.Home.Simulation;

internal sealed record HarvestState(
    int ServerTimestamp,
    int AvatarTimestamp,
    GameTick Tick,
    GameRandom Random,
    CommandExecutionState CommandExecution,
    DataTableResolver DataTableResolver,
    int HighestDataTableId,
    ClientAvatar ClientAvatar,
    ShopEventManagerState ShopEventManager,
    MapGameManagerState MapGameManager,
    NeighborhoodObjectManagerState NeighborhoodObjectManager,
    AvatarDataSnapshot AvatarData,
    InventoryState Inventory,
    DataTableReference ExperienceData,
    ExpansionReadyDataState[] ExpansionReadyDatas,
    HomeSnapshot Home,
    GameObjectState[] GameObjects,
    CarState[] Cars,
    CustomizationManagerState CustomizationManager,
    CreatureManagerState CreatureManager,
    ChronosEventManagerState ChronosEventManager,
    AmbientAnimalState[] AmbientAnimals,
    AmbientAnimalSpawnerState AmbientAnimalSpawner,
    PostmanState Postman,
    FieldState[] Fields,
    AnimalHabitatState[] AnimalHabitats,
    AnimalState[] Animals,
    AnimalHabitatPieceState[] AnimalHabitatPieces,
    GathererHabitatState[] GathererHabitats,
    GathererNestState[] GathererNests,
    GathererState[] Gatherers,
    HelperCharacterState[] HelperCharacters,
    ConstructionBuildingState[] ConstructionBuildings,
    BoyBoxState[] BoyBoxes,
    PhotographerState[] Photographers,
    PersonRouteState PersonRoutes,
    PersonState[] People,
    PeopleSpawnerState PeopleSpawner,
    MysteryBoxSpawnerState MysteryBoxSpawner,
    OrderTableState[] OrderTables,
    WheelState[] Wheels,
    BalloonState[] Balloons,
    SpawnerState[] Spawners,
    BoyState[] Boys
)
{
    private int nextFieldGlobalId =
        4 * DataTableResolver.GlobalIdTableSize
        + GameObjects
            .Where(static gameObject => gameObject.Data.TableId is 4)
            .Max(static gameObject => gameObject.GlobalId % DataTableResolver.GlobalIdTableSize)
        + 1;

    public const int OwnHomeGameMode = 1;

    public static int GameMode => OwnHomeGameMode;
    public static bool ChecksumEnabled => true;
    public static bool FullChecksumEnabled => false;
    public static bool DebugChecksumEnabled => false;

    public static HarvestState Create(
        OwnHomeDataMessage message,
        DataTableResolver dataTableResolver
    )
    {
        var constructorRandom = new GameRandom(0);
        var customizationManager = CustomizationManagerState.Create(
            message.AvatarData.AvatarDataObjects.Common.CustomizationManager
        );
        var environment = ResolveEnvironment(message, dataTableResolver, constructorRandom);
        var gameObjects = environment.GameObjects;
        var personRoutes = PersonRouteState.Resolve(dataTableResolver);
        var people = PersonState.Resolve(gameObjects, dataTableResolver);
        var peopleSpawner = PeopleSpawnerState.Resolve(gameObjects, dataTableResolver);
        var inventory = InventoryState.Create(message.ClientAvatar);
        var mysteryBoxSpawner = MysteryBoxSpawnerState.Resolve(
            gameObjects,
            dataTableResolver,
            constructorRandom,
            inventory
        );
        var wheels = WheelState.Resolve(gameObjects, dataTableResolver, constructorRandom);
        var balloons = BalloonState.Resolve(gameObjects, dataTableResolver, constructorRandom);
        var photographers = PhotographerState.Resolve(gameObjects, dataTableResolver);
        var spawners = SpawnerState.Resolve(
            message.Home,
            gameObjects,
            dataTableResolver,
            constructorRandom,
            photographers
        );
        var animals = AnimalState.Resolve(
            gameObjects,
            environment.AnimalHabitats,
            environment.AnimalHabitatPieces,
            dataTableResolver
        );

        if (!dataTableResolver.TryResolve("data/money.csv", "ExpPoints", out var experienceData))
            throw new InvalidDataException("Unable to resolve ExpPoints from data/money.csv.");

        return CreateState(
            message,
            dataTableResolver,
            constructorRandom,
            customizationManager,
            environment,
            personRoutes,
            people,
            peopleSpawner,
            mysteryBoxSpawner,
            wheels,
            balloons,
            photographers,
            spawners,
            inventory,
            animals,
            experienceData
        );
    }

    private static (
        GameObjectState[] GameObjects,
        AnimalHabitatPieceState[] AnimalHabitatPieces,
        AnimalHabitatState[] AnimalHabitats,
        AmbientAnimalState[] AmbientAnimals,
        AmbientAnimalSpawnerState AmbientAnimalSpawner
    ) ResolveEnvironment(
        OwnHomeDataMessage message,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom
    )
    {
        var gameObjects = message.Home.ResolveGameObjects(dataTableResolver);
        var animalHabitatPieces = AnimalHabitatPieceState.Resolve(
            gameObjects,
            dataTableResolver,
            constructorRandom
        );
        var animalHabitats = AnimalHabitatState.Resolve(
            message.Home,
            gameObjects,
            animalHabitatPieces,
            dataTableResolver
        );
        gameObjects = gameObjects
            .Concat(
                animalHabitatPieces
                    .OrderBy(static piece => piece.GameObject.GlobalId)
                    .Select(static piece => piece.GameObject)
            )
            .ToArray();
        var ambientAnimals = AmbientAnimalState.Resolve(
            gameObjects,
            dataTableResolver,
            constructorRandom
        );
        var ambientAnimalSpawner = AmbientAnimalSpawnerState.Resolve(
            gameObjects,
            dataTableResolver,
            constructorRandom,
            ambientAnimals,
            message.Home.TileMapWidth,
            message.Home.TileMapHeight
        );
        return (
            gameObjects,
            animalHabitatPieces,
            animalHabitats,
            ambientAnimals,
            ambientAnimalSpawner
        );
    }

    private static HarvestState CreateState(
        OwnHomeDataMessage message,
        DataTableResolver dataTableResolver,
        GameRandom random,
        CustomizationManagerState customizationManager,
        (
            GameObjectState[] GameObjects,
            AnimalHabitatPieceState[] AnimalHabitatPieces,
            AnimalHabitatState[] AnimalHabitats,
            AmbientAnimalState[] AmbientAnimals,
            AmbientAnimalSpawnerState AmbientAnimalSpawner
        ) environment,
        PersonRouteState personRoutes,
        PersonState[] people,
        PeopleSpawnerState peopleSpawner,
        MysteryBoxSpawnerState mysteryBoxSpawner,
        WheelState[] wheels,
        BalloonState[] balloons,
        PhotographerState[] photographers,
        SpawnerState[] spawners,
        InventoryState inventory,
        AnimalState[] animals,
        DataTableReference experienceData
    )
    {
        var serverTimestamp = message.ServerTimestamp;
        return new HarvestState(
            serverTimestamp,
            message.ClientAvatar.Unknown0,
            new GameTick(),
            random,
            new CommandExecutionState(),
            dataTableResolver,
            dataTableResolver.HighestTableId,
            message.ClientAvatar,
            new ShopEventManagerState(message.ClientAvatar.UnknownManager0),
            MapGameManagerState.Create(message.AvatarData.AvatarDataObjects.Common.MapGameManager),
            NeighborhoodObjectManagerState.Create(
                message.AvatarData.AvatarDataObjects.Common.NeighborhoodObjectManager
            ),
            message.AvatarData,
            inventory,
            experienceData,
            ExpansionReadyDataState.Resolve(message.Home, dataTableResolver),
            message.Home,
            environment.GameObjects,
            CarState.Resolve(environment.GameObjects, dataTableResolver),
            customizationManager,
            CreatureManagerState.Create(
                message.AvatarData.AvatarDataObjects.Common.CreatureManager,
                serverTimestamp
            ),
            ChronosEventManagerState.Create(
                message.AvatarData.AvatarDataObjects.Common.ChronosEvents,
                dataTableResolver,
                serverTimestamp
            ),
            environment.AmbientAnimals,
            environment.AmbientAnimalSpawner,
            PostmanState.Resolve(environment.GameObjects, dataTableResolver),
            FieldState.Resolve(environment.GameObjects, dataTableResolver),
            environment.AnimalHabitats,
            animals,
            environment.AnimalHabitatPieces,
            GathererHabitatState.Resolve(environment.GameObjects, dataTableResolver),
            GathererNestState.Resolve(environment.GameObjects, dataTableResolver),
            GathererState.Resolve(environment.GameObjects, dataTableResolver),
            HelperCharacterState.Resolve(environment.GameObjects, dataTableResolver),
            ConstructionBuildingState.Resolve(environment.GameObjects, dataTableResolver),
            BoyBoxState.Resolve(environment.GameObjects, dataTableResolver),
            photographers,
            personRoutes,
            people,
            peopleSpawner,
            mysteryBoxSpawner,
            OrderTableState.Resolve(environment.GameObjects, dataTableResolver),
            wheels,
            balloons,
            spawners,
            BoyState.Resolve(environment.GameObjects, dataTableResolver)
        );
    }

    public bool TryGetInventoryCount(DataTableReference data, out int count)
    {
        return Inventory.TryGetValue(0, data, out count);
    }

    public FieldState ReplaceHarvestedField(FieldState field)
    {
        var fieldIndex = Array.IndexOf(Fields, field);

        if (fieldIndex < 0)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {field.GlobalId} is not part of the authoritative home state."
                )
            );

        if (nextFieldGlobalId % DataTableResolver.GlobalIdTableSize is 0)
            throw new InvalidOperationException("Field instance IDs are exhausted.");

        var replacement = field.CreateEmptyReplacement(nextFieldGlobalId++);
        var removedIndex = Array.IndexOf(GameObjects, field.GameObject);
        var tableEndIndex = Array.FindLastIndex(
            GameObjects,
            static gameObject => gameObject.Data.TableId is 4
        );

        if (removedIndex < 0 || tableEndIndex < removedIndex)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {field.GlobalId} has invalid native table ordering."
                )
            );

        Array.Copy(
            GameObjects,
            removedIndex + 1,
            GameObjects,
            removedIndex,
            tableEndIndex - removedIndex
        );
        GameObjects[tableEndIndex] = replacement.GameObject;
        Fields[fieldIndex] = replacement;
        return replacement;
    }

    public void AdvanceInitialSimulation()
    {
        if (Tick.SubTick is not 0)
            return;

        Random.Reset(ServerTimestamp);
        AmbientAnimalState.Update(AmbientAnimals, Random);
        AmbientAnimalSpawner.Initialize(Random);
        AdvanceInitialPeopleAndObjects(updateCustomization: true);
        Tick.Advance();

        AmbientAnimalState.Update(AmbientAnimals, Random);
        AmbientAnimalSpawner.Update(Random);
        MysteryBoxSpawner.ReconcileLoadedBox(GameObjects, Random);
        ChronosEventManager.ReconcileInitialHomeObjects(Random);
        AdvanceInitialPeopleAndObjects(updateCustomization: false);
        Tick.Advance();

        MapGameManager.CompletePostLoadSetup(Random);
        AnimalState.CompletePostLoadSetup(Animals, Random);
        CreatureManager.CompleteInitialSimulation();

        return;

        void AdvanceInitialPeopleAndObjects(bool updateCustomization)
        {
            PeopleSpawner.UpdatePeople(People, PersonRoutes, Random, completeRemovalPass: false);
            PeopleSpawner.Update(ServerTimestamp, Random, People, completeRemovalPass: false);

            foreach (var spawner in Spawners)
                spawner.Update(Random, CustomizationManager.BuilderAvailable);

            foreach (var field in Fields)
                field.AdvanceSubTick();

            if (updateCustomization)
                CustomizationManager.Update(Random);
        }
    }

    public void AdvanceSimulationSubTick()
    {
        AnimalState.Update(Animals, Random);
        AmbientAnimalSpawner.UpdateRegisteredAnimals(Random);
        AmbientAnimalSpawner.Update(Random);
        PeopleSpawner.UpdatePeople(People, PersonRoutes, Random);
        PeopleSpawner.Update(ServerTimestamp, Random, People);

        foreach (var spawner in Spawners)
            spawner.Update(Random, CustomizationManager.BuilderAvailable);

        AmbientAnimalSpawner.CompleteRegisteredAnimalRemoval();

        foreach (var field in Fields)
            field.AdvanceSubTick();

        CustomizationManager.Update(Random);
        CreatureManager.Update(Random);
        Tick.Advance();
    }

    public void AdvanceSimulationTo(int subTick)
    {
        if (subTick < Tick.SubTick)
            throw new ArgumentOutOfRangeException(
                nameof(subTick),
                "Cannot rewind the authoritative simulation."
            );

        while (Tick.SubTick < subTick)
            AdvanceSimulationSubTick();
    }
}
