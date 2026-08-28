namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">NeighborhoodObjectTaskSnapshot</c> home data.
/// </summary>
internal sealed record NeighborhoodObjectTaskSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">TaskDataId</c> value.
    /// </summary>
    public int TaskDataId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PlayerLevelAtTaskStart</c> value.
    /// </summary>
    public int PlayerLevelAtTaskStart { get; init; }
}
