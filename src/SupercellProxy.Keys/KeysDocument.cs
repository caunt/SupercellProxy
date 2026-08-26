using System.Globalization;
using System.Text.RegularExpressions;

namespace SupercellProxy.Keys;

internal sealed class KeysDocument
{
    private static readonly string HeadingPatternText =
        @"^## \[(?<name>[^\]]+)\]\(https://decrypt\.day/app/id(?<id>\d+)\)\s*$";
    private static readonly string KeyCellPatternText = @"^`(?<key>[0-9A-Fa-f]{64})`$";
    private static readonly string TableSeparatorCellPatternText = @"^:?-{3,}:?$";
    private static readonly Regex HeadingRegex = new(
        HeadingPatternText,
        RegexOptions.CultureInvariant | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1)
    );
    private static readonly Regex KeyCellRegex = new(
        KeyCellPatternText,
        RegexOptions.CultureInvariant | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1)
    );
    private static readonly Regex TableSeparatorCellRegex = new(
        TableSeparatorCellPatternText,
        RegexOptions.CultureInvariant | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1)
    );
    private readonly string content;
    private readonly string[] lines;
    private readonly string newLine;

    private KeysDocument(
        string content,
        string[] lines,
        string newLine,
        IReadOnlyList<KeysSection> sections
    )
    {
        this.content = content;
        this.lines = lines;
        this.newLine = newLine;
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
        ISet<string> appIds
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
        var versions = new HashSet<string>(StringComparer.Ordinal);
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

            var version = cells[versionColumnIndex];
            var keyMatch = KeyCellRegex.Match(cells[keyColumnIndex]);
            if (version.Length is 0 || !keyMatch.Success)
                throw CreateInvalidRowException(sectionName, dataEndIndex);
            if (!versions.Add(version))
                throw new InvalidDataException(
                    $"The {sectionName} table contains version {version} more than once."
                );

            entries.Add(
                new ExistingKeyEntry(version, keyMatch.Groups["key"].Value, dataEndIndex, cells)
            );
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

        if (updates.Values.All(static update => update.NewKeys.Count is 0))
            return content;

        var (insertBefore, insertAfter, renderedSections) = PrepareRenderState(updates);

        var result = new List<string>(
            lines.Length + updates.Values.Sum(static update => update.NewKeys.Count)
        );

        for (var index = 0; index <= lines.Length; index++)
        {
            AppendInsertions(result, insertBefore, index);

            if (index == lines.Length)
                break;

            result.Add(FormatExistingLine(index));
            AppendInsertions(result, insertAfter, index);
        }

        return string.Join(newLine, result);

        void AppendInsertions(
            ICollection<string> output,
            IReadOnlyDictionary<int, List<GeneratedKeyEntry>> insertions,
            int lineIndex
        )
        {
            if (!insertions.TryGetValue(lineIndex, out var values))
                return;

            foreach (var value in values.OrderBy(static value => value.SourceIndex))
            {
                var renderedSection = renderedSections[value.AppStoreId];
                output.Add(
                    FormatTableRow(
                        CreateGeneratedCells(renderedSection.Section, value),
                        renderedSection.ColumnWidths
                    )
                );
            }
        }

        string FormatExistingLine(int lineIndex)
        {
            foreach (var renderedSection in renderedSections.Values)
            {
                var section = renderedSection.Section;

                if (lineIndex == section.HeaderIndex)
                    return FormatTableRow(section.Headers, renderedSection.ColumnWidths);

                if (lineIndex == section.SeparatorIndex)
                {
                    var cells = section
                        .Separators.Select(
                            (separator, columnIndex) =>
                                FormatSeparatorCell(
                                    separator,
                                    renderedSection.ColumnWidths[columnIndex]
                                )
                        )
                        .ToArray();

                    return FormatTableRow(cells, renderedSection.ColumnWidths);
                }

                var entry = section.Entries.FirstOrDefault(entry => entry.LineIndex == lineIndex);

                if (entry is not null)
                    return FormatTableRow(entry.Cells, renderedSection.ColumnWidths);
            }

            return lines[lineIndex];
        }
    }

    private (
        Dictionary<int, List<GeneratedKeyEntry>> InsertBefore,
        Dictionary<int, List<GeneratedKeyEntry>> InsertAfter,
        Dictionary<string, (KeysSection Section, int[] ColumnWidths)> RenderedSections
    ) PrepareRenderState(IReadOnlyDictionary<string, KeysSectionUpdate> updates)
    {
        var insertBefore = new Dictionary<int, List<GeneratedKeyEntry>>();
        var insertAfter = new Dictionary<int, List<GeneratedKeyEntry>>();
        var renderedSections = new Dictionary<string, (KeysSection Section, int[] ColumnWidths)>(
            StringComparer.Ordinal
        );
        foreach (var section in Sections)
        {
            if (updates.TryGetValue(section.AppStoreId, out var update) && update.NewKeys.Count > 0)
                AddSectionRenderState(section, update, insertBefore, insertAfter, renderedSections);
        }

        return (insertBefore, insertAfter, renderedSections);
    }

    private static void AddSectionRenderState(
        KeysSection section,
        KeysSectionUpdate update,
        IDictionary<int, List<GeneratedKeyEntry>> insertBefore,
        IDictionary<int, List<GeneratedKeyEntry>> insertAfter,
        IDictionary<string, (KeysSection Section, int[] ColumnWidths)> renderedSections
    )
    {
        var generatedCells = update
            .NewKeys.Select(key => CreateGeneratedCells(section, key))
            .ToArray();
        var columnWidths = CreateColumnWidths(section, generatedCells);
        renderedSections[section.AppStoreId] = (section, columnWidths);

        var sourceIndexes = new Dictionary<string, int>(StringComparer.Ordinal);
        for (var index = 0; index < update.SourceVersions.Count; index++)
            sourceIndexes.TryAdd(update.SourceVersions[index], index);

        var indexedExisting = section
            .Entries.Where(entry => sourceIndexes.ContainsKey(entry.Version))
            .Select(entry => (Entry: entry, SourceIndex: sourceIndexes[entry.Version]))
            .ToArray();
        foreach (var key in update.NewKeys.OrderBy(static key => key.SourceIndex))
        {
            var next = indexedExisting
                .Where(item => item.SourceIndex > key.SourceIndex)
                .OrderBy(static item => item.SourceIndex)
                .FirstOrDefault();
            if (next.Entry is not null)
            {
                AddInsertion(insertBefore, next.Entry.LineIndex, key);
                continue;
            }

            var previous = indexedExisting
                .Where(item => item.SourceIndex < key.SourceIndex)
                .OrderByDescending(static item => item.SourceIndex)
                .FirstOrDefault();
            AddInsertion(
                previous.Entry is null ? insertBefore : insertAfter,
                previous.Entry?.LineIndex ?? section.DataStartIndex,
                key
            );
        }
    }

    private static int[] CreateColumnWidths(
        KeysSection section,
        IReadOnlyList<string[]> generatedCells
    )
    {
        return Enumerable
            .Range(0, section.Headers.Count)
            .Select(columnIndex =>
            {
                var existingWidth = section.Entries.Count is 0
                    ? 0
                    : section.Entries.Max(entry => entry.Cells[columnIndex].Length);
                var generatedWidth = generatedCells.Max(cells => cells[columnIndex].Length);
                return Math.Max(
                    GetMinimumSeparatorWidth(section.Separators[columnIndex]),
                    Math.Max(
                        section.Headers[columnIndex].Length,
                        Math.Max(existingWidth, generatedWidth)
                    )
                );
            })
            .ToArray();
    }

    private static string[] CreateGeneratedCells(KeysSection section, GeneratedKeyEntry key)
    {
        var cells = Enumerable.Repeat(string.Empty, section.Headers.Count).ToArray();
        cells[section.VersionColumnIndex] = key.Version;
        cells[section.KeyColumnIndex] = $"`{key.Key}`";
        return cells;
    }

    private static string FormatTableRow(
        IReadOnlyList<string> cells,
        IReadOnlyList<int> columnWidths
    )
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

    private static void AddInsertion(
        IDictionary<int, List<GeneratedKeyEntry>> insertions,
        int lineIndex,
        GeneratedKeyEntry key
    )
    {
        if (!insertions.TryGetValue(lineIndex, out var values))
        {
            values = [];
            insertions[lineIndex] = values;
        }

        values.Add(key);
    }

    private static int NextNonEmptyLine(IReadOnlyList<string> values, int startIndex)
    {
        for (var index = startIndex; index < values.Count; index++)
        {
            if (!string.IsNullOrWhiteSpace(values[index]))
                return index;
        }

        return -1;
    }
}
