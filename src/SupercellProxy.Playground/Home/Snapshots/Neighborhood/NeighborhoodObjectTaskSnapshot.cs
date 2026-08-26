namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>NeighborhoodObjectTaskSnapshot</c> home data.
/// </summary>
public sealed record NeighborhoodObjectTaskSnapshot
{
    /// <summary>
    /// Gets or sets the <c>TaskDataId</c> value.
    /// </summary>
    public int TaskDataId { get; init; }

    /// <summary>
    /// Gets or sets the <c>PlayerLevelAtTaskStart</c> value.
    /// </summary>
    public int PlayerLevelAtTaskStart { get; init; }
}
