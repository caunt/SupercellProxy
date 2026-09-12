using System.Globalization;
using System.Text.RegularExpressions;

using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys;

internal sealed class KeysDocument
{
    private const string HeadingPatternText =
        @"^## \[(?<name>[^\]]+)\]\(https://decrypt\.day/app/id(?<id>\d+)\)\s*$";
    private const string KeyCellPatternText = @"^`(?<key>[0-9A-Fa-f]{64})`$";
    private const string TableSeparatorCellPatternText = @"^:?-{3,}:?$";

    private readonly string _content;
    private readonly string[] _lines;
    private readonly string _newLine;

    private KeysDocument(string content, string[] lines, string newLine, IReadOnlyList<KeysSection> sections)
    {
        this._content = content;
        this._lines = lines;
        this._newLine = newLine;
        Sections = sections;
    }

    public IReadOnlyList<KeysSection> Sections { get; }

    private static Regex HeadingRegex { get; } = new(HeadingPatternText, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(milliseconds: 1_000));

    private static Regex KeyCellRegex { get; } = new(KeyCellPatternText, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(milliseconds: 1_000));

    private static Regex TableSeparatorCellRegex { get; } = new(TableSeparatorCellPatternText, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(milliseconds: 1_000));

    public static KeysDocument Parse(string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        string detectedNewLine = content.Contains(value: "\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        string[] parsedLines = content.Split(detectedNewLine, StringSplitOptions.None);
        List<KeysSection> sections = [];
        HashSet<string> appIdentifiers = new(StringComparer.Ordinal);

        for (int headingIndex = 0; headingIndex < parsedLines.Length; headingIndex++)
        {
            Match heading = HeadingRegex.Match(parsedLines[headingIndex]);

            if (!heading.Success)
                continue;

            sections.Add(ParseSection(parsedLines, headingIndex, heading, appIdentifiers));
        }

        return sections.Count is 0
            ? throw new InvalidDataException(message: "KEYS.md does not contain any decrypt.day app sections.")
            : new KeysDocument(content, parsedLines, detectedNewLine, sections);
    }

    public string Render(IReadOnlyDictionary<string, KeysSectionUpdate> updates)
    {
        ArgumentNullException.ThrowIfNull(updates);

        RenderedKeysSection[] renderedSections = [.. Sections
            .Where(section => updates.ContainsKey(section.AppStoreIdentifier))
            .Select(section => CreateRenderedSection(section, updates[section.AppStoreIdentifier]))];

        if (renderedSections.Length is 0)
            return _content;

        Dictionary<int, RenderedKeysSection> headers = renderedSections.ToDictionary(static rendered => rendered.Section.HeaderIndex);

        Dictionary<int, RenderedKeysSection> separators = renderedSections.ToDictionary(static rendered => rendered.Section.SeparatorIndex);

        Dictionary<int, RenderedKeysSection> dataStarts = renderedSections.ToDictionary(static rendered => rendered.Section.DataStartIndex);

        List<string> result = new(_lines.Length + updates.Values.Sum(static update => update.NewKeys.Count));

        int index = 0;

        while (index <= _lines.Length)
        {
            if (dataStarts.Remove(index, out RenderedKeysSection? dataSection))
            {
                foreach (string[] row in dataSection.Rows)
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

    private static int[] CreateColumnWidths(KeysSection section, string[][] rows)
    {
        return [.. Enumerable
            .Range(start: 0, section.Headers.Count)
            .Select(
                columnIndex =>
            {
                int rowWidth = rows.Length is 0 ? 0 : rows.Max(cells => cells[columnIndex].Length);

                return Math.Max(GetMinimumSeparatorWidth(section.Separators[columnIndex]), Math.Max(section.Headers[columnIndex].Length, rowWidth));
            }
            )];
    }

    private static string[] CreateGeneratedCells(KeysSection section, string version, string key)
    {
        string[] cells = [.. Enumerable.Repeat(string.Empty, section.Headers.Count)];
        cells[section.VersionColumnIndex] = version;
        cells[section.KeyColumnIndex] = $"`{key}`";

        return cells;
    }

    private static InvalidDataException CreateInvalidRowException(string sectionName, int rowIndex)
    {
        return new InvalidDataException($"The {sectionName} table contains an invalid row at line " + string.Create(CultureInfo.InvariantCulture, $"{rowIndex + 1}."));
    }

    private static RenderedKeysSection CreateRenderedSection(KeysSection section, KeysSectionUpdate update)
    {
        Dictionary<string, (string Key, string[] Cells)> rowsByVersion = new(StringComparer.Ordinal);

        foreach (ExistingKeyEntry entry in section.Entries)
            rowsByVersion.Add(entry.Version, (entry.Key, [.. entry.Cells]));

        foreach (GeneratedKeyEntry key in update.NewKeys)
        {
            string version = AppVersion.Normalize(key.Version);

            if (rowsByVersion.TryGetValue(version, out (string Key, string[] Cells) existing))
            {
                if (!string.Equals(existing.Key, key.Key, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException($"The {section.Name} table contains conflicting keys for version {version}.");

                continue;
            }

            rowsByVersion.Add(version, (key.Key, CreateGeneratedCells(section, version, key.Key)));
        }

        string[][] rows = [.. rowsByVersion
            .Values.Select(static value => value.Cells)
            .OrderByDescending(cells => cells[section.VersionColumnIndex], AppVersion.ValueComparer)];

        return new RenderedKeysSection(section, rows, CreateColumnWidths(section, rows));
    }

    private static string FormatSeparatorCell(string separator, int width)
    {
        bool left = separator.StartsWith(value: ':');
        bool right = separator.EndsWith(value: ':');
        int colonCount = (left ? 1 : 0) + (right ? 1 : 0);

        return (left ? ":" : string.Empty)
            + new string(c: '-', width - colonCount)
            + (right ? ":" : string.Empty);
    }

    private static string FormatTableRow(IReadOnlyList<string> cells, int[] columnWidths)
    {
        return "| "
            + string.Join(separator: " | ", cells.Select((cell, index) => cell.PadRight(columnWidths[index])))
            + " |";
    }

    private static int GetMinimumSeparatorWidth(string separator)
    {
        return 3 + (separator.StartsWith(value: ':') ? 1 : 0) + (separator.EndsWith(value: ':') ? 1 : 0);
    }

    private static int NextNonEmptyLine(string[] values, int startIndex)
    {
        for (int index = startIndex; index < values.Length; index++)
        {
            if (!string.IsNullOrWhiteSpace(values[index]))
                return index;
        }

        return -1;
    }

    private static (IReadOnlyList<ExistingKeyEntry> Entries, int DataEndIndex) ParseEntries(string[] parsedLines, int dataStartIndex, int columnCount, int versionColumnIndex, int keyColumnIndex, string sectionName)
    {
        List<ExistingKeyEntry> entries = [];
        Dictionary<string, string> versions = new(StringComparer.Ordinal);
        int dataEndIndex = dataStartIndex;

        while (HasDataRow())
        {
            string[]? cells = ParseTableCells(parsedLines[dataEndIndex]);

            if (cells is null || cells.Length != columnCount)
                throw CreateInvalidRowException(sectionName, dataEndIndex);

            string sourceVersion = cells[versionColumnIndex];
            Match keyMatch = KeyCellRegex.Match(cells[keyColumnIndex]);

            if (sourceVersion.Length is 0 || !keyMatch.Success)
                throw CreateInvalidRowException(sectionName, dataEndIndex);

            string version = AppVersion.Normalize(sourceVersion);
            string key = keyMatch.Groups[groupname: "key"].Value;
            cells[versionColumnIndex] = version;

            if (versions.TryGetValue(version, out string? existingKey))
            {
                if (!string.Equals(existingKey, key, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException($"The {sectionName} table contains conflicting keys for version {version}.");

                dataEndIndex++;

                continue;
            }

            versions[version] = key;

            entries.Add(new ExistingKeyEntry(version, key, dataEndIndex, cells));
            dataEndIndex++;
        }

        return (entries, dataEndIndex);

        bool HasDataRow()
        {
            return dataEndIndex < parsedLines.Length
            && !string.IsNullOrWhiteSpace(parsedLines[dataEndIndex])
            && !parsedLines[dataEndIndex].StartsWith(value: "## ", StringComparison.Ordinal);
        }
    }

    private static KeysSection ParseSection(string[] parsedLines, int headingIndex, Match heading, HashSet<string> appIdentifiers)
    {
        string appIdentifier = heading.Groups[groupname: "id"].Value;
        string name = heading.Groups[groupname: "name"].Value;

        if (!appIdentifiers.Add(appIdentifier))
            throw new InvalidDataException($"KEYS.md contains app ID {appIdentifier} more than once.");

        int headerIndex = NextNonEmptyLine(parsedLines, headingIndex + 1);
        string[]? headers = headerIndex < 0 ? null : ParseTableCells(parsedLines[headerIndex]);

        int versionColumnIndex = headers is null
            ? -1
            : Array.FindIndex(headers, static header => string.Equals(header, b: "Version", StringComparison.OrdinalIgnoreCase));

        int keyColumnIndex = headers is null
            ? -1
            : Array.FindIndex(headers, static header => string.Equals(header, b: "Key", StringComparison.OrdinalIgnoreCase));

        if (headers is null || versionColumnIndex < 0 || keyColumnIndex < 0)
            throw new InvalidDataException($"The {name} section does not contain a Version/Key table.");

        int separatorIndex = NextNonEmptyLine(parsedLines, headerIndex + 1);
        string[]? separators = separatorIndex < 0 ? null : ParseTableCells(parsedLines[separatorIndex]);

        if (separators is null || separators.Length != headers.Length)
            throw new InvalidDataException($"The {name} table has an invalid separator row.");

        if (separators.Any(static separator => !TableSeparatorCellRegex.IsMatch(separator)))
            throw new InvalidDataException($"The {name} table has an invalid separator row.");

        int dataStartIndex = separatorIndex + 1;
        (IReadOnlyList<ExistingKeyEntry>? entries, int dataEndIndex) = ParseEntries(parsedLines, dataStartIndex, headers.Length, versionColumnIndex, keyColumnIndex, name);

        return new KeysSection(
            name,
            appIdentifier,
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

    private static string[]? ParseTableCells(string line)
    {
        string trimmed = line.Trim();

        return trimmed.Length < 2 || trimmed[index: 0] != '|' || trimmed[new Index(value: 1, fromEnd: true)] != '|'
            ? null
            : [.. trimmed[1..^1]
            .Split(separator: '|', StringSplitOptions.None)
            .Select(static cell => cell.Trim())];
    }

    private string FormatLine(int lineIndex, IReadOnlyDictionary<int, RenderedKeysSection> headers, IReadOnlyDictionary<int, RenderedKeysSection> separators)
    {
        if (headers.TryGetValue(lineIndex, out RenderedKeysSection? headerSection))
            return FormatTableRow(headerSection.Section.Headers, headerSection.ColumnWidths);

        if (!separators.TryGetValue(lineIndex, out RenderedKeysSection? separatorSection))
            return _lines[lineIndex];

        string[] cells = [.. separatorSection
            .Section.Separators.Select((separator, columnIndex) => FormatSeparatorCell(separator, separatorSection.ColumnWidths[columnIndex]))];

        return FormatTableRow(cells, separatorSection.ColumnWidths);
    }

    private sealed record RenderedKeysSection(KeysSection Section, string[][] Rows, int[] ColumnWidths);
}
