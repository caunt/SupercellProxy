namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>ChronosEventBoardSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventBoardSnapshot
{
    /// <summary>
    /// Gets or sets the <c>Events</c> value.
    /// </summary>
    public ChronosEventSnapshot[] Events { get; init; } = [];
}
