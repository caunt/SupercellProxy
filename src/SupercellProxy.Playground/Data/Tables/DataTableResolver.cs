using System.Diagnostics.CodeAnalysis;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Assets;

namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c language="csharp">DataTableResolver</c>.
/// </summary>
internal sealed class DataTableResolver : ICommandDataResolver
{
    /// <summary>
    /// Defines the <c language="csharp">GlobalIdTableSize</c> value.
    /// </summary>
    public const int GlobalIdTableSize = 100000;

    private readonly Dictionary<int, (GameAsset GameAsset, GameDataTable Table)> _dataTables;
    private readonly Dictionary<string, GameAsset> _resourcesByFile;
    private readonly Dictionary<string, int> _tableIdsByFile;

    /// <summary>
    /// Gets the <c language="csharp">HighestTableId</c> value.
    /// </summary>
    public int HighestTableId { get; }

    /// <summary>
    /// Initializes a new <see cref="DataTableResolver"/> instance.
    /// </summary>
    public DataTableResolver(IEnumerable<GameAsset> resources)
    {
        _resourcesByFile = resources.ToDictionary(
            static resource => resource.Fingerprint.File,
            StringComparer.Ordinal
        );
        var resolvedDataTables = new Dictionary<int, (GameAsset GameAsset, GameDataTable Table)>();

        foreach (var (tableId, file) in DataTableRegistry.Create(resources))
        {
            if (!_resourcesByFile.TryGetValue(file, out var resource))
                throw new InvalidOperationException($"GameAsset {file} was not downloaded.");

            if (!resource.TryGetTable(out var table))
                throw new InvalidOperationException($"Failed to parse {file}.");

            resolvedDataTables.Add(tableId, (resource, table));
        }

        _dataTables = resolvedDataTables;
        HighestTableId = resolvedDataTables.Keys.Max();
        _tableIdsByFile = resolvedDataTables.ToDictionary(
            static entry => entry.Value.GameAsset.Fingerprint.File,
            static entry => entry.Key,
            StringComparer.Ordinal
        );
    }

    /// <summary>
    /// Attempts the <c language="csharp">GetTableId</c> operation.
    /// </summary>
    public bool TryGetTableId(string file, out int tableId)
    {
        return _tableIdsByFile.TryGetValue(file, out tableId);
    }

