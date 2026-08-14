using System.Text.RegularExpressions;

namespace SupercellProxy.Keys;

internal sealed partial class KeysDocument
{
    private readonly string content;
    private readonly string[] lines;
    private readonly string newLine;

    private KeysDocument(string content, string[] lines, string newLine, IReadOnlyList<KeysSection> sections)
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

        var newLine = content.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        var lines = content.Split(newLine, StringSplitOptions.None);
        var sections = new List<KeysSection>();
        var appIds = new HashSet<string>(StringComparer.Ordinal);

        for (var headingIndex = 0; headingIndex < lines.Length; headingIndex++)
        {
            var heading = HeadingPattern().Match(lines[headingIndex]);

            if (!heading.Success)
                continue;

            var appId = heading.Groups["id"].Value;

            if (!appIds.Add(appId))
                throw new InvalidDataException($"KEYS.md contains app ID {appId} more than once.");

            var headerIndex = NextNonEmptyLine(lines, headingIndex + 1);
            var headers = headerIndex < 0 ? null : ParseTableCells(lines[headerIndex]);
            var versionColumnIndex = headers is null
                ? -1
                : Array.FindIndex(headers, header =>
                    string.Equals(header, "Version", StringComparison.OrdinalIgnoreCase));
            var keyColumnIndex = headers is null
                ? -1
                : Array.FindIndex(headers, header =>
                    string.Equals(header, "Key", StringComparison.OrdinalIgnoreCase));

            if (headers is null || versionColumnIndex < 0 || keyColumnIndex < 0)
            {
                throw new InvalidDataException(
                    $"The {heading.Groups["name"].Value} section does not contain a Version/Key table.");
            }

            var separatorIndex = NextNonEmptyLine(lines, headerIndex + 1);
            var separators = separatorIndex < 0 ? null : ParseTableCells(lines[separatorIndex]);

            if (separators is null ||
                separators.Length != headers.Length ||
                separators.Any(separator => !TableSeparatorCellPattern().IsMatch(separator)))
            {
                throw new InvalidDataException(
                    $"The {heading.Groups["name"].Value} table has an invalid separator row.");
            }

            var entries = new List<ExistingKeyEntry>();
            var versions = new HashSet<string>(StringComparer.Ordinal);
            var dataStartIndex = separatorIndex + 1;
            var dataEndIndex = dataStartIndex;

            while (dataEndIndex < lines.Length &&
                   !string.IsNullOrWhiteSpace(lines[dataEndIndex]) &&
                   !lines[dataEndIndex].StartsWith("## ", StringComparison.Ordinal))
            {
                var cells = ParseTableCells(lines[dataEndIndex]);

                if (cells is null || cells.Length != headers.Length)
                {
                    throw new InvalidDataException(
                        $"The {heading.Groups["name"].Value} table contains an invalid row at line " +
                        $"{dataEndIndex + 1}.");
                }

                var version = cells[versionColumnIndex];
                var keyMatch = KeyCellPattern().Match(cells[keyColumnIndex]);

                if (version.Length == 0 || !keyMatch.Success)
                {
                    throw new InvalidDataException(
                        $"The {heading.Groups["name"].Value} table contains an invalid row at line " +
                        $"{dataEndIndex + 1}.");
                }

                if (!versions.Add(version))
                {
                    throw new InvalidDataException(
                        $"The {heading.Groups["name"].Value} table contains version {version} more than once.");
                }

                entries.Add(new ExistingKeyEntry(
                    version,
                    keyMatch.Groups["key"].Value,
                    dataEndIndex,
                    cells));
                dataEndIndex++;
            }

            sections.Add(new KeysSection(
                heading.Groups["name"].Value,
                appId,
                headerIndex,
                separatorIndex,
                dataStartIndex,
                dataEndIndex,
                headers,
                separators,
                versionColumnIndex,
                keyColumnIndex,
                entries));
        }

        if (sections.Count == 0)
            throw new InvalidDataException("KEYS.md does not contain any decrypt.day app sections.");

