namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// One entry of a game table: its named line and the unnamed continuation lines under it that hold further elements of array columns.
/// </summary>
/// <param name="Name">Entry name from the first column of its named line.</param>
/// <param name="BaseRow">Cells filled on the named line; empty cells are absent.</param>
/// <param name="ContinuationRows">Cells filled on each continuation line, in order; empty cells are absent.</param>
/// <param name="Snapshots">
/// One full set of column values per line, starting with the named line, where cells a line leaves empty keep the value from the
/// entry's earlier lines.
/// </param>
public sealed record GameDataTableEntry(
    string Name,
    IReadOnlyDictionary<string, LiteralValue> BaseRow,
    IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>> ContinuationRows,
    IReadOnlyList<IReadOnlyDictionary<string, LiteralValue>> Snapshots
);
