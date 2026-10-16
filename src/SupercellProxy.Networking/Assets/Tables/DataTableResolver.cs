using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Looks up rows and cells in the game's CSV tables, either by global id (the number the game uses on the wire and in saves) or by
/// file and entry name. Tables are parsed on first use. Connections on the same asset bundle share one instance through
/// <see cref="Shared"/>.
/// </summary>
/// <remarks>
/// A table entry starts on a line whose first column holds its name; the unnamed lines after it are continuation lines that hold
/// further elements of array columns. Base-row lookups read only the named line, and an empty cell there counts as missing. Indexed
/// lookups read the given line of the entry and treat an empty cell as the type's default.
/// </remarks>
public sealed class DataTableResolver : ICommandDataResolver
{
    /// <summary>
    /// Row numbers available to each table in a global id: a global id is the table id times this value plus the entry's row index,
    /// so 1300002 is the third entry of table 13.
    /// </summary>
    public const int GlobalIdTableSize = 100000;

    private static readonly ConcurrentDictionary<string, DataTableResolver> SharedResolvers = new(StringComparer.Ordinal);
    private readonly Dictionary<int, (GameAsset GameAsset, Lazy<GameDataTable> Table)> _dataTables;
    private readonly Dictionary<string, GameAsset> _resourcesByFile;
    private readonly Dictionary<string, int> _tableIdsByFile;

    /// <summary>
    /// Indexes a downloaded asset bundle by table id and file name; no table is parsed until it is first read.
    /// </summary>
    /// <param name="resources">Every file of the bundle. The tables the game numbers must all be present.</param>
    /// <exception cref="InvalidOperationException">A numbered table's file is missing from the bundle.</exception>
    public DataTableResolver(IEnumerable<GameAsset> resources)
    {
        _resourcesByFile = resources.ToDictionary(static resource => resource.Fingerprint.File, StringComparer.Ordinal);

        Dictionary<int, (GameAsset GameAsset, Lazy<GameDataTable> Table)> resolvedDataTables =
            [];

        foreach ((int tableId, string? file) in DataTableRegistry.Create(resources))
        {
            if (!_resourcesByFile.TryGetValue(file, out GameAsset? resource))
                throw new InvalidOperationException($"GameAsset {file} was not downloaded.");

            resolvedDataTables.Add(tableId, (resource, new Lazy<GameDataTable>(() => ParseTable(resource))));
        }

        _dataTables = resolvedDataTables;
        HighestTableId = resolvedDataTables.Keys.Max();
        _tableIdsByFile = resolvedDataTables.ToDictionary(static entry => entry.Value.GameAsset.Fingerprint.File, static entry => entry.Key, StringComparer.Ordinal);
    }

    /// <summary>
    /// Largest table number this bundle defines; global ids above this table's range cannot resolve.
    /// </summary>
    [JsonPropertyName("HighestTableId")]
    public int HighestTableId { get; }

    /// <summary>Returns one resolver per asset fingerprint, so connections on the same assets share parsed tables.</summary>
    public static DataTableResolver Shared(GameAssetFingerprint? fingerprint, GameAsset[] resources)
    {
        ArgumentNullException.ThrowIfNull(resources);

        return fingerprint is null ? new DataTableResolver(resources) : SharedResolvers.GetOrAdd(fingerprint.Sha, static (sha, assets) => new DataTableResolver(assets), resources);
    }

    /// <summary>
    /// Reads any file of the bundle as UTF-8 text, including files that are not tables, such as the Valley map JSON.
    /// </summary>
    /// <param name="file">Path inside the bundle, as listed in <see cref="GameAssetFiles"/>.</param>
    /// <exception cref="InvalidDataException">The bundle has no such file.</exception>
    public string ReadAssetText(string file)
    {
        return _resourcesByFile.TryGetValue(file, out GameAsset? asset)
            ? asset.AsUnicodeTransformationFormat8
            : throw new InvalidDataException($"The asset {file} is unavailable.");
    }

