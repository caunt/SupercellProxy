using System.Diagnostics.CodeAnalysis;

using SupercellProxy.Networking.Protocol.CommandEncoding;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Represents <c language="csharp">DataTableResolver</c>.
/// </summary>
public sealed class DataTableResolver : ICommandDataResolver
{
    /// <summary>
    /// Defines the <c language="csharp">GlobalIdTableSize</c> value.
    /// </summary>
    public const int GlobalIdentifierTableSize = 100000;

    private readonly Dictionary<int, (GameAsset GameAsset, Lazy<GameDataTable> Table)> _dataTables;
    private readonly Dictionary<string, GameAsset> _resourcesByFile;
    private readonly Dictionary<string, int> _tableIdentifiersByFile;

    /// <summary>
    /// Initializes a new <see cref="DataTableResolver"/> instance.
    /// </summary>
    public DataTableResolver(IEnumerable<GameAsset> resources)
    {
        _resourcesByFile = resources.ToDictionary(static resource => resource.Fingerprint.File, StringComparer.Ordinal);

        Dictionary<int, (GameAsset GameAsset, Lazy<GameDataTable> Table)> resolvedDataTables =
            [];

        foreach ((int tableIdentifier, string? file) in DataTableRegistry.Create(resources))
        {
            if (!_resourcesByFile.TryGetValue(file, out GameAsset? resource))
                throw new InvalidOperationException($"GameAsset {file} was not downloaded.");

            resolvedDataTables.Add(tableIdentifier, (resource, new Lazy<GameDataTable>(() => ParseTable(resource))));
        }

        _dataTables = resolvedDataTables;
        HighestTableIdentifier = resolvedDataTables.Keys.Max();
        _tableIdentifiersByFile = resolvedDataTables.ToDictionary(static entry => entry.Value.GameAsset.Fingerprint.File, static entry => entry.Key, StringComparer.Ordinal);
    }

    /// <summary>
    /// Gets the <c language="csharp">HighestTableId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HighestTableId")]
    public int HighestTableIdentifier { get; }

    /// <summary>
    /// Provides the Read Asset Text value or operation.
    /// </summary>
    public string ReadAssetText(string file)
    {
        return _resourcesByFile.TryGetValue(file, out GameAsset? asset)
            ? asset.AsUnicodeTransformationFormat8
            : throw new InvalidDataException($"The asset {file} is unavailable.");
    }

    /// <summary>
    /// Provides the Resolve All value or operation.
    /// </summary>
    public DataTableReference[] ResolveAll(string file)
    {
        if (!TryGetTableIdentifier(file, out int tableIdentifier) || !TryGetTableEntryCount(file, out int count))
            throw new InvalidDataException($"The data table {file} is unavailable.");

        DataTableReference[] entries = new DataTableReference[count];

        for (int index = 0; index < count; index++)
        {
            if (!TryResolve((tableIdentifier * GlobalIdentifierTableSize) + index, out DataTableReference? entry))
                throw new InvalidDataException(message: "The data table entry is unavailable.");

            entries[index] = entry;
        }

        return entries;
    }

    /// <summary>
    /// Provides the Resolve All value or operation.
    /// </summary>
    public DataTableReference[] ResolveAll(int tableIdentifier)
    {
        return !_dataTables.TryGetValue(tableIdentifier, out (GameAsset GameAsset, Lazy<GameDataTable> Table) table)
            ? throw new InvalidDataException(message: "The requested native data table is unavailable.")
            : ResolveAll(table.GameAsset.Fingerprint.File);
    }

