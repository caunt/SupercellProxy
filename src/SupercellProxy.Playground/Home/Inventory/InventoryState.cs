using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Network.Messages.Clientbound;

namespace SupercellProxy.Playground.Home;

internal sealed class InventoryState
{
    private readonly Dictionary<int, int>[] dataReferenceValues;

    private InventoryState(
        int[][] values,
        Dictionary<int, int>[] dataReferenceValues,
        int deprecatedDataCount,
        int unknown0
    )
    {
        Values = values;
        this.dataReferenceValues = dataReferenceValues;
        DeprecatedDataCount = deprecatedDataCount;
        Unknown0 = unknown0;
    }

    public int[][] Values { get; }
    public IReadOnlyDictionary<int, int>[] DataReferenceValues => dataReferenceValues;
    public int DeprecatedDataCount { get; }
    public int Unknown0 { get; }

    public static InventoryState Create(ClientAvatar clientAvatar)
    {
        var values = clientAvatar
            .InventoryValues.Select(static entries => entries.ToArray())
            .ToArray();
        var resolvedDataReferenceValues = clientAvatar
            .InventoryMaps.Select(CreateDataReferenceValues)
            .ToArray();

        return new InventoryState(
            values,
            resolvedDataReferenceValues,
            clientAvatar.DeprecatedInventoryDataCount,
            clientAvatar.InventoryUnknown0
        );
    }

    public bool TryGetValue(int inventoryIndex, DataTableReference data, out int value)
    {
        ValidateInventoryIndex(inventoryIndex);
        return dataReferenceValues[inventoryIndex].TryGetValue(data.GlobalId, out value);
    }

    public int GetTotalValue(DataTableReference data)
    {
        var total = 0;

        foreach (var values in dataReferenceValues)
        {
            if (values.TryGetValue(data.GlobalId, out var value))
                total = checked(total + value);
        }

        return total;
    }

    internal void Add(int inventoryIndex, DataTableReference data, int amount)
    {
        var newValue = GetValueAfterAdding(inventoryIndex, data, amount);
        var values = dataReferenceValues[inventoryIndex];

        if (newValue is 0)
            values.Remove(data.GlobalId);
        else
            values[data.GlobalId] = newValue;
    }

    internal void ValidateAdd(int inventoryIndex, DataTableReference data, int amount)
    {
        _ = GetValueAfterAdding(inventoryIndex, data, amount);
    }

    private int GetValueAfterAdding(int inventoryIndex, DataTableReference data, int amount)
    {
        ValidateInventoryIndex(inventoryIndex);

        var values = dataReferenceValues[inventoryIndex];
        values.TryGetValue(data.GlobalId, out var currentValue);
        var newValue = checked(currentValue + amount);

        if (newValue < 0)
            throw new InvalidOperationException(
                $"Inventory value {data.Name} cannot become negative."
            );

        return newValue;
    }

    private static Dictionary<int, int> CreateDataReferenceValues(DataReferenceValue[] entries)
    {
        var values = new Dictionary<int, int>();

        foreach (var entry in entries)
        {
            if (entry.GlobalDataId < DataTableResolver.GlobalIdTableSize)
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Invalid inventory data global ID {entry.GlobalDataId}."
                    )
                );

            if (entry.Value is 0)
                values.Remove(entry.GlobalDataId);
            else
                values[entry.GlobalDataId] = entry.Value;
        }

        return values;
    }

    private void ValidateInventoryIndex(int inventoryIndex)
    {
        if (
            uint.CreateTruncating(inventoryIndex)
            >= uint.CreateTruncating(dataReferenceValues.Length)
        )
            throw new ArgumentOutOfRangeException(nameof(inventoryIndex));
    }
}
