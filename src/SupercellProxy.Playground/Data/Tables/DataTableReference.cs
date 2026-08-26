namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c>DataTableReference</c>.
/// </summary>
public sealed record DataTableReference(
    int GlobalId,
    int TableId,
    int RowIndex,
    string Name,
    string File,
    string FileSha
);