    /// <summary>
    /// Attempts the <c language="csharp">GetTableEntryCount</c> operation.
    /// </summary>
    public bool TryGetTableEntryCount(string file, out int count)
    {
        count = default;

        if (!_tableIdentifiersByFile.TryGetValue(file, out int tableIdentifier))
            return false;

        if (ResolveTable(tableIdentifier) is not { } table)
            return false;

        count = table.Entries.Count;

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">GetTableId</c> operation.
    /// </summary>
    public bool TryGetTableIdentifier(string file, out int tableIdentifier)
    {
        return _tableIdentifiersByFile.TryGetValue(file, out tableIdentifier);
    }

    /// <summary>
    /// Attempts the <c language="csharp">Resolve</c> operation.
    /// </summary>
    public bool TryResolve(int globalIdentifier, [NotNullWhen(true)] out DataTableReference? reference)
    {
        if (globalIdentifier < GlobalIdentifierTableSize)
        {
            reference = null;

            return false;
        }

        int tableIdentifier = globalIdentifier / GlobalIdentifierTableSize;
        int rowIndex = globalIdentifier % GlobalIdentifierTableSize;

        if (ResolveTable(tableIdentifier) is not { } table || rowIndex >= table.Entries.Count)
        {
            reference = null;

            return false;
        }

        string name = table.Entries[rowIndex].Name;

        if (string.IsNullOrWhiteSpace(name))
        {
            reference = null;

            return false;
        }

        GameAssetFingerprintEntry fingerprint = _dataTables[tableIdentifier].GameAsset.Fingerprint;
        reference = new DataTableReference(globalIdentifier, tableIdentifier, rowIndex, name, fingerprint.File, fingerprint.Sha);

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">Resolve</c> operation.
    /// </summary>
    public bool TryResolve(string file, string name, [NotNullWhen(true)] out DataTableReference? reference)
    {
        reference = null;

        if (!_tableIdentifiersByFile.TryGetValue(file, out int tableIdentifier))
            return false;

        if (ResolveTable(tableIdentifier) is not { } table)
            return false;

        int rowIndex = -1;

        for (int index = 0; index < table.Entries.Count; index++)
        {
            if (!table.Entries[index].Name.Equals(name, StringComparison.Ordinal))
                continue;

            if (rowIndex is not -1)
                throw new InvalidDataException($"Data table {file} contains duplicate entry name {name}.");

            rowIndex = index;
        }

        return rowIndex is not -1
            && TryResolve((tableIdentifier * GlobalIdentifierTableSize) + rowIndex, out reference);
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveBoolean</c> operation.
    /// </summary>
    public bool TryResolveBoolean(int globalIdentifier, string fieldName, out bool value)
    {
        value = default;

        if (ResolveEntry(globalIdentifier) is not { } entry || !entry.BaseRow.TryGetValue(fieldName, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null || !cell.TryGetBoolean(out bool booleanValue))
            return false;

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

        if (ResolveEntry(file, name) is not { } entry || !entry.BaseRow.TryGetValue(fieldName, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null || !cell.TryGetBoolean(out bool booleanValue))
            return false;

        value = booleanValue;

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveBoolean</c> operation.
    /// </summary>
    public bool TryResolveBoolean(int globalIdentifier, string fieldName, int valueIndex, out bool value)
    {
        value = default;

        if (ResolveEntry(globalIdentifier) is not { } entry)
            return false;

        if (!TryResolveIndexedValue(entry, fieldName, valueIndex, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null)
            return true;

        if (cell.TryGetBoolean(out bool booleanValue))
        {
            value = booleanValue;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(int globalIdentifier, string fieldName, out int value)
    {
        value = default;

        if (ResolveEntry(globalIdentifier) is not { } entry || !entry.BaseRow.TryGetValue(fieldName, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null || !cell.TryGetInt32(out int intValue))
            return false;

        value = intValue;

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(string file, int physicalRowIndex, string fieldName, out int value)
    {
        value = default;

        if (ResolvePhysicalRow(file, physicalRowIndex) is not { } row)
            return false;

        if (!row.TryGetValue(fieldName, out LiteralValue cell) || cell.Kind == LiteralKind.Null)
            return false;

        if (!cell.TryGetInt32(out int intValue))
            return false;

        value = intValue;

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(string file, string name, string fieldName, out int value)
    {
        value = default;

        if (!_resourcesByFile.TryGetValue(file, out GameAsset? resource) || !resource.TryGetTable(out GameDataTable? table))
            return false;

        GameDataTableEntry[] entries = [.. table
            .Entries.Where(entry => entry.Name.Equals(name, StringComparison.Ordinal))];

        if (entries.Length > 1)
            throw new InvalidDataException($"Data table {file} contains duplicate entry name {name}.");

        if (entries.Length is not 1 || !entries[0].BaseRow.TryGetValue(fieldName, out LiteralValue cell) || cell.Kind == LiteralKind.Null)
            return false;

        if (!cell.TryGetInt32(out int intValue))
            return false;

        value = intValue;

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(string file, string name, string fieldName, int valueIndex, out int value)
    {
        value = default;

        if (ResolveEntry(file, name) is not { } entry || !TryResolveIndexedValue(entry, fieldName, valueIndex, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null)
            return true;

        if (cell.TryGetInt32(out int intValue))
        {
            value = intValue;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveInt</c> operation.
    /// </summary>
    public bool TryResolveInt(int globalIdentifier, string fieldName, int valueIndex, out int value)
    {
        value = default;

        if (ResolveEntry(globalIdentifier) is not { } entry)
            return false;

        if (!TryResolveIndexedValue(entry, fieldName, valueIndex, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null)
            return true;

        if (cell.TryGetInt32(out int intValue))
        {
            value = intValue;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolvePhysicalRowCount</c> operation.
    /// </summary>
    public bool TryResolvePhysicalRowCount(string file, out int count)
    {
        count = default;

        if (!TryResolvePhysicalRows(file, out IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>>? rows))
            return false;

        count = rows.Count;

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    public bool TryResolveString(int globalIdentifier, string fieldName, [NotNullWhen(true)] out string? value)
    {
        value = null;

        if (ResolveEntry(globalIdentifier) is not { } entry || !entry.BaseRow.TryGetValue(fieldName, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null || !cell.TryGetString(out string? stringValue))
            return false;

        value = stringValue;

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    public bool TryResolveString(int globalIdentifier, string fieldName, int valueIndex, [NotNullWhen(true)] out string? value)
    {
        value = null;

        if (ResolveEntry(globalIdentifier) is not { } entry)
            return false;

        if (!TryResolveIndexedValue(entry, fieldName, valueIndex, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null)
        {
            value = string.Empty;

            return true;
        }

        if (cell.TryGetString(out string? stringValue))
        {
            value = stringValue;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    public bool TryResolveString(string file, string name, string fieldName, int valueIndex, [NotNullWhen(true)] out string? value)
    {
        value = null;

        if (ResolveEntry(file, name) is not { } entry || !TryResolveIndexedValue(entry, fieldName, valueIndex, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null)
        {
            value = string.Empty;

            return true;
        }

        if (cell.TryGetString(out string? stringValue))
        {
            value = stringValue;

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

        if (ResolveEntry(file, name) is not { } entry || !entry.Snapshots.Any(row => row.ContainsKey(fieldName)))
            return false;

        count = entry.Snapshots.Count(row => row.ContainsKey(fieldName));

        return true;
    }

    /// <summary>
    /// Attempts the <c language="csharp">ResolveValueCount</c> operation.
    /// </summary>
    public bool TryResolveValueCount(int globalIdentifier, string fieldName, out int count)
    {
        count = default;

        if (ResolveEntry(globalIdentifier) is not { } entry)
            return false;

        if (!entry.BaseRow.ContainsKey(fieldName) && !entry.ContinuationRows.Any(row => row.ContainsKey(fieldName)))
            return false;

        count = entry.BaseRow.ContainsKey(fieldName) ? 1 : 0;
        count += entry.ContinuationRows.Count(row => row.ContainsKey(fieldName));

        return true;
    }

    /// <summary>
    /// Provides the With Changes value or operation.
    /// </summary>
    public DataTableResolver WithChanges(IReadOnlyList<DataTableChange> changes)
    {
        return new(DataTablePatcher.Apply(_resourcesByFile.Values, changes));
    }

    private static GameDataTable ParseTable(GameAsset resource)
    {
        try
        {
            if (resource.TryGetTable(out GameDataTable? table))
                return table;
        }
        catch (FormatException exception)
        {
            throw new InvalidDataException($"Failed to parse {resource.Fingerprint.File}.", exception);
        }

        throw new InvalidOperationException($"Failed to parse {resource.Fingerprint.File}.");
    }

    private static bool TryResolveIndexedValue(GameDataTableEntry entry, string fieldName, int valueIndex, out LiteralValue value)
    {
        value = default;

        if (valueIndex < 0 || valueIndex >= entry.Snapshots.Count || !entry.Snapshots[index: 0].ContainsKey(fieldName))
            return false;

        IReadOnlyDictionary<string, LiteralValue> row = valueIndex is 0 ? entry.BaseRow : entry.ContinuationRows[valueIndex - 1];
        value = row.GetValueOrDefault(fieldName);

        return true;
    }

    private GameDataTableEntry? ResolveEntry(int identifier)
    {
        return TryResolveTableEntry(identifier, out GameDataTableEntry? entry) ? entry : null;
    }

    private GameDataTableEntry? ResolveEntry(string file, string name)
    {
        return TryResolveTableEntry(file, name, out GameDataTableEntry? entry) ? entry : null;
    }

    private IReadOnlyDictionary<string, LiteralValue>? ResolvePhysicalRow(string file, int index)
    {
        return index < 0
            ? null
            : !TryResolvePhysicalRows(file, out IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>>? rows)
            ? null
            : index < rows.Count ? rows[index] : null;
    }

    private GameDataTable? ResolveTable(int identifier)
    {
        return _dataTables.TryGetValue(identifier, out (GameAsset GameAsset, Lazy<GameDataTable> Table) table) ? table.Table.Value : null;
    }

    private bool TryResolvePhysicalRows(string file, [NotNullWhen(true)] out IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>>? rows)
    {
        rows = null;

        if (!_resourcesByFile.TryGetValue(file, out GameAsset? resource) || !resource.TryGetTable(out GameDataTable? table))
            return false;

        rows = [.. table.Entries.SelectMany(static entry => entry.Snapshots)];

        return true;
    }

    private bool TryResolveTableEntry(int globalIdentifier, [NotNullWhen(true)] out GameDataTableEntry? entry)
    {
        if (globalIdentifier < GlobalIdentifierTableSize)
        {
            entry = null;

            return false;
        }

        int tableIdentifier = globalIdentifier / GlobalIdentifierTableSize;
        int rowIndex = globalIdentifier % GlobalIdentifierTableSize;

        if (ResolveTable(tableIdentifier) is not { } table || rowIndex >= table.Entries.Count)
        {
            entry = null;

            return false;
        }

        entry = table.Entries[rowIndex];

        return true;
    }

    private bool TryResolveTableEntry(string file, string name, [NotNullWhen(true)] out GameDataTableEntry? entry)
    {
        entry = null;

        if (!_resourcesByFile.TryGetValue(file, out GameAsset? resource) || !resource.TryGetTable(out GameDataTable? table))
            return false;

        GameDataTableEntry[] matches = [.. table
            .Entries.Where(candidate => candidate.Name.Equals(name, StringComparison.Ordinal))];

        if (matches.Length > 1)
            throw new InvalidDataException($"Data table {file} contains duplicate entry name {name}.");

        if (matches.Length is not 1)
            return false;

        entry = matches[0];

        return true;
    }
}
