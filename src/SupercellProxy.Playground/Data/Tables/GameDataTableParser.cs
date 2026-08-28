using System.Globalization;
using System.Text;

namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c language="csharp">GameDataTableParser</c>.
/// </summary>
internal static class GameDataTableParser
{
    /// <summary>
    /// Executes the <c language="csharp">Parse</c> operation.
    /// </summary>
    public static GameDataTable Parse(string csvText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(csvText);

        var rows = ParsePhysicalRows(csvText);

        if (rows.Count < 2)
            throw new FormatException("Expected at least a header row and a type row.");

        var headers = rows[0];
        var types = NormalizeDataRow(rows[1], headers.Count, rowIndex: 1);
        var entryBuilders = new List<GameDataTableEntryBuilder>();

        GameDataTableEntryBuilder? currentEntryBuilder = null;

        for (var rowIndex = 2; rowIndex < rows.Count; rowIndex++)
        {
            var row = NormalizeDataRow(rows[rowIndex], headers.Count, rowIndex);

            if (IsEmptyRow(row))
                continue;

            var parsedValues = ParseSparseValues(headers, types, row);

            var startsNewEntry = !string.IsNullOrWhiteSpace(row[0]);

            if (startsNewEntry)
            {
                currentEntryBuilder = new GameDataTableEntryBuilder(headers);
                currentEntryBuilder.ApplyBaseRow(parsedValues);
                entryBuilders.Add(currentEntryBuilder);
                continue;
            }

            if (currentEntryBuilder is null)
                throw new FormatException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Continuation row at index {rowIndex} appeared before any entry start row."
                    )
                );

            currentEntryBuilder.ApplyContinuationRow(parsedValues);
        }

        var entries = new List<GameDataTableEntry>(entryBuilders.Count);

        foreach (var entryBuilder in entryBuilders)
            entries.Add(entryBuilder.Build());

        return new GameDataTable(headers.AsReadOnly(), types.AsReadOnly(), entries.AsReadOnly());
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
                AppendQuotedCharacter(
                    csvText,
                    ref index,
                    currentCharacter,
                    currentCell,
                    ref insideQuotes
                );
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

        return CompletePhysicalRows(rows, currentRow, currentCell, insideQuotes);
    }

    private static void AppendQuotedCharacter(
        string csvText,
        ref int index,
        char currentCharacter,
        StringBuilder currentCell,
        ref bool insideQuotes
    )
    {
        if (currentCharacter != '"')
        {
            currentCell.Append(currentCharacter);
            return;
        }

        var nextIndex = index + 1;
        if (nextIndex < csvText.Length && csvText[nextIndex] == '"')
        {
            currentCell.Append('"');
            index++;
            return;
        }

        insideQuotes = false;
    }

    private static List<List<string>> CompletePhysicalRows(
        List<List<string>> rows,
        List<string> currentRow,
        StringBuilder currentCell,
        bool insideQuotes
    )
    {
        if (insideQuotes)
            throw new FormatException("CSV ended while still inside a quoted field.");

        currentRow.Add(currentCell.ToString());

        if (currentRow.Count > 1 || currentRow[0].Length > 0)
            rows.Add(currentRow);

        return rows;
    }

    private static List<string> NormalizeDataRow(
        List<string> row,
        int expectedColumnCount,
        int rowIndex
    )
    {
        if (row.Count > expectedColumnCount)
            throw new FormatException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Row {rowIndex} has {row.Count} columns but the header declares {expectedColumnCount} columns."
                )
            );

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

    private static Dictionary<string, object?> ParseSparseValues(
        List<string> headers,
        List<string> types,
        List<string> row
    )
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
        var normalizedType = declaredType.Trim().ToUpperInvariant();

        return normalizedType switch
        {
            "INT" => int.Parse(cellText.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture),
            "LONG" => long.Parse(
                cellText.Trim(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture
            ),
            "FLOAT" => float.Parse(
                cellText.Trim(),
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture
            ),
            "DOUBLE" => double.Parse(
                cellText.Trim(),
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture
            ),
            "BOOLEAN" or "BOOL" => ParseBoolean(cellText),
            "STRING" => cellText,
            _ => cellText,
        };
    }

    private static bool ParseBoolean(string cellText)
    {
        var normalizedValue = cellText.Trim();

        if (normalizedValue.Equals("true", StringComparison.OrdinalIgnoreCase))
            return true;

        if (normalizedValue.Equals("false", StringComparison.OrdinalIgnoreCase))
            return false;

        if (string.Equals(normalizedValue, "1", StringComparison.Ordinal))
            return true;

        if (string.Equals(normalizedValue, "0", StringComparison.Ordinal))
            return false;

        throw new FormatException($"Cannot parse boolean value '{cellText}'.");
    }
}