    /// <summary>
    /// Lists every entry of a numbered table, in file order, so the array index equals the entry's row index.
    /// </summary>
    /// <param name="file">Path of a table the game gives a table id.</param>
    /// <exception cref="InvalidDataException">The file is not a numbered table, or one of its entries has no name.</exception>
    public DataTableReference[] ResolveAll(string file)
    {
        if (!TryGetTableId(file, out int tableId) || !TryGetTableEntryCount(file, out int count))
            throw new InvalidDataException($"The data table {file} is unavailable.");

        DataTableReference[] entries = new DataTableReference[count];

        for (int index = 0; index < count; index++)
        {
            if (!TryResolve((tableId * GlobalIdTableSize) + index, out DataTableReference? entry))
                throw new InvalidDataException(message: "The data table entry is unavailable.");

            entries[index] = entry;
        }

        return entries;
    }

    /// <summary>
    /// Lists every entry of the table with the given native table id, in file order.
    /// </summary>
    /// <param name="tableId">Table number as used in global ids, such as 13 for animals.</param>
    /// <exception cref="InvalidDataException">This bundle has no table with that number.</exception>
    public DataTableReference[] ResolveAll(int tableId)
    {
        return !_dataTables.TryGetValue(tableId, out (GameAsset GameAsset, Lazy<GameDataTable> Table) table)
            ? throw new InvalidDataException(message: "The requested native data table is unavailable.")
            : ResolveAll(table.GameAsset.Fingerprint.File);
    }

    /// <summary>
    /// Counts the entries (named lines) of a numbered table; continuation lines are not counted.
    /// </summary>
    /// <param name="file">Path of a table the game gives a table id.</param>
    /// <param name="count">Number of entries, or 0 when the file is not a numbered table.</param>
    public bool TryGetTableEntryCount(string file, out int count)
    {
        count = default;

        if (!_tableIdsByFile.TryGetValue(file, out int tableId))
            return false;

        if (ResolveTable(tableId) is not { } table)
            return false;

        count = table.Entries.Count;

        return true;
    }

    /// <summary>
    /// Finds the number the game gives a table file, which is the leading part of its entries' global ids.
    /// </summary>
    /// <param name="file">Path of the table inside the bundle.</param>
    /// <param name="tableId">The table's number, or 0 when the file is not a numbered table.</param>
    public bool TryGetTableId(string file, out int tableId)
    {
        return _tableIdsByFile.TryGetValue(file, out tableId);
    }

    /// <summary>
    /// Turns a global id received from the server or read from a save into the table entry it points at.
    /// </summary>
    /// <param name="globalId">Table id times <see cref="GlobalIdTableSize"/> plus the entry's row index.</param>
    /// <param name="reference">The entry's table, row and name; null when the id is out of range or points at an unnamed row.</param>
    public bool TryResolve(int globalId, [NotNullWhen(true)] out DataTableReference? reference)
    {
        if (globalId < GlobalIdTableSize)
        {
            reference = null;

            return false;
        }

        int tableId = globalId / GlobalIdTableSize;
        int rowIndex = globalId % GlobalIdTableSize;

        if (ResolveTable(tableId) is not { } table || rowIndex >= table.Entries.Count)
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

        GameAssetFingerprintEntry fingerprint = _dataTables[tableId].GameAsset.Fingerprint;
        reference = new DataTableReference(globalId, tableId, rowIndex, name, fingerprint.File, fingerprint.Sha);

        return true;
    }

    /// <summary>
    /// Finds a numbered table's entry by its name, which gives its global id for use in commands.
    /// </summary>
    /// <param name="file">Path of a table the game gives a table id.</param>
    /// <param name="name">Entry name exactly as written in the table's first column, such as <c>Chicken</c>.</param>
    /// <param name="reference">The entry with its global id; null when the table or the name is unknown.</param>
    /// <exception cref="InvalidDataException">Two entries in the table share the name.</exception>
    public bool TryResolve(string file, string name, [NotNullWhen(true)] out DataTableReference? reference)
    {
        reference = null;

        if (!_tableIdsByFile.TryGetValue(file, out int tableId))
            return false;

        if (ResolveTable(tableId) is not { } table)
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
            && TryResolve((tableId * GlobalIdTableSize) + rowIndex, out reference);
    }

