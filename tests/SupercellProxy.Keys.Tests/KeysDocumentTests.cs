namespace SupercellProxy.Keys.Tests;

public sealed class KeysDocumentTests
{
    private const string FirstKey =
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
    private const string SecondKey =
        "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB";

    [Fact]
    public void RenderNormalizesMergesAndSortsOnlyTheSelectedSection()
    {
        var source = CreateMultiSectionDocument();
        var document = KeysDocument.Parse(source);
        var updates = new Dictionary<string, KeysSectionUpdate>(StringComparer.Ordinal)
        {
            ["1053012308"] = new KeysSectionUpdate([new GeneratedKeyEntry("15.535.29", FirstKey)]),
        };

        var rendered = document.Render(updates);
        var reparsed = KeysDocument.Parse(rendered);
        var versions = reparsed.Sections[0].Entries.Select(static entry => entry.Version);

        Assert.Equal(
            ["16.402.2", "15.535.29", "15.535.22", "15.535.3"],
            versions,
            StringComparer.Ordinal
        );
        Assert.DoesNotContain("| v15.", rendered, StringComparison.Ordinal);
        Assert.Contains("| Version   |", rendered, StringComparison.Ordinal);
        Assert.Contains("| v2.0    |", rendered, StringComparison.Ordinal);
    }

    [Fact]
    public void RenderMaintainsASectionWithoutAddingKeysAndIsIdempotent()
    {
        var source = CreateDocument(
            ("15.535.3", FirstKey),
            ("16.402.2", SecondKey),
            ("v15.535.22", FirstKey)
        );
        var updates = CreateEmptyUpdate();

        var rendered = KeysDocument.Parse(source).Render(updates);
        var renderedAgain = KeysDocument.Parse(rendered).Render(updates);

        Assert.Equal(rendered, renderedAgain);
        Assert.Equal(
            ["16.402.2", "15.535.22", "15.535.3"],
            KeysDocument.Parse(rendered).Sections[0].Entries.Select(static entry => entry.Version),
            StringComparer.Ordinal
        );
    }

    [Fact]
    public void ParseRejectsConflictingKeysForEquivalentVersions()
    {
        var source = CreateDocument(("15.535.3", FirstKey), ("v15.535.3", SecondKey));

        var exception = Assert.Throws<InvalidDataException>(() => KeysDocument.Parse(source));

        Assert.Contains(
            "conflicting keys for version 15.535.3",
            exception.Message,
            StringComparison.Ordinal
        );
    }

    private static Dictionary<string, KeysSectionUpdate> CreateEmptyUpdate()
    {
        return new Dictionary<string, KeysSectionUpdate>(StringComparer.Ordinal)
        {
            ["1053012308"] = new KeysSectionUpdate([]),
        };
    }

    private static string CreateDocument(params (string Version, string Key)[] entries)
    {
        var rows = string.Join(
            '\n',
            entries.Select(static entry => $"| {entry.Version} | `{entry.Key}` |")
        );
        return string.Join(
            '\n',
            [
                "## [Clash Royale](https://decrypt.day/app/id1053012308)",
                "| Version | Key |",
                "| ------- | --- |",
                rows,
                string.Empty,
            ]
        );
    }

    private static string CreateMultiSectionDocument()
    {
        return string.Join(
            '\n',
            [
                "# Keys",
                string.Empty,
                "## [Clash Royale](https://decrypt.day/app/id1053012308)",
                "| Version    | Key                                                                  |",
                "| ---------- | -------------------------------------------------------------------- |",
                $"| 15.535.3   | `{FirstKey}` |",
                $"| 16.402.2   | `{SecondKey}` |",
                $"| v15.535.22 | `{FirstKey}` |",
                $"| v15.535.3  | `{FirstKey}` |",
                string.Empty,
                "## [Other](https://decrypt.day/app/id1234567890)",
                "| Version | Key                                                                  |",
                "| ------- | -------------------------------------------------------------------- |",
                $"| v2.0    | `{SecondKey}` |",
                $"| 10.0    | `{FirstKey}` |",
                string.Empty,
            ]
        );
    }
}
