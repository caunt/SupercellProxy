namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Defines the Data Table Replacement contract.
/// </summary>
public sealed record DataTableReplacement
{
    /// <summary>
    /// Gets the Content value.
    /// </summary>
    public string Content { get; init; } = string.Empty;
}
