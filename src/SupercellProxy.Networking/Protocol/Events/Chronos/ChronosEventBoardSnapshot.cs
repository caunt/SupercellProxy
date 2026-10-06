namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventBoardSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventBoardSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Events</c> value.
    /// </summary>
    public ChronosEventSnapshot[] Events { get; init; } = [];

    /// <summary>Gets the native integer flags indexed by event type; nonzero means liked.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("likedEventTypes")]
    public int[] LikedEventTypes { get; init; } = [];
}
