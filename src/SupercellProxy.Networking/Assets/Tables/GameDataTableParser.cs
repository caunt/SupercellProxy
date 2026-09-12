using System.Globalization;
using System.Runtime.InteropServices;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Represents <c language="csharp">GameDataTableParser</c>.
/// </summary>
public static class GameDataTableParser
{
    /// <summary>
    /// Executes the <c language="csharp">Parse</c> operation.
    /// </summary>
    public static GameDataTable Parse(string csvText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(csvText);

        List<List<string>> rows = ParseMutableRows(csvText);

        if (rows.Count < 2)
            throw new FormatException(message: "Expected at least a header row and a type row.");

        List<string> headers = rows[index: 0];
        List<string> types = NormalizeDataRow(rows[index: 1], headers.Count, rowIndex: 1);
        List<GameDataTableEntryBuilder> entryBuilders = [];

        GameDataTableEntryBuilder? currentEntryBuilder = null;

        for (int rowIndex = 2; rowIndex < rows.Count; rowIndex++)
        {
            List<string> row = NormalizeDataRow(rows[rowIndex], headers.Count, rowIndex);

            if (IsEmptyRow(row))
                continue;

            Dictionary<string, LiteralValue> parsedValues = ParseSparseValues(headers, types, row);

            bool startsNewEntry = !string.IsNullOrWhiteSpace(row[index: 0]);

            if (startsNewEntry)
            {
                currentEntryBuilder = new GameDataTableEntryBuilder(headers, row[index: 0]);
                currentEntryBuilder.ApplyBaseRow(parsedValues);
                entryBuilders.Add(currentEntryBuilder);

                continue;
            }

            if (currentEntryBuilder is null)
                throw new FormatException(string.Create(CultureInfo.InvariantCulture, $"Continuation row at index {rowIndex} appeared before any entry start row."));

            currentEntryBuilder.ApplyContinuationRow(parsedValues);
        }

        List<GameDataTableEntry> entries = new(entryBuilders.Count);

        foreach (GameDataTableEntryBuilder entryBuilder in entryBuilders)
            entries.Add(entryBuilder.Build());

        return new GameDataTable(headers.AsReadOnly(), types.AsReadOnly(), entries.AsReadOnly());
    }

    /// <summary>
    /// Provides the Parse Physical Rows value or operation.
    /// </summary>
    public static IReadOnlyList<IReadOnlyList<string>> ParsePhysicalRows(string csvText)
    {
        return ParseMutableRows(csvText).AsReadOnly();
    }

    internal static List<List<string>> ParseMutableRows(string csvText)
    {
        ArgumentNullException.ThrowIfNull(csvText);
        List<List<string>> rows = [];
        List<string> currentRow = [];
        List<char> currentCell = [];
        bool insideQuotes = false;

        for (int index = 0; index < csvText.Length; index++)
        {
            char currentCharacter = csvText[index];

            if (insideQuotes)
            {
                AppendQuotedCharacter(csvText, ref index, currentCharacter, currentCell, ref insideQuotes);

                continue;
            }

            if (currentCharacter == '"')
            {
                insideQuotes = true;

                continue;
            }

            if (currentCharacter == ',')
            {
                currentRow.Add(new string(CollectionsMarshal.AsSpan(currentCell)));
                currentCell.Clear();

                continue;
            }

            if (currentCharacter == '\r')
                continue;

            if (currentCharacter == '\n')
            {
                currentRow.Add(new string(CollectionsMarshal.AsSpan(currentCell)));
                currentCell.Clear();

                rows.Add(currentRow);
                currentRow = [];

                continue;
            }

            currentCell.Add(currentCharacter);
        }

        return CompletePhysicalRows(rows, currentRow, currentCell, insideQuotes);
    }

    private static void AppendQuotedCharacter(string csvText, ref int index, char currentCharacter, List<char> currentCell, ref bool insideQuotes)
    {
        if (currentCharacter != '"')
        {
            currentCell.Add(currentCharacter);

            return;
        }

        int nextIndex = index + 1;

        if (nextIndex < csvText.Length && csvText[nextIndex] == '"')
        {
            currentCell.Add(item: '"');
            index++;

            return;
        }

        insideQuotes = false;
    }

    private static List<List<string>> CompletePhysicalRows(List<List<string>> rows, List<string> currentRow, List<char> currentCell, bool insideQuotes)
    {
        if (insideQuotes)
            throw new FormatException(message: "CSV ended while still inside a quoted field.");

        currentRow.Add(new string(CollectionsMarshal.AsSpan(currentCell)));

        if (currentRow.Count > 1 || currentRow[index: 0].Length > 0)
            rows.Add(currentRow);

        return rows;
    }

    private static bool IsEmptyRow(List<string> row)
    {
        foreach (string value in row)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return false;
        }

        return true;
    }

    private static List<string> NormalizeDataRow(List<string> row, int expectedColumnCount, int rowIndex)
    {
        if (row.Count > expectedColumnCount)
        {
            throw new FormatException(
                string.Create(CultureInfo.InvariantCulture, $"Row {rowIndex} has {row.Count} columns but the header declares {expectedColumnCount} columns.")
            );
        }

        if (row.Count == expectedColumnCount)
            return row;

        List<string> normalizedRow = new(expectedColumnCount);
        normalizedRow.AddRange(row);

        while (normalizedRow.Count < expectedColumnCount)
            normalizedRow.Add(string.Empty);

        return normalizedRow;
    }

    private static bool ParseBoolean(string cellText)
    {
        string normalizedValue = cellText.Trim();

        return normalizedValue.Equals(value: "true", StringComparison.OrdinalIgnoreCase) || (!normalizedValue.Equals(value: "false", StringComparison.OrdinalIgnoreCase) && (string.Equals(normalizedValue, b: "1", StringComparison.Ordinal) || (string.Equals(normalizedValue, b: "0", StringComparison.Ordinal)
            ? false
            : throw new FormatException($"Cannot parse boolean value '{cellText}'."))));
    }

    private static Dictionary<string, LiteralValue> ParseSparseValues(List<string> headers, List<string> types, List<string> row)
    {
        Dictionary<string, LiteralValue> values = new(headers.Count, StringComparer.Ordinal);

        for (int index = 0; index < headers.Count; index++)
        {
            string cellText = row[index];

            if (string.IsNullOrWhiteSpace(cellText))
                continue;

            values[headers[index]] = ParseValue(cellText, types[index]);
        }

        return values;
    }

    private static LiteralValue ParseValue(string cellText, string declaredType)
    {
        string normalizedType = declaredType.Trim().ToUpperInvariant();

        return normalizedType switch
        {
            "INT" => new LiteralValue(int.Parse(cellText.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture)),
            "LONG" => new LiteralValue(long.Parse(cellText.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture)),
            "FLOAT" => new LiteralValue(float.Parse(cellText.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture)),
            "DOUBLE" => new LiteralValue(double.Parse(cellText.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture)),
            "BOOLEAN" or "BOOL" => new LiteralValue(ParseBoolean(cellText)),
            "STRING" => new LiteralValue(cellText),
            _ => new LiteralValue(cellText),
        };
    }
}
