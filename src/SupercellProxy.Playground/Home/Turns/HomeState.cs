using System.Globalization;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Messages.Clientbound;

namespace SupercellProxy.Playground.Home;

internal sealed record HomeState(
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
    DecorationEventManagerState DecorationEventManager,
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
    private int _nextFieldGlobalId =
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

    public static HomeState Create(OwnHomeDataMessage message, DataTableResolver dataTableResolver)
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
            photographers,
            inventory
        );
        var animals = AnimalState.Resolve(
            gameObjects,
            environment.AnimalHabitats,
            environment.AnimalHabitatPieces,
            dataTableResolver
        );

        if (
            !dataTableResolver.TryResolve(GameAssetFiles.Money, "ExpPoints", out var experienceData)
        )
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

    private static HomeState CreateState(
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
        var commonAvatarData = message.AvatarData.AvatarDataObjects.Common;
        return new HomeState(
            serverTimestamp,
            message.ClientAvatar.Unknown0,
            new GameTick(),
            random,
            new CommandExecutionState(),
            dataTableResolver,
            dataTableResolver.HighestTableId,
            message.ClientAvatar,
            new ShopEventManagerState(message.ClientAvatar.UnknownManager0),
            MapGameManagerState.Create(commonAvatarData.MapGameManager),
            NeighborhoodObjectManagerState.Create(commonAvatarData.NeighborhoodObjectManager),
            message.AvatarData,
            inventory,
            experienceData,
            ExpansionReadyDataState.Resolve(message.Home, dataTableResolver),
            message.Home,
            environment.GameObjects,
            CarState.Resolve(environment.GameObjects, dataTableResolver),
            customizationManager,
            CreatureManagerState.Create(commonAvatarData.CreatureManager, serverTimestamp),
            ChronosEventManagerState.Create(
                commonAvatarData.ChronosEvents,
                dataTableResolver,
                serverTimestamp
            ),
            DecorationEventManagerState.Create(commonAvatarData.DecorationEventManager),
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

        if (_nextFieldGlobalId % DataTableResolver.GlobalIdTableSize is 0)
            throw new InvalidOperationException("Field instance IDs are exhausted.");

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
        var replacement = field.CreateEmptyReplacement(_nextFieldGlobalId++);
        GameObjects[tableEndIndex] = replacement.GameObject;
        Fields[fieldIndex] = replacement;
        return replacement;
    }

    public void AdvanceInitialSimulation(
        Action<InitialHomeStage, int>? recordRandomCheckpoint = null,
        Action<int, AmbientAnimalState, int, int, int[]>? recordAmbientUpdate = null
    )
    {
        if (Tick.SubTick is not 0)
            return;

        Random.Reset(ServerTimestamp);
        Record(InitialHomeStage.RandomReset);
        AmbientAnimalState.Update(
            AmbientAnimals,
            Random,
            (animal, before, after, bounds) =>
                recordAmbientUpdate?.Invoke(1, animal, before, after, bounds)
        );
        Record(InitialHomeStage.FirstAmbientUpdate);
        AmbientAnimalSpawner.Initialize(Random);
        Record(InitialHomeStage.AmbientSpawnerInitialization);
        AdvanceInitialPeopleAndObjects(updateCustomization: true);
        Record(InitialHomeStage.FirstObjectAndManagerUpdate);
        Tick.Advance();

        AmbientAnimalState.Update(
            AmbientAnimals,
            Random,
            (animal, before, after, bounds) =>
                recordAmbientUpdate?.Invoke(2, animal, before, after, bounds)
        );
        Record(InitialHomeStage.SecondAmbientUpdate);
        AmbientAnimalSpawner.Update(Random);
        Record(InitialHomeStage.SecondAmbientSpawnerUpdate);
        ChronosEventManager.ReconcileInitialHomeObjects(Random);
        Record(InitialHomeStage.EventReconciliation);
        AdvanceInitialPeopleAndObjects(updateCustomization: false);
        Record(InitialHomeStage.SecondObjectUpdate);
        Tick.Advance();

        MapGameManager.CompletePostLoadSetup(Random);
        AnimalState.CompletePostLoadSetup(Animals, Random);
        CreatureManager.CompleteInitialSimulation();
        Record(InitialHomeStage.Completed);

        return;

        void Record(InitialHomeStage stage) => recordRandomCheckpoint?.Invoke(stage, Random.Calls);

        void AdvanceInitialPeopleAndObjects(bool updateCustomization)
        {
            PeopleSpawner.UpdatePeople(People, PersonRoutes, Random);
            Record(
                updateCustomization
                    ? InitialHomeStage.FirstPeopleUpdate
                    : InitialHomeStage.SecondPeopleUpdate
            );
            PeopleSpawner.Update(ServerTimestamp, Random, People);
            Record(
                updateCustomization
                    ? InitialHomeStage.FirstPeopleSpawnerUpdate
                    : InitialHomeStage.SecondPeopleSpawnerUpdate
            );

            MysteryBoxSpawner.NormalUpdate(GameObjects);

            foreach (var spawner in Spawners)
                spawner.Update(Random, CustomizationManager.BuilderAvailable);
            Record(
                updateCustomization
                    ? InitialHomeStage.FirstSpawnerUpdate
                    : InitialHomeStage.SecondSpawnerUpdate
            );

            foreach (var field in Fields)
                field.AdvanceSubTick();

            MysteryBoxSpawner.PreUpdate(Random, Inventory);
            Record(
                updateCustomization
                    ? InitialHomeStage.FirstMysteryBoxSpawnerPreUpdate
                    : InitialHomeStage.SecondMysteryBoxSpawnerPreUpdate
            );

            if (updateCustomization)
            {
                CustomizationManager.Update(Random);
                Record(InitialHomeStage.CustomizationUpdate);
            }
        }
    }

    public void AdvanceSimulationSubTick()
    {
        AnimalState.Update(Animals, Random);
        AmbientAnimalSpawner.UpdateRegisteredAnimals(Random);
        AmbientAnimalSpawner.Update(Random);
        PeopleSpawner.UpdatePeople(People, PersonRoutes, Random);
        PeopleSpawner.Update(ServerTimestamp, Random, People);
        MysteryBoxSpawner.NormalUpdate(GameObjects);

        foreach (var spawner in Spawners)
            spawner.Update(Random, CustomizationManager.BuilderAvailable);

        AmbientAnimalSpawner.CompleteRegisteredAnimalRemoval();

        foreach (var field in Fields)
            field.AdvanceSubTick();

        MysteryBoxSpawner.PreUpdate(Random, Inventory);
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
