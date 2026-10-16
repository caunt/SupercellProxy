namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// CSV text a table change supplies in place of existing data, either a whole file or the source of replaced columns.
/// </summary>
public sealed record DataTableReplacement
{
    /// <summary>
    /// The CSV text, including the column-name and column-type header rows.
    /// </summary>
    public string Content { get; init; } = string.Empty;
}
