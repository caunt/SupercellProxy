namespace SupercellProxy.Keys;

internal sealed record KeysSectionUpdate(
    IReadOnlyList<string> SourceVersions,
    IReadOnlyList<GeneratedKeyEntry> NewKeys
);
