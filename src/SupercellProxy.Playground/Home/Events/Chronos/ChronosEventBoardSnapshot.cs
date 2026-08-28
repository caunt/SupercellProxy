namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventBoardSnapshot</c> home data.
/// </summary>
internal sealed record ChronosEventBoardSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Events</c> value.
    /// </summary>
    public ChronosEventSnapshot[] Events { get; init; } = [];
}
