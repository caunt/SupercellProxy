namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c language="csharp">GameDataTable</c>.
/// </summary>
internal sealed record GameDataTable(
    IReadOnlyList<string> Headers,
    IReadOnlyList<string> Types,
    IReadOnlyList<GameDataTableEntry> Entries
);
