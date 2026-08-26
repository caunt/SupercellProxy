namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>NeighborhoodObjectManagerSnapshot</c> home data.
/// </summary>
public sealed record NeighborhoodObjectManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c>ActiveEventId</c> value.
    /// </summary>
    public int ActiveEventId { get; init; }

    /// <summary>
    /// Gets or sets the <c>State</c> value.
    /// </summary>
    public NeighborhoodObjectStateSnapshot State { get; init; } = new();
}
