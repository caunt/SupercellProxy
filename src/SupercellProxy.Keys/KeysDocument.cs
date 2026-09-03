using System.Globalization;
using System.Text.RegularExpressions;

namespace SupercellProxy.Keys;

internal sealed partial class KeysDocument
{
    private const string HeadingPatternText =
        @"^## \[(?<name>[^\]]+)\]\(https://decrypt\.day/app/id(?<id>\d+)\)\s*$";
    private const string KeyCellPatternText = @"^`(?<key>[0-9A-Fa-f]{64})`$";
    private const string TableSeparatorCellPatternText = @"^:?-{3,}:?$";

    [GeneratedRegex(
        HeadingPatternText,
        RegexOptions.CultureInvariant,
        matchTimeoutMilliseconds: 1_000
    )]
    private static partial Regex HeadingRegex { get; }

    [GeneratedRegex(
        KeyCellPatternText,
        RegexOptions.CultureInvariant,
        matchTimeoutMilliseconds: 1_000
    )]
    private static partial Regex KeyCellRegex { get; }

    [GeneratedRegex(
        TableSeparatorCellPatternText,
        RegexOptions.CultureInvariant,
        matchTimeoutMilliseconds: 1_000
    )]
    private static partial Regex TableSeparatorCellRegex { get; }

    private readonly string _content;
    private readonly string[] _lines;
    private readonly string _newLine;

    private KeysDocument(
        string content,
        string[] lines,
        string newLine,
        IReadOnlyList<KeysSection> sections
    )
    {
        this._content = content;
        this._lines = lines;
        this._newLine = newLine;
        Sections = sections;
    }

    public IReadOnlyList<KeysSection> Sections { get; }

    public static KeysDocument Parse(string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var detectedNewLine = content.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        var parsedLines = content.Split(detectedNewLine, StringSplitOptions.None);
        var sections = new List<KeysSection>();
        var appIds = new HashSet<string>(StringComparer.Ordinal);

        for (var headingIndex = 0; headingIndex < parsedLines.Length; headingIndex++)
        {
            var heading = HeadingRegex.Match(parsedLines[headingIndex]);

            if (!heading.Success)
                continue;

            sections.Add(ParseSection(parsedLines, headingIndex, heading, appIds));
        }

        if (sections.Count is 0)
            throw new InvalidDataException(
                "KEYS.md does not contain any decrypt.day app sections."
            );

        return new KeysDocument(content, parsedLines, detectedNewLine, sections);
    }

    private static KeysSection ParseSection(
        string[] parsedLines,
        int headingIndex,
        Match heading,
        HashSet<string> appIds
    )
    {
        var appId = heading.Groups["id"].Value;
        var name = heading.Groups["name"].Value;
        if (!appIds.Add(appId))
            throw new InvalidDataException($"KEYS.md contains app ID {appId} more than once.");

        var headerIndex = NextNonEmptyLine(parsedLines, headingIndex + 1);
        var headers = headerIndex < 0 ? null : ParseTableCells(parsedLines[headerIndex]);
        var versionColumnIndex = headers is null
            ? -1
            : Array.FindIndex(
                headers,
                static header =>
                    string.Equals(header, "Version", StringComparison.OrdinalIgnoreCase)
            );
        var keyColumnIndex = headers is null
            ? -1
            : Array.FindIndex(
                headers,
                static header => string.Equals(header, "Key", StringComparison.OrdinalIgnoreCase)
            );
        if (headers is null || versionColumnIndex < 0 || keyColumnIndex < 0)
            throw new InvalidDataException(
                $"The {name} section does not contain a Version/Key table."
            );

        var separatorIndex = NextNonEmptyLine(parsedLines, headerIndex + 1);
        var separators = separatorIndex < 0 ? null : ParseTableCells(parsedLines[separatorIndex]);
        if (
            separators is null
            || separators.Length != headers.Length
            || separators.Any(static separator => !TableSeparatorCellRegex.IsMatch(separator))
        )
        {
            throw new InvalidDataException($"The {name} table has an invalid separator row.");
        }

        var dataStartIndex = separatorIndex + 1;
        var (entries, dataEndIndex) = ParseEntries(
            parsedLines,
            dataStartIndex,
            headers.Length,
            versionColumnIndex,
            keyColumnIndex,
            name
        );
        return new KeysSection(
            name,
            appId,
            headerIndex,
            separatorIndex,
            dataStartIndex,
            dataEndIndex,
            headers,
            separators,
            versionColumnIndex,
            keyColumnIndex,
            entries
        );
    }

