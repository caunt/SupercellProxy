using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace SupercellProxy.Playground.Resources.Csv;

public static class SupercellCsvParser
{
    public static SupercellCsvTable Parse(string csvText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(csvText);

        var rows = ParsePhysicalRows(csvText);

        if (rows.Count < 2)
            throw new FormatException("Expected at least a header row and a type row.");

        var headers = rows[0];
        var types = NormalizeDataRow(rows[1], headers.Count, rowIndex: 1);
        var entryBuilders = new List<SupercellCsvEntryBuilder>();

        SupercellCsvEntryBuilder? currentEntryBuilder = null;

        for (var rowIndex = 2; rowIndex < rows.Count; rowIndex++)
        {
            var row = NormalizeDataRow(rows[rowIndex], headers.Count, rowIndex);

            if (IsEmptyRow(row))
                continue;

            var parsedValues = ParseSparseValues(headers, types, row);

            var startsNewEntry = !string.IsNullOrWhiteSpace(row[0]);

            if (startsNewEntry)
            {
                currentEntryBuilder = new SupercellCsvEntryBuilder(headers);
                currentEntryBuilder.ApplyBaseRow(parsedValues);
                entryBuilders.Add(currentEntryBuilder);
                continue;
            }

            if (currentEntryBuilder is null)
                throw new FormatException($"Continuation row at index {rowIndex} appeared before any entry start row.");

            currentEntryBuilder.ApplyContinuationRow(parsedValues);
        }

        var entries = new List<SupercellCsvEntry>(entryBuilders.Count);

        foreach (var entryBuilder in entryBuilders)
            entries.Add(entryBuilder.Build());

        return new SupercellCsvTable(headers.AsReadOnly(), types.AsReadOnly(), entries.AsReadOnly());
    }

    private static List<List<string>> ParsePhysicalRows(string csvText)
    {
        var rows = new List<List<string>>();
        var currentRow = new List<string>();
        var currentCell = new StringBuilder();
        var insideQuotes = false;

        for (var index = 0; index < csvText.Length; index++)
        {
            var currentCharacter = csvText[index];

            if (insideQuotes)
            {
                if (currentCharacter == '"')
                {
                    var nextIndex = index + 1;

                    if (nextIndex < csvText.Length && csvText[nextIndex] == '"')
                    {
                        currentCell.Append('"');
                        index++;
                        continue;
                    }

                    insideQuotes = false;
                    continue;
                }

                currentCell.Append(currentCharacter);
                continue;
            }

            if (currentCharacter == '"')
            {
                insideQuotes = true;
                continue;
            }

            if (currentCharacter == ',')
            {
                currentRow.Add(currentCell.ToString());
                currentCell.Clear();
                continue;
            }

            if (currentCharacter == '\r')
            {
                continue;
            }

            if (currentCharacter == '\n')
            {
                currentRow.Add(currentCell.ToString());
                currentCell.Clear();

                rows.Add(currentRow);
                currentRow = [];
                continue;
            }

            currentCell.Append(currentCharacter);
        }

        if (insideQuotes)
            throw new FormatException("CSV ended while still inside a quoted field.");

        currentRow.Add(currentCell.ToString());

        if (currentRow.Count > 1 || currentRow[0].Length > 0)
            rows.Add(currentRow);

        return rows;
    }

    private static List<string> NormalizeDataRow(List<string> row, int expectedColumnCount, int rowIndex)
    {
        if (row.Count > expectedColumnCount)
            throw new FormatException($"Row {rowIndex} has {row.Count} columns but the header declares {expectedColumnCount} columns.");

        if (row.Count == expectedColumnCount)
            return row;

        var normalizedRow = new List<string>(expectedColumnCount);
        normalizedRow.AddRange(row);

        while (normalizedRow.Count < expectedColumnCount)
            normalizedRow.Add(string.Empty);

        return normalizedRow;
    }

    private static bool IsEmptyRow(List<string> row)
    {
        foreach (var value in row)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return false;
        }

        return true;
    }

    private static Dictionary<string, object?> ParseSparseValues(IReadOnlyList<string> headers, IReadOnlyList<string> types, IReadOnlyList<string> row)
    {
        var values = new Dictionary<string, object?>(headers.Count, StringComparer.Ordinal);

        for (var index = 0; index < headers.Count; index++)
        {
            var cellText = row[index];

            if (string.IsNullOrWhiteSpace(cellText))
                continue;

            values[headers[index]] = ParseValue(cellText, types[index]);
        }

        return values;
    }

    private static object ParseValue(string cellText, string declaredType)
    {
        var normalizedType = declaredType.Trim().ToLowerInvariant();

        return normalizedType switch
        {
            "int" => int.Parse(cellText.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture),
            "long" => long.Parse(cellText.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture),
            "float" => float.Parse(cellText.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
            "double" => double.Parse(cellText.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
            "boolean" or "bool" => ParseBoolean(cellText),
            "string" => cellText,
            _ => cellText
        };
    }

    private static bool ParseBoolean(string cellText)
    {
        var normalizedValue = cellText.Trim();

        if (normalizedValue.Equals("true", StringComparison.OrdinalIgnoreCase))
            return true;

        if (normalizedValue.Equals("false", StringComparison.OrdinalIgnoreCase))
            return false;

        if (normalizedValue == "1")
            return true;

        if (normalizedValue == "0")
            return false;

        throw new FormatException($"Cannot parse boolean value '{cellText}'.");
    }

    private sealed class SupercellCsvEntryBuilder(IReadOnlyList<string> headers)
    {
        private readonly Dictionary<string, object?> currentValues = CreateInitialState(headers);
        private readonly List<IReadOnlyDictionary<string, object?>> continuationRows = [];
        private readonly List<IReadOnlyDictionary<string, object?>> snapshots = [];
        private IReadOnlyDictionary<string, object?>? baseRow;

        public void ApplyBaseRow(Dictionary<string, object?> values)
        {
            if (baseRow is not null)
                throw new InvalidOperationException("Base row has already been applied.");

            ApplyValues(values);
            baseRow = CreateReadOnlyCopy(values);
            snapshots.Add(CreateReadOnlyCopy(currentValues));
        }

        public void ApplyContinuationRow(Dictionary<string, object?> values)
        {
            if (baseRow is null)
                throw new InvalidOperationException("Cannot apply a continuation row before a base row.");

            ApplyValues(values);
            continuationRows.Add(CreateReadOnlyCopy(values));
            snapshots.Add(CreateReadOnlyCopy(currentValues));
        }

        public SupercellCsvEntry Build()
        {
            if (baseRow is null)
                throw new InvalidOperationException("Cannot build an entry without a base row.");

            var name = string.Empty;

            if (baseRow.TryGetValue("Name", out var nameValue) && nameValue is string parsedName)
                name = parsedName;

            return new SupercellCsvEntry(name, baseRow, continuationRows.AsReadOnly(), snapshots.AsReadOnly());
        }

        private void ApplyValues(Dictionary<string, object?> values)
        {
            foreach (var pair in values)
                currentValues[pair.Key] = pair.Value;
        }

        private static Dictionary<string, object?> CreateInitialState(IReadOnlyList<string> headers)
        {
            var state = new Dictionary<string, object?>(headers.Count, StringComparer.Ordinal);

            foreach (var header in headers)
                state[header] = null;

            return state;
        }

        private static IReadOnlyDictionary<string, object?> CreateReadOnlyCopy(Dictionary<string, object?> source)
        {
            return new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>(source, StringComparer.Ordinal));
        }
    }
}
