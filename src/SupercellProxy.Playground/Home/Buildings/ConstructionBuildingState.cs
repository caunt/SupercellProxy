using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record ConstructionBuildingState(
    GameObjectState GameObject,
    TimerSnapshot ConstructionTimer,
    bool ChecksumFlag0,
    DataTableReference? TargetData
)
{
    public static ConstructionBuildingState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryGetTableId(
                GameAssetFiles.ConstructionBuildings,
                out var constructionBuildingTableId
            )
        )
            throw new InvalidOperationException(
                $"{GameAssetFiles.ConstructionBuildings} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == constructionBuildingTableId)
            .Select(gameObject => Create(gameObject, dataTableResolver))
            .ToArray();
    }

    private static ConstructionBuildingState Create(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver
    )
    {
        var targetGlobalId = gameObject.Snapshot.TargetData;
        DataTableReference? targetData = null;

        if (
            targetGlobalId is not 0
            && !dataTableResolver.TryResolve(targetGlobalId, out targetData)
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Construction building {gameObject.GlobalId} has unresolved TargetData {targetGlobalId}."
                )
            );

        return new ConstructionBuildingState(
            gameObject,
            TimerSnapshot.Decode(gameObject.Snapshot.ConstructionTimer),
            ChecksumFlag0: false,
            targetData
        );
    }
}
