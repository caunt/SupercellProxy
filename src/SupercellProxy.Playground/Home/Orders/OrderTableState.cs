using System.Globalization;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record OrderTableState(GameObjectState GameObject, OrderState[] Orders)
{
    public static OrderTableState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        const string orderTablesFile = "data/order_tables.csv";

        if (!dataTableResolver.TryGetTableId(orderTablesFile, out var orderTableId))
            throw new InvalidOperationException(
                $"{orderTablesFile} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == orderTableId)
            .Select(gameObject => Create(gameObject, dataTableResolver))
            .ToArray();
    }

    private static OrderTableState Create(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver
    )
    {
        var snapshots = gameObject.Snapshot.Orders;

        if (snapshots.Length is not 9)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Order table {gameObject.GlobalId} has {snapshots.Length} orders instead of 9."
                )
            );

        return new OrderTableState(
            gameObject,
            snapshots
                .Select((snapshot, slot) => OrderState.Create(slot, snapshot, dataTableResolver))
                .ToArray()
        );
    }
}
