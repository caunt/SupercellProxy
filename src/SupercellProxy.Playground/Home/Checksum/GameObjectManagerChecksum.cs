using System.Globalization;

namespace SupercellProxy.Playground.Home.Checksum;

internal static class GameObjectManagerChecksum
{
    private static readonly int[] OwnHomeSecondaryTableIds =
    {
        49,
        35,
        21,
        18,
        14,
        13,
        12,
        11,
        10,
        6,
        4,
        2,
    };

    private static readonly HashSet<int> SecondaryBaseTableIds = new()
    {
        3,
        5,
        12,
        13,
        14,
        50,
        53,
        54,
        58,
        112,
    };

    private static readonly HashSet<int> SecondaryUpgradeableTableIds = new()
    {
        6,
        10,
        18,
        35,
        40,
        56,
        73,
        88,
        91,
        103,
        124,
        163,
        174,
        175,
        178,
        182,
        204,
        223,
        256,
        321,
    };

    public static void EncodeSecondary(ChecksumEncoder encoder, HomeState state)
    {
        var gameObjects = state
            .GameObjects.Where(gameObject =>
                gameObject.Data.TableId is not 49
                || state.PeopleSpawner.IsPersonRegistered(gameObject.GlobalId)
            )
            .ToLookup(static gameObject => gameObject.Data.TableId);
        var animalHabitats = state.AnimalHabitats.ToDictionary(static habitat =>
            habitat.GameObject.GlobalId
        );
        var people = state.People.ToDictionary(static person => person.GameObject.GlobalId);

        foreach (var tableId in OwnHomeSecondaryTableIds)
        {
            foreach (var gameObject in gameObjects[tableId])
            {
                if (tableId is 49 && people.TryGetValue(gameObject.GlobalId, out var person))
                    GameObjectChecksum.EncodePrimaryPerson(encoder, person);
                else if (
                    tableId is 11
                    && animalHabitats.TryGetValue(gameObject.GlobalId, out var animalHabitat)
                )
                    GameObjectChecksum.EncodeAnimalHabitat(encoder, animalHabitat);
                else
                    GameObjectChecksum.EncodeBase(encoder, gameObject);
            }
        }
    }

    public static void EncodeFull(ChecksumEncoder encoder, HomeState state)
    {
        var gameObjects = state
            .GameObjects.Where(gameObject =>
                gameObject.Data.TableId is not 49
                || state.PeopleSpawner.IsPersonRegistered(gameObject.GlobalId)
            )
            .ToLookup(static gameObject => gameObject.Data.TableId);
        var animalHabitats = state.AnimalHabitats.ToDictionary(static habitat =>
            habitat.GameObject.GlobalId
        );

        for (var tableId = 2; tableId <= state.HighestDataTableId; tableId++)
        {
            var tableObjects = gameObjects[tableId];
            encoder.WriteInt32(tableObjects.Count());

            foreach (var gameObject in tableObjects)
            {
                switch (gameObject.Data.TableId)
                {
                    case 3:
                    case 5:
                    case 13:
                    case 14:
                    case 50:
                        GameObjectChecksum.EncodeBase(encoder, gameObject);
                        break;
                    case 11
                        when animalHabitats.TryGetValue(gameObject.GlobalId, out var animalHabitat):
                        GameObjectChecksum.EncodeAnimalHabitat(encoder, animalHabitat);
                        break;
                    default:
                        throw CreateUnsupportedException(gameObject, "full");
                }
            }
        }
    }

