namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c>GameDataTable</c>.
/// </summary>
public sealed record GameDataTable(
    IReadOnlyList<string> Headers,
    IReadOnlyList<string> Types,
    IReadOnlyList<GameDataTableEntry> Entries
);
