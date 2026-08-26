namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>MapGameSnapshot</c> home data.
/// </summary>
public sealed record MapGameSnapshot
{
    /// <summary>
    /// Gets or sets the <c>Event</c> value.
    /// </summary>
    public int Event { get; init; }

    /// <summary>
    /// Gets or sets the <c>MapGlobalId</c> value.
    /// </summary>
    public int MapGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c>QuestrManager</c> value.
    /// </summary>
    public MapGameQuestSnapshot QuestrManager { get; init; } = new();
}
