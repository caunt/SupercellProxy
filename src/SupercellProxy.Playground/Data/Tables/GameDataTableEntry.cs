namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c>GameDataTableEntry</c>.
/// </summary>
public sealed record GameDataTableEntry(
    string Name,
    IReadOnlyDictionary<string, object?> BaseRow,
    IReadOnlyList<IReadOnlyDictionary<string, object?>> ContinuationRows,
    IReadOnlyList<IReadOnlyDictionary<string, object?>> Snapshots
);
