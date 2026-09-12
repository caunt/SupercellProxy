namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Represents <c language="csharp">DataTableReference</c>.
/// </summary>
public sealed record DataTableReference(
    [property: System.Text.Json.Serialization.JsonPropertyName("GlobalId")] int GlobalIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("TableId")] int TableIdentifier,
    int RowIndex,
    string Name,
    string File,
    string FileSha
);
