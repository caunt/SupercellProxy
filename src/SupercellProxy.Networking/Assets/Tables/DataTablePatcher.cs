using System.Collections.Immutable;
using System.Text;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Defines the Data Table Changes contract.
/// </summary>
public static class DataTablePatcher
{
    /// <summary>
    /// Provides the Apply value or operation.
    /// </summary>
    public static GameAsset[] Apply(IEnumerable<GameAsset> assets, IReadOnlyList<DataTableChange> changes)
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
                resources[change.File] = original with
                {
                    Content = Encoding.UTF8.GetBytes(replacement.Content),
                };
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
        {
            resources[file] = resources[file] with
            {
                Content = Encoding.UTF8.GetBytes(WriteRows(rows)),
            };
        }

        return [.. resources.Values];
    }

    private static void ApplyCell(List<List<string>> rows, DataTableChange change)
    {
        if (rows.Count < 2 || string.IsNullOrEmpty(change.Row) || string.IsNullOrEmpty(change.Column))
            throw new InvalidDataException($"The table cell change {change.File} has {rows.Count} rows; row '{change.Row}', column '{change.Column}'.");

        int column = rows[index: 0].IndexOf(change.Column);

        HashSet<string> names = change
            .Row.Split(separator: ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet(StringComparer.Ordinal);

        List<string>[] matches = [.. rows.Skip(count: 2).Where(row => row.Count > 0 && names.Contains(row[index: 0]))];

        if (column < 0 || matches.Length != names.Count)
            throw new InvalidDataException($"The changed cell {change.File}/{change.Row}/{change.Column} is unavailable.");

        string value = change.NewValue.ToCellText();

        foreach (List<string>? target in matches)
        {
            while (target.Count <= column)
                target.Add(string.Empty);

            target[column] = value;
        }
    }

    private static void ApplyColumns(List<List<string>> rows, string content, string selector)
    {
        List<List<string>> replacement = GameDataTableParser.ParseMutableRows(content);

        if (rows.Count < 2 || replacement.Count != rows.Count)
            throw new InvalidDataException(message: "Replacement table columns have a different row count.");

        foreach (
            string name in selector.Split(separator: ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        )
        {
            int targetColumn = rows[index: 0].IndexOf(name);
            int sourceColumn = replacement[index: 0].IndexOf(name);

            if (targetColumn < 0 || sourceColumn < 0)
                throw new InvalidDataException($"The replacement column {name} is unavailable.");

            for (int index = 1; index < rows.Count; index++)
            {
                List<string> row = rows[index];

                while (row.Count <= targetColumn)
                    row.Add(string.Empty);

                row[targetColumn] =
                    sourceColumn < replacement[index].Count
                        ? replacement[index][sourceColumn]
                        : string.Empty;
            }
        }
    }

    private static string Quote(string value)
    {
        return value.IndexOfAny([',', '"', '\r', '\n']) >= 0
            ? "\"" + value.Replace(oldValue: "\"", newValue: "\"\"", StringComparison.Ordinal) + "\""
            : value;
    }

    private static string WriteRows(List<List<string>> rows)
    {
        return string.Join(separator: '\n', rows.Select(static row => string.Join(separator: ',', row.Select(Quote))));
    }
}
