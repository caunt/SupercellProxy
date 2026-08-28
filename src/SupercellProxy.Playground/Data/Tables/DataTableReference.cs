namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c language="csharp">DataTableReference</c>.
/// </summary>
internal sealed record DataTableReference(
    int GlobalId,
    int TableId,
    int RowIndex,
    string Name,
    string File,
    string FileSha
);
