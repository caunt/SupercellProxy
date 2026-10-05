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
        HashSet<string> appIds = new(StringComparer.Ordinal);

        for (int headingIndex = 0; headingIndex < parsedLines.Length; headingIndex++)
        {
            Match heading = HeadingRegex.Match(parsedLines[headingIndex]);

            if (!heading.Success)
                continue;

            sections.Add(ParseSection(parsedLines, headingIndex, heading, appIds));
        }

        return sections.Count is 0
            ? throw new InvalidDataException(message: "KEYS.md does not contain any decrypt.day app sections.")
            : new KeysDocument(content, parsedLines, detectedNewLine, sections);
    }

    public string Render(IReadOnlyDictionary<string, KeysSectionUpdate> updates)
    {
        ArgumentNullException.ThrowIfNull(updates);

        RenderedKeysSection[] renderedSections = [.. Sections
            .Where(section => updates.ContainsKey(section.AppStoreId))
            .Select(section => CreateRenderedSection(section, updates[section.AppStoreId]))];

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

    private static string[] CreateGeneratedCells(KeysSection section, string version, string key, int? keyVersion)
    {
        string[] cells = [.. Enumerable.Repeat(string.Empty, section.Headers.Count)];
        cells[section.VersionColumnIndex] = version;
        cells[section.KeyVersionColumnIndex] = keyVersion?.ToString(CultureInfo.InvariantCulture) ?? "—";
        cells[section.KeyColumnIndex] = $"`{key}`";

        return cells;
    }

    private static InvalidDataException CreateInvalidRowException(string sectionName, int rowIndex)
    {
        return new InvalidDataException($"The {sectionName} table contains an invalid row at line " + string.Create(CultureInfo.InvariantCulture, $"{rowIndex + 1}."));
    }

    private static RenderedKeysSection CreateRenderedSection(KeysSection section, KeysSectionUpdate update)
    {
        Dictionary<string, (string Key, int? KeyVersion, string[] Cells)> rowsByVersion = new(StringComparer.Ordinal);

        foreach (ExistingKeyEntry entry in section.Entries)
            rowsByVersion.Add(entry.Version, (entry.Key, entry.KeyVersion, [.. entry.Cells]));

        foreach (GeneratedKeyEntry key in update.NewKeys)
        {
            string version = AppVersion.Normalize(key.Version);

            if (rowsByVersion.TryGetValue(version, out (string Key, int? KeyVersion, string[] Cells) existing))
            {
                if (!string.Equals(existing.Key, key.Key, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException($"The {section.Name} table contains conflicting keys for version {version}.");

                if (existing.KeyVersion is not null && key.KeyVersion is not null && existing.KeyVersion != key.KeyVersion)
                    throw new InvalidDataException($"The {section.Name} table contains conflicting key versions for version {version}.");

                if (key.KeyVersion is not null)
                {
                    existing.Cells[section.KeyVersionColumnIndex] = key.KeyVersion.Value.ToString(CultureInfo.InvariantCulture);
                    rowsByVersion[version] = (existing.Key, key.KeyVersion, existing.Cells);
                }

                continue;
            }

            rowsByVersion.Add(version, (key.Key, key.KeyVersion, CreateGeneratedCells(section, version, key.Key, key.KeyVersion)));
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

    private static KeyDocumentRows ParseEntries(
        string[] parsedLines,
        int dataStartIndex,
        int columnCount,
        int versionColumnIndex,
        int keyColumnIndex,
        int keyVersionColumnIndex,
        string sectionName
    )
    {
        List<ExistingKeyEntry> entries = [];
        Dictionary<string, int> versions = new(StringComparer.Ordinal);
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
            int? keyVersion = null;
            string keyVersionCell = keyVersionColumnIndex < 0 ? "—" : cells[keyVersionColumnIndex];

            if (keyVersionCell is not ("" or "—"))
            {
                if (!int.TryParse(keyVersionCell, NumberStyles.None, CultureInfo.InvariantCulture, out int parsed) || parsed <= 0)
                    throw CreateInvalidRowException(sectionName, dataEndIndex);

                keyVersion = parsed;
            }

            string[] normalizedCells = [version, keyVersion?.ToString(CultureInfo.InvariantCulture) ?? "—", $"`{key}`",
                .. Enumerable.Range(start: 0, cells.Length)
                    .Where(index => index != versionColumnIndex && index != keyColumnIndex && index != keyVersionColumnIndex)
                    .Select(index => cells[index])];

            if (versions.TryGetValue(version, out int existingIndex))
            {
                ExistingKeyEntry existing = entries[existingIndex];

                if (!string.Equals(existing.Key, key, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException($"The {sectionName} table contains conflicting keys for version {version}.");

                if (existing.KeyVersion is not null && keyVersion is not null && existing.KeyVersion != keyVersion)
                    throw new InvalidDataException($"The {sectionName} table contains conflicting key versions for version {version}.");

                if (existing.KeyVersion is null && keyVersion is not null)
                    entries[existingIndex] = new ExistingKeyEntry(version, key, keyVersion, dataEndIndex, normalizedCells);

                dataEndIndex++;

                continue;
            }

            versions[version] = entries.Count;

            entries.Add(new ExistingKeyEntry(version, key, keyVersion, dataEndIndex, normalizedCells));
            dataEndIndex++;
        }

        return new KeyDocumentRows(entries, dataEndIndex);

        bool HasDataRow()
        {
            return dataEndIndex < parsedLines.Length
            && !string.IsNullOrWhiteSpace(parsedLines[dataEndIndex])
            && !parsedLines[dataEndIndex].StartsWith(value: "## ", StringComparison.Ordinal);
        }
    }

    private static KeysSection ParseSection(string[] parsedLines, int headingIndex, Match heading, HashSet<string> appIds)
    {
        string appId = heading.Groups[groupname: "id"].Value;
        string name = heading.Groups[groupname: "name"].Value;

        if (!appIds.Add(appId))
            throw new InvalidDataException($"KEYS.md contains app ID {appId} more than once.");

        int headerIndex = NextNonEmptyLine(parsedLines, headingIndex + 1);
        string[]? headers = headerIndex < 0 ? null : ParseTableCells(parsedLines[headerIndex]);

        int versionColumnIndex = headers is null
            ? -1
            : Array.FindIndex(
                headers,
                static header => header.Equals(value: "Version", StringComparison.OrdinalIgnoreCase)
                || header.Equals(value: "Game version", StringComparison.OrdinalIgnoreCase)
            );

        int keyVersionColumnIndex = headers is null
            ? -1
            : Array.FindIndex(headers, static header => header.Equals(value: "Key version", StringComparison.OrdinalIgnoreCase));

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
        (IReadOnlyList<ExistingKeyEntry>? entries, int dataEndIndex) = ParseEntries(parsedLines, dataStartIndex, headers.Length, versionColumnIndex, keyColumnIndex, keyVersionColumnIndex, name);

        int[] extraColumns = [.. Enumerable.Range(start: 0, headers.Length)
            .Where(index => index != versionColumnIndex && index != keyColumnIndex && index != keyVersionColumnIndex)];

        return new KeysSection(
            name,
            appId,
            headerIndex,
            separatorIndex,
            dataStartIndex,
            dataEndIndex,
            ["Game version", "Key version", "Key", .. extraColumns.Select(index => headers[index])],
            [separators[versionColumnIndex], keyVersionColumnIndex < 0 ? "---" : separators[keyVersionColumnIndex],
                separators[keyColumnIndex], .. extraColumns.Select(index => separators[index])],
            VersionColumnIndex: 0,
            KeyVersionColumnIndex: 1,
            KeyColumnIndex: 2,
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