    /// <summary>
    /// Attempts the <c language="csharp">GetTableEntryCount</c> operation.
    /// </summary>
    public bool TryGetTableEntryCount(string file, out int count)
    {
        count = default;

        if (
            !_tableIdsByFile.TryGetValue(file, out var tableId)
            || !_dataTables.TryGetValue(tableId, out var dataTable)
        )
        {
            return false;
        }

        count = dataTable.Table.Entries.Count;
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">Resolve</c> operation.
    /// </summary>
    public bool TryResolve(int globalId, [NotNullWhen(true)] out DataTableReference? reference)
    {
        if (globalId < GlobalIdTableSize)
        {
            reference = null;
            return false;
        }

        var tableId = globalId / GlobalIdTableSize;
        var rowIndex = globalId % GlobalIdTableSize;

        if (
            !_dataTables.TryGetValue(tableId, out var dataTable)
            || rowIndex >= dataTable.Table.Entries.Count
        )
        {
            reference = null;
            return false;
        }

        var name = dataTable.Table.Entries[rowIndex].Name;

        if (string.IsNullOrWhiteSpace(name))
        {
            reference = null;
            return false;
        }

        reference = new DataTableReference(
            globalId,
            tableId,
            rowIndex,
            name,
            dataTable.GameAsset.Fingerprint.File,
            dataTable.GameAsset.Fingerprint.Sha
        );
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">Resolve</c> operation.
    /// </summary>
    public bool TryResolve(
        string file,
        string name,
        [NotNullWhen(true)] out DataTableReference? reference
    )
    {
        reference = null;

        if (
            !_tableIdsByFile.TryGetValue(file, out var tableId)
            || !_dataTables.TryGetValue(tableId, out var dataTable)
        )
            return false;

        var rowIndex = -1;

        for (var i = 0; i < dataTable.Table.Entries.Count; i++)
        {
            if (!dataTable.Table.Entries[i].Name.Equals(name, StringComparison.Ordinal))
                continue;

            if (rowIndex is not -1)
                throw new InvalidDataException(
                    $"Data table {file} contains duplicate entry name {name}."
                );

            rowIndex = i;
        }

        return rowIndex is not -1
            && TryResolve(tableId * GlobalIdTableSize + rowIndex, out reference);
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    public bool TryResolveString(
        int globalId,
        string fieldName,
        [NotNullWhen(true)] out string? value
    )
    {
        value = null;

        if (
            !TryResolveTableEntry(globalId, out var entry)
            || !entry.BaseRow.TryGetValue(fieldName, out var fieldValue)
            || fieldValue is not string stringValue
        )
        {
            return false;
        }

        value = stringValue;
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    public bool TryResolveString(
        int globalId,
        string fieldName,
        int valueIndex,
        [NotNullWhen(true)] out string? value
    )
    {
        value = null;

        if (
            !TryResolveTableEntry(globalId, out var entry)
            || !TryResolveIndexedValue(entry, fieldName, valueIndex, out var fieldValue)
        )
        {
            return false;
        }

        if (fieldValue is null)
        {
            value = string.Empty;
            return true;
        }

        if (fieldValue is string stringValue)
        {
            value = stringValue;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    public bool TryResolveString(
        string file,
        string name,
        string fieldName,
        int valueIndex,
        [NotNullWhen(true)] out string? value
    )
    {
        value = null;

        if (
            !TryResolveTableEntry(file, name, out var entry)
            || !TryResolveIndexedValue(entry, fieldName, valueIndex, out var fieldValue)
        )
        {
            return false;
        }

        if (fieldValue is null)
        {
            value = string.Empty;
            return true;
        }

        if (fieldValue is string stringValue)
        {
            value = stringValue;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(int globalId, string fieldName, out int value)
    {
        value = default;

        if (
            !TryResolveTableEntry(globalId, out var entry)
            || !entry.BaseRow.TryGetValue(fieldName, out var fieldValue)
            || fieldValue is not int intValue
        )
        {
            return false;
        }

        value = intValue;
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(string file, int physicalRowIndex, string fieldName, out int value)
    {
        value = default;

        if (
            physicalRowIndex < 0
            || !TryResolvePhysicalRows(file, out var rows)
            || physicalRowIndex >= rows.Count
            || !rows[physicalRowIndex].TryGetValue(fieldName, out var fieldValue)
            || fieldValue is not int intValue
        )
        {
            return false;
        }

        value = intValue;
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(string file, string name, string fieldName, out int value)
    {
        value = default;

        if (
            !_resourcesByFile.TryGetValue(file, out var resource)
            || !resource.TryGetTable(out var table)
        )
            return false;

        var entries = table
            .Entries.Where(entry => entry.Name.Equals(name, StringComparison.Ordinal))
            .ToArray();

        if (entries.Length > 1)
            throw new InvalidDataException(
                $"Data table {file} contains duplicate entry name {name}."
            );

        if (
            entries.Length is not 1
            || !entries[0].BaseRow.TryGetValue(fieldName, out var fieldValue)
            || fieldValue is not int intValue
        )
        {
            return false;
        }

        value = intValue;
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(
        string file,
        string name,
        string fieldName,
        int valueIndex,
        out int value
    )
    {
        value = default;

        if (
            !TryResolveTableEntry(file, name, out var entry)
            || !TryResolveIndexedValue(entry, fieldName, valueIndex, out var fieldValue)
        )
        {
            return false;
        }

        if (fieldValue is null)
            return true;

        if (fieldValue is int intValue)
        {
            value = intValue;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveValueCount</c> operation.
    /// </summary>
    public bool TryResolveValueCount(string file, string name, string fieldName, out int count)
    {
        count = default;

        if (
            !TryResolveTableEntry(file, name, out var entry)
            || !entry.Snapshots.Any(row => row.ContainsKey(fieldName))
        )
        {
            return false;
        }

        count = entry.Snapshots.Count(row => row.ContainsKey(fieldName));
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolvePhysicalRowCount</c> operation.
    /// </summary>
    public bool TryResolvePhysicalRowCount(string file, out int count)
    {
        count = default;

        if (!TryResolvePhysicalRows(file, out var rows))
            return false;

        count = rows.Count;
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveBoolean</c> operation.
    /// </summary>
    public bool TryResolveBoolean(int globalId, string fieldName, out bool value)
    {
        value = default;

        if (
            !TryResolveTableEntry(globalId, out var entry)
            || !entry.BaseRow.TryGetValue(fieldName, out var fieldValue)
            || fieldValue is not bool booleanValue
        )
        {
            return false;
        }

        value = booleanValue;
        return true;
    }

    /// <summary>
    /// Attempts to resolve a Boolean field from a named row in any loaded data-table resource,
    /// including resources that do not have a global table identifier.
    /// </summary>
    public bool TryResolveBoolean(string file, string name, string fieldName, out bool value)
    {
        value = default;

        if (
            !TryResolveTableEntry(file, name, out var entry)
            || !entry.BaseRow.TryGetValue(fieldName, out var fieldValue)
            || fieldValue is not bool booleanValue
        )
            return false;

        value = booleanValue;
        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveBoolean</c> operation.
    /// </summary>
    public bool TryResolveBoolean(int globalId, string fieldName, int valueIndex, out bool value)
    {
        value = default;

        if (
            !TryResolveTableEntry(globalId, out var entry)
            || !TryResolveIndexedValue(entry, fieldName, valueIndex, out var fieldValue)
        )
        {
            return false;
        }

        if (fieldValue is null)
            return true;

        if (fieldValue is bool booleanValue)
        {
            value = booleanValue;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(int globalId, string fieldName, int valueIndex, out int value)
    {
        value = default;

        if (
            !TryResolveTableEntry(globalId, out var entry)
            || !TryResolveIndexedValue(entry, fieldName, valueIndex, out var fieldValue)
        )
        {
            return false;
        }

        if (fieldValue is null)
            return true;

        if (fieldValue is int intValue)
        {
            value = intValue;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveValueCount</c> operation.
    /// </summary>
    public bool TryResolveValueCount(int globalId, string fieldName, out int count)
    {
        count = default;

        if (!TryResolveTableEntry(globalId, out var entry))
            return false;

        if (
            !entry.BaseRow.ContainsKey(fieldName)
            && !entry.ContinuationRows.Any(row => row.ContainsKey(fieldName))
        )
        {
            return false;
        }

        count = entry.BaseRow.ContainsKey(fieldName) ? 1 : 0;
        count += entry.ContinuationRows.Count(row => row.ContainsKey(fieldName));
        return true;
    }

    private bool TryResolveTableEntry(
        int globalId,
        [NotNullWhen(true)] out GameDataTableEntry? entry
    )
    {
        if (globalId < GlobalIdTableSize)
        {
            entry = null;
            return false;
        }

        var tableId = globalId / GlobalIdTableSize;
        var rowIndex = globalId % GlobalIdTableSize;

        if (
            !_dataTables.TryGetValue(tableId, out var dataTable)
            || rowIndex >= dataTable.Table.Entries.Count
        )
        {
            entry = null;
            return false;
        }

        entry = dataTable.Table.Entries[rowIndex];
        return true;
    }

    private static bool TryResolveIndexedValue(
        GameDataTableEntry entry,
        string fieldName,
        int valueIndex,
        out object? value
    )
    {
        value = null;

        if (
            valueIndex < 0
            || valueIndex >= entry.Snapshots.Count
            || !entry.Snapshots[0].ContainsKey(fieldName)
        )
        {
            return false;
        }

        var row = valueIndex is 0 ? entry.BaseRow : entry.ContinuationRows[valueIndex - 1];
        row.TryGetValue(fieldName, out value);
        return true;
    }

    private bool TryResolveTableEntry(
        string file,
        string name,
        [NotNullWhen(true)] out GameDataTableEntry? entry
    )
    {
        entry = null;

        if (
            !_resourcesByFile.TryGetValue(file, out var resource)
            || !resource.TryGetTable(out var table)
        )
            return false;

        var matches = table
            .Entries.Where(candidate => candidate.Name.Equals(name, StringComparison.Ordinal))
            .ToArray();

        if (matches.Length > 1)
            throw new InvalidDataException(
                $"Data table {file} contains duplicate entry name {name}."
            );

        if (matches.Length is not 1)
            return false;

        entry = matches[0];
        return true;
    }

    private bool TryResolvePhysicalRows(
        string file,
        [NotNullWhen(true)] out IReadOnlyList<IReadOnlyDictionary<string, object?>>? rows
    )
    {
        rows = null;

        if (
            !_resourcesByFile.TryGetValue(file, out var resource)
            || !resource.TryGetTable(out var table)
        )
            return false;

        rows = table.Entries.SelectMany(static entry => entry.Snapshots).ToArray();
        return true;
    }
}
