using System.Globalization;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record BoyBoxState(GameObjectState GameObject, DataTableReference? Item, int Count)
{
    public static BoyBoxState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        const string boyBoxFile = "data/boy_box.csv";

        if (!dataTableResolver.TryGetTableId(boyBoxFile, out var boyBoxTableId))
            throw new InvalidOperationException(
                $"{boyBoxFile} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == boyBoxTableId)
            .Select(gameObject => Create(gameObject, dataTableResolver))
            .ToArray();
    }

    private static BoyBoxState Create(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver
    )
    {
        var itemGlobalId = gameObject.Snapshot.ItemGlobalId;
        DataTableReference? item = null;

        if (itemGlobalId is not 0 && !dataTableResolver.TryResolve(itemGlobalId, out item))
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Boy box {gameObject.GlobalId} has unresolved ItemID {itemGlobalId}."
                )
            );

        return new BoyBoxState(gameObject, item, gameObject.Snapshot.Count);
    }
}
