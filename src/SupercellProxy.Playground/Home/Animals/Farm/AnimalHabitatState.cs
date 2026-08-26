using System.Globalization;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record AnimalHabitatState(
    GameObjectState GameObject,
    int PieceCount,
    int AnimalCount
)
{
    private const string PieceField = "Piece";

    public int PieceAndAnimalCount => PieceCount + AnimalCount;

    public static AnimalHabitatState[] Resolve(
        HomeSnapshot home,
        GameObjectState[] gameObjects,
        AnimalHabitatPieceState[] animalHabitatPieces,
        DataTableResolver dataTableResolver
    )
    {
        const string animalHabitatsFile = "data/animal_habitats.csv";
        const string animalsFile = "data/animals.csv";

        if (!dataTableResolver.TryGetTableId(animalHabitatsFile, out var animalHabitatTableId))
            throw new InvalidOperationException(
                $"{animalHabitatsFile} is not registered as a native data table."
            );

        if (!dataTableResolver.TryGetTableId(animalsFile, out var animalTableId))
            throw new InvalidOperationException(
                $"{animalsFile} is not registered as a native data table."
            );

        var animalHabitats = gameObjects
            .Where(gameObject => gameObject.Data.TableId == animalHabitatTableId)
            .ToArray();
        var animals = gameObjects
            .Where(gameObject => gameObject.Data.TableId == animalTableId)
            .ToArray();
        var animalSnapshots = home
            .Objects.Where(gameObject =>
                gameObject.DataGlobalId / DataTableResolver.GlobalIdTableSize == animalTableId
            )
            .ToArray();

        if (animals.Length != animalSnapshots.Length)
            throw new InvalidDataException(
                $"Resolved {animals.Length} animals from {animalSnapshots.Length} snapshots."
            );

        var animalCounts = AttachAnimals(animalHabitats, animals, animalSnapshots);
        return CreateStates(animalHabitats, animalHabitatPieces, animalCounts, dataTableResolver);
    }

    private static int[] AttachAnimals(
        IReadOnlyList<GameObjectState> animalHabitats,
        IReadOnlyList<GameObjectState> animals,
        IReadOnlyList<GameObjectSnapshot> animalSnapshots
    )
    {
        var animalCounts = new int[animalHabitats.Count];
        for (var i = 0; i < animals.Count; i++)
        {
            var animal = animalSnapshots[i];
            var animalHabitatIndex = animal.Data.TryGetValue("AnimalHabitatIndex", out var value)
                ? value.GetInt32()
                : 0;

            if (
                uint.CreateTruncating(animalHabitatIndex)
                >= uint.CreateTruncating(animalCounts.Length)
            )
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Animal references missing habitat index {animalHabitatIndex}."
                    )
                );

            animalCounts[animalHabitatIndex]++;
            animals[i].AttachTo(animalHabitats[animalHabitatIndex]);
            animals[i].MoveTo(animals[i].PositionX | 0x100, animals[i].PositionY | 0x100);
        }

        return animalCounts;
    }

    private static AnimalHabitatState[] CreateStates(
        IReadOnlyList<GameObjectState> animalHabitats,
        IReadOnlyList<AnimalHabitatPieceState> animalHabitatPieces,
        IReadOnlyList<int> animalCounts,
        DataTableResolver dataTableResolver
    )
    {
        var states = new AnimalHabitatState[animalHabitats.Count];

        for (var i = 0; i < states.Length; i++)
        {
            var animalHabitat = animalHabitats[i];

            if (
                !dataTableResolver.TryResolveValueCount(
                    animalHabitat.Data.GlobalId,
                    PieceField,
                    out var configuredPieceCount
                )
            )
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Animal habitat {animalHabitat.Data.GlobalId} has no {PieceField} data."
                    )
                );

            var pieceCount = animalHabitatPieces.Count(piece =>
                piece.AnimalHabitatGlobalId == animalHabitat.GlobalId
            );

            if (pieceCount != configuredPieceCount)
            {
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Animal habitat {animalHabitat.GlobalId} has {pieceCount} pieces instead of {configuredPieceCount}."
                    )
                );
            }

            states[i] = new AnimalHabitatState(animalHabitat, pieceCount, animalCounts[i]);
        }

        return states;
    }
}