    private static (IReadOnlyList<ExistingKeyEntry> Entries, int DataEndIndex) ParseEntries(
        string[] parsedLines,
        int dataStartIndex,
        int columnCount,
        int versionColumnIndex,
        int keyColumnIndex,
        string sectionName
    )
    {
        var entries = new List<ExistingKeyEntry>();
        var versions = new Dictionary<string, string>(StringComparer.Ordinal);
        var dataEndIndex = dataStartIndex;
        while (
            dataEndIndex < parsedLines.Length
            && !string.IsNullOrWhiteSpace(parsedLines[dataEndIndex])
            && !parsedLines[dataEndIndex].StartsWith("## ", StringComparison.Ordinal)
        )
        {
            var cells = ParseTableCells(parsedLines[dataEndIndex]);
            if (cells is null || cells.Length != columnCount)
                throw CreateInvalidRowException(sectionName, dataEndIndex);

            var sourceVersion = cells[versionColumnIndex];
            var keyMatch = KeyCellRegex.Match(cells[keyColumnIndex]);
            if (sourceVersion.Length is 0 || !keyMatch.Success)
                throw CreateInvalidRowException(sectionName, dataEndIndex);
            var version = AppVersion.Normalize(sourceVersion);
            var key = keyMatch.Groups["key"].Value;
            cells[versionColumnIndex] = version;

            if (versions.TryGetValue(version, out var existingKey))
            {
                if (!string.Equals(existingKey, key, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidDataException(
                        $"The {sectionName} table contains conflicting keys for version {version}."
                    );
                }

                dataEndIndex++;
                continue;
            }

            versions[version] = key;

            entries.Add(new ExistingKeyEntry(version, key, dataEndIndex, cells));
            dataEndIndex++;
        }

        return (entries, dataEndIndex);
    }

    private static InvalidDataException CreateInvalidRowException(string sectionName, int rowIndex)
    {
        return new InvalidDataException(
            $"The {sectionName} table contains an invalid row at line "
                + string.Create(CultureInfo.InvariantCulture, $"{rowIndex + 1}.")
        );
    }

    public string Render(IReadOnlyDictionary<string, KeysSectionUpdate> updates)
    {
        ArgumentNullException.ThrowIfNull(updates);

        var renderedSections = Sections
            .Where(section => updates.ContainsKey(section.AppStoreId))
            .Select(section => CreateRenderedSection(section, updates[section.AppStoreId]))
            .ToArray();
        if (renderedSections.Length is 0)
            return _content;

        var headers = renderedSections.ToDictionary(static rendered =>
            rendered.Section.HeaderIndex
        );
        var separators = renderedSections.ToDictionary(static rendered =>
            rendered.Section.SeparatorIndex
        );
        var dataStarts = renderedSections.ToDictionary(static rendered =>
            rendered.Section.DataStartIndex
        );
        var result = new List<string>(
            _lines.Length + updates.Values.Sum(static update => update.NewKeys.Count)
        );
        var index = 0;
        while (index <= _lines.Length)
        {
            if (dataStarts.Remove(index, out var dataSection))
            {
                foreach (var row in dataSection.Rows)
                    result.Add(FormatTableRow(row, dataSection.ColumnWidths));

                if (dataSection.Section.DataEndIndex > index)
                {
                    index = dataSection.Section.DataEndIndex;
                    continue;
                }
            }

            if (index == _lines.Length)
                break;

            result.Add(FormatLine(index, headers, separators));

            index++;
        }

        return string.Join(_newLine, result);
    }

    private string FormatLine(
        int lineIndex,
        IReadOnlyDictionary<int, RenderedKeysSection> headers,
        IReadOnlyDictionary<int, RenderedKeysSection> separators
    )
    {
        if (headers.TryGetValue(lineIndex, out var headerSection))
            return FormatTableRow(headerSection.Section.Headers, headerSection.ColumnWidths);

        if (!separators.TryGetValue(lineIndex, out var separatorSection))
            return _lines[lineIndex];

        var cells = separatorSection
            .Section.Separators.Select(
                (separator, columnIndex) =>
                    FormatSeparatorCell(separator, separatorSection.ColumnWidths[columnIndex])
            )
            .ToArray();
        return FormatTableRow(cells, separatorSection.ColumnWidths);
    }

    private static RenderedKeysSection CreateRenderedSection(
        KeysSection section,
        KeysSectionUpdate update
    )
    {
        var rowsByVersion = new Dictionary<string, (string Key, string[] Cells)>(
            StringComparer.Ordinal
        );
        foreach (var entry in section.Entries)
            rowsByVersion.Add(entry.Version, (entry.Key, [.. entry.Cells]));

        foreach (var key in update.NewKeys)
        {
            var version = AppVersion.Normalize(key.Version);
            if (rowsByVersion.TryGetValue(version, out var existing))
            {
                if (!string.Equals(existing.Key, key.Key, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidDataException(
                        $"The {section.Name} table contains conflicting keys for version {version}."
                    );
                }

                continue;
            }

            rowsByVersion.Add(version, (key.Key, CreateGeneratedCells(section, version, key.Key)));
        }

        var rows = rowsByVersion
            .Values.Select(static value => value.Cells)
            .OrderByDescending(cells => cells[section.VersionColumnIndex], AppVersion.ValueComparer)
            .ToArray();
        return new RenderedKeysSection(section, rows, CreateColumnWidths(section, rows));
    }

    private static int[] CreateColumnWidths(KeysSection section, string[][] rows)
    {
        return Enumerable
            .Range(0, section.Headers.Count)
            .Select(columnIndex =>
            {
                var rowWidth = rows.Length is 0 ? 0 : rows.Max(cells => cells[columnIndex].Length);
                return Math.Max(
                    GetMinimumSeparatorWidth(section.Separators[columnIndex]),
                    Math.Max(section.Headers[columnIndex].Length, rowWidth)
                );
            })
            .ToArray();
    }

    private static string[] CreateGeneratedCells(KeysSection section, string version, string key)
    {
        var cells = Enumerable.Repeat(string.Empty, section.Headers.Count).ToArray();
        cells[section.VersionColumnIndex] = version;
        cells[section.KeyColumnIndex] = $"`{key}`";
        return cells;
    }

    private static string FormatTableRow(IReadOnlyList<string> cells, int[] columnWidths)
    {
        return "| "
            + string.Join(" | ", cells.Select((cell, index) => cell.PadRight(columnWidths[index])))
            + " |";
    }

    private static int GetMinimumSeparatorWidth(string separator)
    {
        return 3 + (separator.StartsWith(':') ? 1 : 0) + (separator.EndsWith(':') ? 1 : 0);
    }

    private static string FormatSeparatorCell(string separator, int width)
    {
        var left = separator.StartsWith(':');
        var right = separator.EndsWith(':');
        var colonCount = (left ? 1 : 0) + (right ? 1 : 0);

        return (left ? ":" : string.Empty)
            + new string('-', width - colonCount)
            + (right ? ":" : string.Empty);
    }

    private static string[]? ParseTableCells(string line)
    {
        var trimmed = line.Trim();

        if (trimmed.Length < 2 || trimmed[0] != '|' || trimmed[^1] != '|')
            return null;

        return trimmed[1..^1]
            .Split('|', StringSplitOptions.None)
            .Select(static cell => cell.Trim())
            .ToArray();
    }

    private static int NextNonEmptyLine(string[] values, int startIndex)
    {
        for (var index = startIndex; index < values.Length; index++)
        {
            if (!string.IsNullOrWhiteSpace(values[index]))
                return index;
        }

        return -1;
    }

    private sealed record RenderedKeysSection(
        KeysSection Section,
        string[][] Rows,
        int[] ColumnWidths
    );
}
