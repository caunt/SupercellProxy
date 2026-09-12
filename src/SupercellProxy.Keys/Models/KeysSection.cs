namespace SupercellProxy.Keys.Models;

internal sealed record KeysSection(
    string Name,
    [property: System.Text.Json.Serialization.JsonPropertyName("AppStoreId")] string AppStoreIdentifier,
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
