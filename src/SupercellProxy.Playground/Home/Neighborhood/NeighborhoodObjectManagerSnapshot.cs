namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">NeighborhoodObjectManagerSnapshot</c> home data.
/// </summary>
internal sealed record NeighborhoodObjectManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">ActiveEventId</c> value.
    /// </summary>
    public int ActiveEventId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">State</c> value.
    /// </summary>
    public NeighborhoodObjectStateSnapshot State { get; init; } = new();
}
