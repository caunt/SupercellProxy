using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
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
        if (
            !dataTableResolver.TryGetTableId(
                GameAssetFiles.AnimalHabitats,
                out var animalHabitatTableId
            )
        )
            throw new InvalidOperationException(
                $"{GameAssetFiles.AnimalHabitats} is not registered as a native data table."
            );

        if (!dataTableResolver.TryGetTableId(GameAssetFiles.Animals, out var animalTableId))
            throw new InvalidOperationException(
                $"{GameAssetFiles.Animals} is not registered as a native data table."
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
        GameObjectState[] animalHabitats,
        GameObjectState[] animals,
        GameObjectSnapshot[] animalSnapshots
    )
    {
        var animalCounts = new int[animalHabitats.Length];
        for (var i = 0; i < animals.Length; i++)
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
            animals[i]
                .MoveTo(
                    animals[i].PositionX | GameObjectState.TileCenter,
                    animals[i].PositionY | GameObjectState.TileCenter
                );
        }

        return animalCounts;
    }

    private static AnimalHabitatState[] CreateStates(
        GameObjectState[] animalHabitats,
        IReadOnlyList<AnimalHabitatPieceState> animalHabitatPieces,
        int[] animalCounts,
        DataTableResolver dataTableResolver
    )
    {
        var states = new AnimalHabitatState[animalHabitats.Length];

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
