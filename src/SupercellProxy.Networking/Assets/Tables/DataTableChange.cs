namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Defines the Data Table Change contract.
/// </summary>
public sealed record DataTableChange
{

    /// <summary>
    /// Gets the Column value.
    /// </summary>
    public string? Column { get; init; }
    /// <summary>
    /// Gets the File value.
    /// </summary>
    public string File { get; init; } = string.Empty;

    /// <summary>
    /// Gets the New Full CSV value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("NewFullCSV")]
    public DataTableReplacement? NewFullCommaSeparatedValues { get; init; }

    /// <summary>
    /// Gets the New Value value.
    /// </summary>
    public TableCellValue NewValue { get; init; }

    /// <summary>
    /// Gets the Row value.
    /// </summary>
    public string? Row { get; init; }
}
