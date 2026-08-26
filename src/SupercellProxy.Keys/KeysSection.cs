namespace SupercellProxy.Keys;

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
    IReadOnlyList<ExistingKeyEntry> Entries
);
