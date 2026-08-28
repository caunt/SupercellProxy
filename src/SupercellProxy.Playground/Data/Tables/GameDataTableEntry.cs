namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c language="csharp">GameDataTableEntry</c>.
/// </summary>
internal sealed record GameDataTableEntry(
    string Name,
    IReadOnlyDictionary<string, object?> BaseRow,
    IReadOnlyList<IReadOnlyDictionary<string, object?>> ContinuationRows,
    IReadOnlyList<IReadOnlyDictionary<string, object?>> Snapshots
);
