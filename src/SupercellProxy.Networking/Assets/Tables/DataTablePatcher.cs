using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Applies the CSV edits of live table-change events to copies of the game tables, the way the client applies them while those
/// events run.
/// </summary>
public static class DataTablePatcher
{
    // Farms on different event variants often end up with byte-identical edited files. Handing them one asset per content lets them
    // share its parsed table instead of each farm parsing its own copy.
    private static readonly ConcurrentDictionary<string, WeakReference<GameAsset>> SharedEdits = new(StringComparer.Ordinal);

    /// <summary>
    /// Returns the bundle with every edit applied in order. Edited tables come back as new asset copies; the originals are not changed.
    /// </summary>
    /// <param name="assets">Every file of the bundle.</param>
    /// <param name="changes">Edits to apply; later edits see the result of earlier ones.</param>
    /// <returns>All files of the bundle, edited ones replaced.</returns>
    /// <exception cref="InvalidDataException">
    /// An edit names a missing table, a cell edit has no row or column, or a column replacement is not a table.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="changes"/> or <paramref name="assets"/> is <see langword="null"/>.</exception>
    public static ImmutableArray<GameAsset> Apply(IEnumerable<GameAsset> assets, IReadOnlyList<DataTableChange> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);

        Dictionary<string, GameAsset> resources = assets.ToDictionary(static asset => asset.Fingerprint.File, StringComparer.Ordinal);
        ImmutableDictionary<string, List<List<string>>> editedRows = ImmutableDictionary.Create<string, List<List<string>>>(StringComparer.Ordinal);

        foreach (DataTableChange change in changes)
        {
            if (!resources.TryGetValue(change.File, out GameAsset? original) || !original.IsCsv)
                throw new InvalidDataException($"The changed table {change.File} is unavailable.");

            if (change.NewFullCommaSeparatedValues is { } replacement && string.IsNullOrEmpty(change.Column))
            {
                resources[change.File] = original with { Content = Encoding.UTF8.GetBytes(replacement.Content), };
                editedRows = editedRows.Remove(change.File);

                continue;
            }

            if (!editedRows.TryGetValue(change.File, out List<List<string>>? rows))
            {
                rows = GameDataTableParser.ParseMutableRows(original.AsUnicodeTransformationFormat8);
                editedRows = editedRows.Add(change.File, rows);
            }

            if (change.NewFullCommaSeparatedValues is { } columns)
                ApplyColumns(rows, columns.Content, change.Column ?? string.Empty);
            else
                ApplyCell(rows, change);
        }

        foreach ((string? file, List<List<string>>? rows) in editedRows)
            resources[file] = resources[file] with { Content = Encoding.UTF8.GetBytes(Write(rows)), };

        foreach (string file in changes.Select(static change => change.File).Distinct(StringComparer.Ordinal))
            resources[file] = Share(resources[file]);

        return [.. resources.Values];
    }

    // Native LogicCSVChangeMap (0x100541400): Row and Column each list comma-separated names, a "*" row selects every entry, and a
    // row or column the table lacks is skipped with a warning.
    private static void ApplyCell(List<List<string>> rows, DataTableChange change)
    {
        if (rows.Count < 2 || string.IsNullOrEmpty(change.Row) || string.IsNullOrEmpty(change.Column))
            throw new InvalidDataException($"The table cell change {change.File} has {rows.Count} rows; row '{change.Row}', column '{change.Column}'.");

        HashSet<string> names = Split(change.Row).ToHashSet(StringComparer.Ordinal);
        bool everyEntry = names.Contains(item: "*");

        int[] columns = [.. Split(change.Column).Select(name => rows[index: 0].IndexOf(name))
            .Where(static column => column >= 0)];

        int[] starts =
        [
            .. Enumerable.Range(start: 2, rows.Count - 2)
                .Where(index => rows[index].Count > 0 && rows[index][index: 0] is { Length: > 0 } name && (everyEntry || names.Contains(name))),
        ];

        string value = change.NewValue.ToCellText();

        // An array element lives on the entry's ArrayIndex-th line: the named line, then its unnamed continuation lines; a missing or
        // negative index means the named line. Later entries are edited first, so lines added to an entry that is too short do not move
        // the entries still to edit.
        foreach (int start in starts.Reverse())
        {
            int end = start + 1;

            while (end < rows.Count && rows[end].Count > 0 && string.IsNullOrEmpty(rows[end][index: 0]))
                end++;

            int line = start + Math.Max(change.ArrayIndex ?? 0, val2: 0);

            while (line >= end)
                rows.Insert(end++, item: [string.Empty]);

            List<string> target = rows[line];

            foreach (int column in columns)
            {
                while (target.Count <= column)
                    target.Add(string.Empty);

                target[column] = value;
            }
        }
    }

    // Native LogicCSVChangeMap (0x100541400, column branch at 0x100541894): each named column drops its first n values, n being the
    // shorter of the two columns, and takes the replacement's values in front, so lines past a shorter replacement keep their own values
    // and the tables may differ in line count (2026-10-28 08:00 event). A column either table lacks ("New CSV: … no column found") or
    // whose type differs ("type has changed") is skipped with a warning.
    private static void ApplyColumns(List<List<string>> rows, string content, string selector)
    {
        List<List<string>> replacement = GameDataTableParser.ParseMutableRows(content);

        if (rows.Count < 2 || replacement.Count < 2)
            throw new InvalidDataException($"The replacement columns {selector} have no table: {rows.Count} and {replacement.Count} lines.");

        foreach (string name in Split(selector))
        {
            int targetColumn = rows[index: 0].IndexOf(name);
            int sourceColumn = replacement[index: 0].IndexOf(name);

            if (targetColumn < 0 || sourceColumn < 0 || Cell(rows[index: 1], targetColumn) != Cell(replacement[index: 1], sourceColumn))
                continue;

            for (int index = 2; index < Math.Min(rows.Count, replacement.Count); index++)
            {
                List<string> row = rows[index];

                while (row.Count <= targetColumn)
                    row.Add(string.Empty);

                row[targetColumn] = Cell(replacement[index], sourceColumn);
            }
        }
    }

    private static string Cell(List<string> row, int column)
    {
        return column < row.Count ? row[column] : string.Empty;
    }

    private static string Quote(string value)
    {
        return value.IndexOfAny(anyOf: [',', '"', '\r', '\n']) >= 0
            ? "\"" + value.Replace(oldValue: "\"", newValue: "\"\"", StringComparison.Ordinal) + "\""
            : value;
    }

    private static GameAsset Share(GameAsset edited)
    {
        string key = $"{edited.Fingerprint.File}:{Convert.ToHexString(SHA256.HashData(edited.Content.Span))}";
        WeakReference<GameAsset> added = new(edited);

        // A live copy of the same content is kept and reused; a collected one is replaced by this asset.
        WeakReference<GameAsset> stored = SharedEdits.AddOrUpdate(key, added, (unusedKey, known) => known.TryGetTarget(out _) ? known : added);

        return stored.TryGetTarget(out GameAsset? shared) ? shared : edited;
    }

    private static string[] Split(string names)
    {
        return names.Split(separator: ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    private static string Write(List<List<string>> rows)
    {
        return string.Join(separator: '\n', rows.Select(static row => string.Join(separator: ',', row.Select(Quote))));
    }
}