        return new KeysDocument(content, lines, newLine, sections);
    }

    public string Render(IReadOnlyDictionary<string, KeysSectionUpdate> updates)
    {
        ArgumentNullException.ThrowIfNull(updates);

        if (updates.Values.All(update => update.NewKeys.Count == 0))
            return content;

        var insertBefore = new Dictionary<int, List<GeneratedKeyEntry>>();
        var insertAfter = new Dictionary<int, List<GeneratedKeyEntry>>();
        var renderedSections = new Dictionary<string, (KeysSection Section, int[] ColumnWidths)>(
            StringComparer.Ordinal);

        foreach (var section in Sections)
        {
            if (!updates.TryGetValue(section.AppStoreId, out var update) || update.NewKeys.Count == 0)
                continue;

            var generatedCells = update.NewKeys
                .Select(key => CreateGeneratedCells(section, key))
                .ToArray();
            var columnWidths = Enumerable.Range(0, section.Headers.Count)
                .Select(columnIndex =>
                {
                    var existingWidth = section.Entries.Count == 0
                        ? 0
                        : section.Entries.Max(entry => entry.Cells[columnIndex].Length);
                    var generatedWidth = generatedCells.Max(cells => cells[columnIndex].Length);

                    return Math.Max(
                        GetMinimumSeparatorWidth(section.Separators[columnIndex]),
                        Math.Max(
                            section.Headers[columnIndex].Length,
                            Math.Max(existingWidth, generatedWidth)));
                })
                .ToArray();

            renderedSections[section.AppStoreId] = (section, columnWidths);

            var sourceIndexes = new Dictionary<string, int>(StringComparer.Ordinal);

            for (var index = 0; index < update.SourceVersions.Count; index++)
                sourceIndexes.TryAdd(update.SourceVersions[index], index);
            var indexedExisting = section.Entries
                .Where(entry => sourceIndexes.ContainsKey(entry.Version))
                .Select(entry => (Entry: entry, SourceIndex: sourceIndexes[entry.Version]))
                .ToArray();

            foreach (var key in update.NewKeys.OrderBy(key => key.SourceIndex))
            {
                var next = indexedExisting
                    .Where(item => item.SourceIndex > key.SourceIndex)
                    .OrderBy(item => item.SourceIndex)
                    .FirstOrDefault();

                if (next.Entry is not null)
                {
                    AddInsertion(insertBefore, next.Entry.LineIndex, key);
                    continue;
                }

                var previous = indexedExisting
                    .Where(item => item.SourceIndex < key.SourceIndex)
                    .OrderByDescending(item => item.SourceIndex)
                    .FirstOrDefault();

                if (previous.Entry is not null)
                {
                    AddInsertion(insertAfter, previous.Entry.LineIndex, key);
                    continue;
                }

                AddInsertion(insertBefore, section.DataStartIndex, key);
            }
        }

        var result = new List<string>(lines.Length + updates.Values.Sum(update => update.NewKeys.Count));

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
            int lineIndex)
        {
            if (!insertions.TryGetValue(lineIndex, out var values))
                return;

            foreach (var value in values.OrderBy(value => value.SourceIndex))
            {
                var renderedSection = renderedSections[value.AppStoreId];
                output.Add(FormatTableRow(
                    CreateGeneratedCells(renderedSection.Section, value),
                    renderedSection.ColumnWidths));
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
                    var cells = section.Separators
                        .Select((separator, columnIndex) =>
                            FormatSeparatorCell(separator, renderedSection.ColumnWidths[columnIndex]))
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

    private static string[] CreateGeneratedCells(KeysSection section, GeneratedKeyEntry key)
    {
        var cells = Enumerable.Repeat(string.Empty, section.Headers.Count).ToArray();
        cells[section.VersionColumnIndex] = key.Version;
        cells[section.KeyColumnIndex] = $"`{key.Key}`";
        return cells;
    }

    private static string FormatTableRow(IReadOnlyList<string> cells, IReadOnlyList<int> columnWidths)
    {
        return "| " + string.Join(
            " | ",
            cells.Select((cell, index) => cell.PadRight(columnWidths[index]))) + " |";
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

        return (left ? ":" : string.Empty) +
               new string('-', width - colonCount) +
               (right ? ":" : string.Empty);
    }

    private static string[]? ParseTableCells(string line)
    {
        var trimmed = line.Trim();

        if (trimmed.Length < 2 || trimmed[0] != '|' || trimmed[^1] != '|')
            return null;

        return trimmed[1..^1]
            .Split('|', StringSplitOptions.None)
            .Select(cell => cell.Trim())
            .ToArray();
    }

    private static void AddInsertion(
        IDictionary<int, List<GeneratedKeyEntry>> insertions,
        int lineIndex,
        GeneratedKeyEntry key)
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

    [GeneratedRegex(@"^## \[(?<name>[^\]]+)\]\(https://decrypt\.day/app/id(?<id>\d+)\)\s*$", RegexOptions.CultureInvariant)]
    private static partial Regex HeadingPattern();

    [GeneratedRegex(@"^:?-{3,}:?$", RegexOptions.CultureInvariant)]
    private static partial Regex TableSeparatorCellPattern();

    [GeneratedRegex(@"^`(?<key>[0-9A-Fa-f]{64})`$", RegexOptions.CultureInvariant)]
    private static partial Regex KeyCellPattern();
}

internal sealed record KeysSection(
    string Name,
    string AppStoreId,
    int HeaderIndex,
    int SeparatorIndex,
    int DataStartIndex,
    int DataEndIndex,
    IReadOnlyList<string> Headers,
    IReadOnlyList<string> Separators,
    int VersionColumnIndex,
    int KeyColumnIndex,
    IReadOnlyList<ExistingKeyEntry> Entries);

internal sealed record ExistingKeyEntry(
    string Version,
    string Key,
    int LineIndex,
    IReadOnlyList<string> Cells);

internal sealed record GeneratedKeyEntry(
    string AppStoreId,
    string Version,
    string Key,
    int SourceIndex);

internal sealed record KeysSectionUpdate(
    IReadOnlyList<string> SourceVersions,
    IReadOnlyList<GeneratedKeyEntry> NewKeys);