    internal static void EncodeSecondaryGameObject(
        ChecksumEncoder encoder,
        GameObjectState gameObject,
        IReadOnlyDictionary<int, CarState> cars,
        IReadOnlyDictionary<int, FieldState> fields,
        IReadOnlyDictionary<int, AmbientAnimalState> ambientAnimals,
        AmbientAnimalSpawnerState ambientAnimalSpawner,
        IReadOnlyDictionary<int, AnimalHabitatState> animalHabitats,
        IReadOnlyDictionary<int, GathererHabitatState> gathererHabitats,
        IReadOnlyDictionary<int, GathererNestState> gathererNests,
        IReadOnlyDictionary<int, GathererState> gatherers,
        IReadOnlyDictionary<int, HelperCharacterState> helperCharacters,
        IReadOnlyDictionary<int, ConstructionBuildingState> constructionBuildings,
        IReadOnlyDictionary<int, BoyBoxState> boyBoxes,
        IReadOnlyDictionary<int, PhotographerState> photographers,
        IReadOnlyDictionary<int, PersonState> people,
        IReadOnlyDictionary<int, OrderTableState> orderTables,
        IReadOnlyDictionary<int, WheelState> wheels,
        IReadOnlyDictionary<int, SpawnerState> spawners,
        IReadOnlyDictionary<int, BoyState> boys
    )
    {
        var tableId = gameObject.Data.TableId;
        if (
            TryEncodeSecondaryCore(
                encoder,
                gameObject,
                cars,
                fields,
                ambientAnimals,
                ambientAnimalSpawner,
                animalHabitats
            )
            || TryEncodeSecondaryCharacters(
                encoder,
                gameObject,
                gathererHabitats,
                gathererNests,
                gatherers,
                helperCharacters,
                constructionBuildings,
                boyBoxes,
                photographers,
                people
            )
            || TryEncodeSecondaryActivities(
                encoder,
                gameObject,
                orderTables,
                wheels,
                spawners,
                boys
            )
        )
        {
            return;
        }

        if (SecondaryBaseTableIds.Contains(tableId))
        {
            GameObjectChecksum.EncodeSecondaryBase(encoder, gameObject);
            return;
        }

        if (SecondaryUpgradeableTableIds.Contains(tableId))
        {
            GameObjectChecksum.EncodeSecondaryUpgradeable(encoder, gameObject);
            return;
        }

        throw CreateUnsupportedException(gameObject, "secondary");
    }

    private static bool TryEncodeSecondaryCore(
        ChecksumEncoder encoder,
        GameObjectState gameObject,
        IReadOnlyDictionary<int, CarState> cars,
        IReadOnlyDictionary<int, FieldState> fields,
        IReadOnlyDictionary<int, AmbientAnimalState> ambientAnimals,
        AmbientAnimalSpawnerState ambientAnimalSpawner,
        IReadOnlyDictionary<int, AnimalHabitatState> animalHabitats
    )
    {
        var tableId = gameObject.Data.TableId;
        if (tableId is 12 or 13)
            GameObjectChecksum.EncodeSecondaryBase(encoder, gameObject);
        else if (tableId is 2 && cars.TryGetValue(gameObject.GlobalId, out var car))
            GameObjectChecksum.EncodeSecondaryCar(encoder, car);
        else if (tableId is 4 && fields.TryGetValue(gameObject.GlobalId, out var field))
            GameObjectChecksum.EncodeSecondaryField(encoder, field);
        else if (
            tableId is 45
            && ambientAnimals.TryGetValue(gameObject.GlobalId, out var ambientAnimal)
        )
            GameObjectChecksum.EncodeSecondaryAmbientAnimal(encoder, ambientAnimal);
        else if (tableId is 46 && gameObject.GlobalId == ambientAnimalSpawner.GameObject.GlobalId)
            GameObjectChecksum.EncodeSecondaryAmbientAnimalSpawner(encoder, ambientAnimalSpawner);
        else if (
            tableId is 11
            && animalHabitats.TryGetValue(gameObject.GlobalId, out var animalHabitat)
        )
            GameObjectChecksum.EncodeSecondaryAnimalHabitat(encoder, animalHabitat);
        else if (tableId is 90)
            GameObjectChecksum.EncodeSecondaryPostman(encoder, gameObject);
        else
            return false;

        return true;
    }

