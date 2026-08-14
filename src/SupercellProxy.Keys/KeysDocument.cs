using System.Text.RegularExpressions;

namespace SupercellProxy.Keys;

internal sealed partial class KeysDocument
{
    private const int KeyWidth = 66;

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

            if (headerIndex < 0 || !TableHeaderPattern().IsMatch(lines[headerIndex]))
            {
                throw new InvalidDataException(
                    $"The {heading.Groups["name"].Value} section does not contain a Version/Key table.");
            }

            var separatorIndex = NextNonEmptyLine(lines, headerIndex + 1);

            if (separatorIndex < 0 || !TableSeparatorPattern().IsMatch(lines[separatorIndex]))
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
                var row = TableRowPattern().Match(lines[dataEndIndex]);

                if (!row.Success)
                {
                    throw new InvalidDataException(
                        $"The {heading.Groups["name"].Value} table contains an invalid row at line " +
                        $"{dataEndIndex + 1}.");
                }

                var version = row.Groups["version"].Value.Trim();

                if (!versions.Add(version))
                {
                    throw new InvalidDataException(
                        $"The {heading.Groups["name"].Value} table contains version {version} more than once.");
                }

                entries.Add(new ExistingKeyEntry(
                    version,
                    row.Groups["key"].Value,
                    dataEndIndex));
                dataEndIndex++;
            }

            var versionWidth = "Version".Length;

            if (entries.Count > 0)
                versionWidth = Math.Max(versionWidth, entries.Max(entry => entry.Version.Length));

            sections.Add(new KeysSection(
                heading.Groups["name"].Value,
                appId,
                headerIndex,
                separatorIndex,
                dataStartIndex,
                dataEndIndex,
                versionWidth,
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
        var renderedSections = new Dictionary<string, (KeysSection Section, int VersionWidth)>(
            StringComparer.Ordinal);

        foreach (var section in Sections)
        {
            if (!updates.TryGetValue(section.AppStoreId, out var update) || update.NewKeys.Count == 0)
                continue;

            renderedSections[section.AppStoreId] = (
                section,
                Math.Max(section.VersionWidth, update.NewKeys.Max(key => key.Version.Length)));

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
                output.Add(FormatDataRow(value.Version, value.Key, renderedSection.VersionWidth));
            }
        }

        string FormatExistingLine(int lineIndex)
        {
            foreach (var renderedSection in renderedSections.Values)
            {
                var section = renderedSection.Section;

                if (lineIndex == section.HeaderIndex)
                {
                    return $"| {"Version".PadRight(renderedSection.VersionWidth)} " +
                           $"| {"Key".PadRight(KeyWidth)} |";
                }

                if (lineIndex == section.SeparatorIndex)
                {
                    return $"| {new string('-', renderedSection.VersionWidth)} " +
                           $"| {new string('-', KeyWidth)} |";
                }

                var entry = section.Entries.FirstOrDefault(entry => entry.LineIndex == lineIndex);

                if (entry is not null)
                    return FormatDataRow(entry.Version, entry.Key, renderedSection.VersionWidth);
            }

            return lines[lineIndex];
        }
    }

    private static string FormatDataRow(string version, string key, int versionWidth)
    {
        return $"| {version.PadRight(versionWidth)} | `{key}` |";
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

    [GeneratedRegex(@"^\|\s*Version\s*\|\s*Key\s*\|\s*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex TableHeaderPattern();

    [GeneratedRegex(@"^\|\s*:?-{3,}:?\s*\|\s*:?-{3,}:?\s*\|\s*$", RegexOptions.CultureInvariant)]
    private static partial Regex TableSeparatorPattern();

    [GeneratedRegex(@"^\|\s*(?<version>[^|]+?)\s*\|\s*`(?<key>[0-9A-Fa-f]{64})`\s*\|\s*$", RegexOptions.CultureInvariant)]
    private static partial Regex TableRowPattern();

}

internal sealed record KeysSection(
    string Name,
    string AppStoreId,
    int HeaderIndex,
    int SeparatorIndex,
    int DataStartIndex,
    int DataEndIndex,
    int VersionWidth,
    IReadOnlyList<ExistingKeyEntry> Entries);

internal sealed record ExistingKeyEntry(string Version, string Key, int LineIndex);

internal sealed record GeneratedKeyEntry(
    string AppStoreId,
    string Version,
    string Key,
    int SourceIndex);

internal sealed record KeysSectionUpdate(
    IReadOnlyList<string> SourceVersions,
    IReadOnlyList<GeneratedKeyEntry> NewKeys);
