using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventBoardSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventBoardSnapshot
{
    /// <summary>
    /// The events currently on the player's event board, each with its schedule and the player's progress in it.
    /// </summary>
    public ChronosEventSnapshot[] Events { get; init; } = [];

    /// <summary>Gets the native integer flags indexed by event type; nonzero means liked.</summary>
    [JsonPropertyName("likedEventTypes")]
    public int[] LikedEventTypes { get; init; } = [];
}