    private static bool TryEncodeSecondaryCharacters(
        ChecksumEncoder encoder,
        GameObjectState gameObject,
        IReadOnlyDictionary<int, GathererHabitatState> gathererHabitats,
        IReadOnlyDictionary<int, GathererNestState> gathererNests,
        IReadOnlyDictionary<int, GathererState> gatherers,
        IReadOnlyDictionary<int, HelperCharacterState> helperCharacters,
        IReadOnlyDictionary<int, ConstructionBuildingState> constructionBuildings,
        IReadOnlyDictionary<int, BoyBoxState> boyBoxes,
        IReadOnlyDictionary<int, PhotographerState> photographers,
        IReadOnlyDictionary<int, PersonState> people
    )
    {
        var tableId = gameObject.Data.TableId;
        if (tableId is 147 && gathererNests.TryGetValue(gameObject.GlobalId, out var gathererNest))
            GameObjectChecksum.EncodeSecondaryGathererNest(encoder, gathererNest);
        else if (
            tableId is 146
            && gathererHabitats.TryGetValue(gameObject.GlobalId, out var gathererHabitat)
        )
            GameObjectChecksum.EncodeSecondaryGathererHabitat(encoder, gathererHabitat);
        else if (tableId is 149 && gatherers.TryGetValue(gameObject.GlobalId, out var gatherer))
            GameObjectChecksum.EncodeSecondaryGatherer(encoder, gatherer);
        else if (
            tableId is 181
            && helperCharacters.TryGetValue(gameObject.GlobalId, out var helperCharacter)
        )
            GameObjectChecksum.EncodeSecondaryHelperCharacter(encoder, helperCharacter);
        else if (
            tableId is 21
            && constructionBuildings.TryGetValue(gameObject.GlobalId, out var building)
        )
            GameObjectChecksum.EncodeSecondaryConstructionBuilding(encoder, building);
        else if (tableId is 71 && boyBoxes.TryGetValue(gameObject.GlobalId, out var boyBox))
            GameObjectChecksum.EncodeSecondaryBoyBox(encoder, boyBox);
        else if (
            tableId is 297
            && photographers.TryGetValue(gameObject.GlobalId, out var photographer)
        )
            GameObjectChecksum.EncodeSecondaryPhotographer(encoder, photographer);
        else if (tableId is 49 && people.TryGetValue(gameObject.GlobalId, out var person))
            GameObjectChecksum.EncodeSecondaryPerson(encoder, person);
        else
            return false;

        return true;
    }

    private static bool TryEncodeSecondaryActivities(
        ChecksumEncoder encoder,
        GameObjectState gameObject,
        IReadOnlyDictionary<int, OrderTableState> orderTables,
        IReadOnlyDictionary<int, WheelState> wheels,
        IReadOnlyDictionary<int, SpawnerState> spawners,
        IReadOnlyDictionary<int, BoyState> boys
    )
    {
        var tableId = gameObject.Data.TableId;
        if (tableId is 32 && orderTables.TryGetValue(gameObject.GlobalId, out var orderTable))
            GameObjectChecksum.EncodeSecondaryOrderTable(encoder, orderTable);
        else if (tableId is 74 && wheels.TryGetValue(gameObject.GlobalId, out var wheel))
            GameObjectChecksum.EncodeSecondaryWheel(encoder, wheel);
        else if (
            tableId is 208 or 296
            && spawners.TryGetValue(gameObject.GlobalId, out var spawner)
        )
            GameObjectChecksum.EncodeSecondarySpawner(encoder, spawner);
        else if (tableId is 70 && boys.TryGetValue(gameObject.GlobalId, out var boy))
            GameObjectChecksum.EncodeSecondaryBoy(encoder, boy);
        else
            return false;

        return true;
    }

    private static InvalidOperationException CreateUnsupportedException(
        GameObjectState gameObject,
        string checksum
    )
    {
        return new InvalidOperationException(
            string.Create(
                CultureInfo.InvariantCulture,
                $"The {checksum} checksum for game-object table {gameObject.Data.TableId} ({gameObject.Data.File}) is not implemented."
            )
        );
    }
}
