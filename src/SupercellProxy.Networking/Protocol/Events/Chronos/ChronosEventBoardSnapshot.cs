using System.Collections.Immutable;
using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventBoardSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventBoardSnapshot
{
    /// <summary>
    /// Gets or sets the events currently on the player's event board, each with its schedule and the player's progress in it.
    /// </summary>
    /// <value>One snapshot per board event in saved order; empty when the board holds no events.</value>
    public ImmutableArray<ChronosEventSnapshot> Events { get; init; } = [];

    /// <summary>Gets or sets the native integer flags indexed by event type; nonzero means liked.</summary>
    /// <value>Element <c>n</c> is 1 when the player liked event type <c>n</c> and 0 otherwise; empty by default.</value>
    [JsonPropertyName("likedEventTypes")]
    public ImmutableArray<int> LikedEventTypes { get; init; } = [];
}
