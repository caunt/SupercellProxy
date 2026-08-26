using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record PhotographerState(
    GameObjectState GameObject,
    int State,
    int ChecksumState0,
    int NextPoint,
    int ChecksumState1,
    int ChecksumState2,
    bool ChecksumFlag0,
    bool ChecksumFlag1,
    IntPair ChecksumPair0,
    IntPair ChecksumPair1,
    IntPair ChecksumPair2,
    bool HasParent,
    IntPair[]? ChecksumPoints0,
    IntPair[]? ChecksumPoints1
)
{
    public static PhotographerState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        const string photographerFile = "data/photographer.csv";

        if (!dataTableResolver.TryGetTableId(photographerFile, out var photographerTableId))
            throw new InvalidOperationException(
                $"{photographerFile} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == photographerTableId)
            .Select(CreateInitial)
            .ToArray();
    }

    private static PhotographerState CreateInitial(GameObjectState gameObject)
    {
        var snapshot = gameObject.Snapshot;

        if (
            snapshot.State is not 0 and not 2 and not 6
            || snapshot.NextPoint is < 0 or > 1
            || snapshot.LinkedGlobalId is not 0
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Photographer {gameObject.GlobalId} has unsupported initial state: State={snapshot.State}, NextPoint={snapshot.NextPoint}, LinkedGlobalId={snapshot.LinkedGlobalId}."
                )
            );
        }

        return new PhotographerState(
            gameObject,
            snapshot.State,
            0,
            snapshot.NextPoint,
            -1,
            -1,
            ChecksumFlag0: false,
            ChecksumFlag1: false,
            default,
            default,
            default,
            HasParent: false,
            ChecksumPoints0: null,
            ChecksumPoints1: null
        );
    }
}
