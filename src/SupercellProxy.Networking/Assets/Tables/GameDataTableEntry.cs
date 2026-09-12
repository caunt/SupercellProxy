namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Represents <c language="csharp">GameDataTableEntry</c>.
/// </summary>
public sealed record GameDataTableEntry(
    string Name,
    IReadOnlyDictionary<string, LiteralValue> BaseRow,
    IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>> ContinuationRows,
    IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>> Snapshots
);