    /// <summary>
    /// Reads a Boolean column from the named line of an entry.
    /// </summary>
    /// <param name="globalId">Entry to read, as a global id.</param>
    /// <param name="fieldName">Column header, such as <c>Disabled</c>.</param>
    /// <param name="value">The cell's value; false when it could not be read.</param>
    /// <returns>False when the entry is unknown or the cell is empty or not a Boolean.</returns>
    public bool TryResolveBoolean(int globalId, string fieldName, out bool value)
    {
        value = default;

        if (ResolveEntry(globalId) is not { } entry || !entry.BaseRow.TryGetValue(fieldName, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null || !cell.TryGetBoolean(out bool booleanValue))
            return false;

        value = booleanValue;

        return true;
    }

    /// <summary>
    /// Reads a Boolean column from the named line of an entry found by name, in any CSV table of the bundle, including tables the
    /// game gives no table id.
    /// </summary>
    /// <param name="file">Path of the table inside the bundle.</param>
    /// <param name="name">Entry name as written in the table's first column.</param>
    /// <param name="fieldName">Column header to read.</param>
    /// <param name="value">The cell's value; false when it could not be read.</param>
    /// <returns>False when the table or entry is unknown or the cell is empty or not a Boolean.</returns>
    /// <exception cref="InvalidDataException">Two entries in the table share the name.</exception>
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
    /// Reads one element of a Boolean array column: the value on the entry's line at <paramref name="valueIndex"/>, where 0 is the
    /// named line and higher indexes are its continuation lines.
    /// </summary>
    /// <param name="globalId">Entry to read, as a global id.</param>
    /// <param name="fieldName">Column header to read.</param>
    /// <param name="valueIndex">Line within the entry, starting at 0 for the named line.</param>
    /// <param name="value">The element; false when that line leaves the cell empty.</param>
    /// <returns>True for an empty cell too; false when the entry, column or line does not exist or the cell is not a Boolean.</returns>
    public bool TryResolveBoolean(int globalId, string fieldName, int valueIndex, out bool value)
    {
        value = default;

        if (ResolveEntry(globalId) is not { } entry)
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

    /// <summary>Gets the number of rows belonging to one data-table entry, including continuation rows.</summary>
    /// <param name="globalId">Entry to measure, as a global id.</param>
    /// <param name="count">The named line plus its continuation lines; 0 when the entry is unknown.</param>
    public bool TryResolveEntryRowCount(int globalId, out int count)
    {
        GameDataTableEntry? entry = ResolveEntry(globalId);
        count = entry?.Snapshots.Count ?? 0;

        return entry is not null;
    }

    /// <summary>
    /// Reads an integer column from the named line of an entry.
    /// </summary>
    /// <param name="globalId">Entry to read, as a global id.</param>
    /// <param name="fieldName">Column header, such as <c>UnlockLevel</c>.</param>
    /// <param name="value">The cell's value; 0 when it could not be read.</param>
    /// <returns>False when the entry is unknown or the cell is empty or not an integer.</returns>
    public bool TryResolveInt(int globalId, string fieldName, out int value)
    {
        value = default;

        if (ResolveEntry(globalId) is not { } entry || !entry.BaseRow.TryGetValue(fieldName, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null || !cell.TryGetInt32(out int intValue))
            return false;

        value = intValue;

        return true;
    }

    /// <summary>
    /// Reads an integer column from a CSV line counted across the whole table rather than within one entry. Values left empty on a
    /// continuation line carry over from earlier lines of the same entry. Suits tables whose lines are a plain list, such as
    /// per-level tables.
    /// </summary>
    /// <param name="file">Path of any CSV table in the bundle.</param>
    /// <param name="physicalRowIndex">Line number after the two header rows, starting at 0, skipping blank lines.</param>
    /// <param name="fieldName">Column header to read.</param>
    /// <param name="value">The cell's value; 0 when it could not be read.</param>
    /// <returns>False when the table or line does not exist or the value is empty or not an integer.</returns>
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
    /// Reads an integer column from the named line of an entry found by name, in any CSV table of the bundle.
    /// </summary>
    /// <param name="file">Path of the table inside the bundle.</param>
    /// <param name="name">Entry name as written in the table's first column.</param>
    /// <param name="fieldName">Column header to read.</param>
    /// <param name="value">The cell's value; 0 when it could not be read.</param>
    /// <returns>False when the table or entry is unknown or the cell is empty or not an integer.</returns>
    /// <exception cref="InvalidDataException">Two entries in the table share the name.</exception>
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
    /// Reads one element of an integer array column from an entry found by name, in any CSV table of the bundle.
    /// </summary>
    /// <param name="file">Path of the table inside the bundle.</param>
    /// <param name="name">Entry name as written in the table's first column.</param>
    /// <param name="fieldName">Column header to read.</param>
    /// <param name="valueIndex">Line within the entry, starting at 0 for the named line.</param>
    /// <param name="value">The element; 0 when that line leaves the cell empty.</param>
    /// <returns>True for an empty cell too; false when the entry, column or line does not exist or the cell is not an integer.</returns>
    /// <exception cref="InvalidDataException">Two entries in the table share the name.</exception>
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
    /// Reads one element of an integer array column, such as one level's value in a per-level column.
    /// </summary>
    /// <param name="globalId">Entry to read, as a global id.</param>
    /// <param name="fieldName">Column header to read.</param>
    /// <param name="valueIndex">Line within the entry, starting at 0 for the named line.</param>
    /// <param name="value">The element; 0 when that line leaves the cell empty.</param>
    /// <returns>True for an empty cell too; false when the entry, column or line does not exist or the cell is not an integer.</returns>
    public bool TryResolveInt(int globalId, string fieldName, int valueIndex, out int value)
    {
        value = default;

        if (ResolveEntry(globalId) is not { } entry)
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
    /// Counts the non-blank CSV lines of a table below its two header rows, named and continuation lines alike; this is the range of
    /// line numbers the line-based integer lookup accepts.
    /// </summary>
    /// <param name="file">Path of any CSV table in the bundle.</param>
    /// <param name="count">Number of lines; 0 when the file is missing or not a table.</param>
    public bool TryResolvePhysicalRowCount(string file, out int count)
    {
        count = default;

        if (!TryResolvePhysicalRows(file, out IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>>? rows))
            return false;

        count = rows.Count;

        return true;
    }

    /// <summary>
    /// Reads a text column from the named line of an entry. Commands use it to pick a payload layout from a data entry's type column.
    /// </summary>
    /// <param name="globalId">Entry to read, as a global id.</param>
    /// <param name="fieldName">Column header, such as <c>Tags</c>.</param>
    /// <param name="value">The cell's text; null when it could not be read.</param>
    /// <returns>False when the entry is unknown or the cell is empty.</returns>
    public bool TryResolveString(int globalId, string fieldName, [NotNullWhen(true)] out string? value)
    {
        value = null;

        if (ResolveEntry(globalId) is not { } entry || !entry.BaseRow.TryGetValue(fieldName, out LiteralValue cell))
            return false;

        if (cell.Kind == LiteralKind.Null || !cell.TryGetString(out string? stringValue))
            return false;

        value = stringValue;

        return true;
    }

    /// <summary>
    /// Reads one element of a text array column of an entry found by global id.
    /// </summary>
    /// <param name="globalId">Entry to read, as a global id.</param>
    /// <param name="fieldName">Column header to read.</param>
    /// <param name="valueIndex">Line within the entry, starting at 0 for the named line.</param>
    /// <param name="value">The element; an empty string when that line leaves the cell empty.</param>
    /// <returns>True for an empty cell too; false when the entry, column or line does not exist.</returns>
    public bool TryResolveString(int globalId, string fieldName, int valueIndex, [NotNullWhen(true)] out string? value)
    {
        value = null;

        if (ResolveEntry(globalId) is not { } entry)
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
    /// Reads one element of a text array column of an entry found by name, in any CSV table of the bundle; with index 0 this reads
    /// the named line, which is how single-value settings tables are read.
    /// </summary>
    /// <param name="file">Path of the table inside the bundle.</param>
    /// <param name="name">Entry name as written in the table's first column.</param>
    /// <param name="fieldName">Column header to read.</param>
    /// <param name="valueIndex">Line within the entry, starting at 0 for the named line.</param>
    /// <param name="value">The element; an empty string when that line leaves the cell empty.</param>
    /// <returns>True for an empty cell too; false when the table, entry, column or line does not exist.</returns>
    /// <exception cref="InvalidDataException">Two entries in the table share the name.</exception>
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
    /// Counts the lines of an entry found by name that can hold an element of the column, which is the length of the array the
    /// column stores for that entry.
    /// </summary>
    /// <param name="file">Path of the table inside the bundle.</param>
    /// <param name="name">Entry name as written in the table's first column.</param>
    /// <param name="fieldName">Column header to measure.</param>
    /// <param name="count">The named line plus its continuation lines when the column exists; otherwise 0.</param>
    /// <exception cref="InvalidDataException">Two entries in the table share the name.</exception>
    public bool TryResolveValueCount(string file, string name, string fieldName, out int count)
    {
        count = default;

        if (ResolveEntry(file, name) is not { } entry || !entry.Snapshots.Any(row => row.ContainsKey(fieldName)))
            return false;

        count = entry.Snapshots.Count(row => row.ContainsKey(fieldName));

        return true;
    }

    /// <summary>
    /// Counts how many lines of an entry actually fill the column, which is how many elements an array column such as a list of
    /// export variants holds. Lines that leave the cell empty are not counted.
    /// </summary>
    /// <param name="globalId">Entry to measure, as a global id.</param>
    /// <param name="fieldName">Column header to measure.</param>
    /// <param name="count">Number of filled lines; 0 when no line fills the column.</param>
    /// <returns>False when the entry is unknown or no line fills the column.</returns>
    public bool TryResolveValueCount(int globalId, string fieldName, out int count)
    {
        count = default;

        if (ResolveEntry(globalId) is not { } entry)
            return false;

        if (!entry.BaseRow.ContainsKey(fieldName) && !entry.ContinuationRows.Any(row => row.ContainsKey(fieldName)))
            return false;

        count = entry.BaseRow.ContainsKey(fieldName) ? 1 : 0;
        count += entry.ContinuationRows.Count(row => row.ContainsKey(fieldName));

        return true;
    }

    /// <summary>
    /// Builds a separate resolver over a copy of the bundle with live table-change events applied; this resolver is left untouched.
    /// </summary>
    /// <param name="changes">Edits from the table-change events that are running, applied in order.</param>
    /// <exception cref="InvalidDataException">An edit names a table, entry or column the bundle does not have.</exception>
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

    private GameDataTableEntry? ResolveEntry(int id)
    {
        return TryResolveTableEntry(id, out GameDataTableEntry? entry) ? entry : null;
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

    private GameDataTable? ResolveTable(int id)
    {
        return _dataTables.TryGetValue(id, out (GameAsset GameAsset, Lazy<GameDataTable> Table) table) ? table.Table.Value : null;
    }

    private bool TryResolvePhysicalRows(string file, [NotNullWhen(true)] out IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>>? rows)
    {
        rows = null;

        if (!_resourcesByFile.TryGetValue(file, out GameAsset? resource) || !resource.TryGetTable(out GameDataTable? table))
            return false;

        rows = [.. table.Entries.SelectMany(static entry => entry.Snapshots)];

        return true;
    }

    private bool TryResolveTableEntry(int globalId, [NotNullWhen(true)] out GameDataTableEntry? entry)
    {
        if (globalId < GlobalIdTableSize)
        {
            entry = null;

            return false;
        }

        int tableId = globalId / GlobalIdTableSize;
        int rowIndex = globalId % GlobalIdTableSize;

        if (ResolveTable(tableId) is not { } table || rowIndex >= table.Entries.Count)
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
