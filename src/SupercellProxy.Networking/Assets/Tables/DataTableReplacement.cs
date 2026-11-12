namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// CSV text a table change supplies in place of existing data, either a whole file or the source of replaced columns.
/// </summary>
public sealed record DataTableReplacement
{
    /// <summary>
    /// Gets or sets the CSV text, including the column-name and column-type header rows.
    /// </summary>
    /// <value>The complete CSV as one string, empty by default; it is stored as UTF-8 when it replaces a whole file, and a column copy
    /// is refused when it has fewer than the two header rows.</value>
    public string Content { get; init; } = string.Empty;
}
