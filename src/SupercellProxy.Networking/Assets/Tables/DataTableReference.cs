namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Represents <c language="csharp">DataTableReference</c>.
/// </summary>
public sealed record DataTableReference(
    [property: System.Text.Json.Serialization.JsonPropertyName("GlobalId")] int GlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("TableId")] int TableId,
    int RowIndex,
    string Name,
    string File,
    string FileSha
);
