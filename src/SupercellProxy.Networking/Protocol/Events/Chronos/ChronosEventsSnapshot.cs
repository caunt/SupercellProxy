using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventsSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventsSnapshot
{
    /// <summary>
    /// Gets the Seen Active Event Ids value.
    /// </summary>
    [JsonPropertyName("seen_active_event_ids")]
    public int[] SeenActiveEventIdentifiers { get; init; } = [];

    /// <summary>
    /// Gets the Seen Events value.
    /// </summary>
    [JsonPropertyName("seen_events")]
    public SeenEventSnapshot[] SeenEvents { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">EventBoardState</c> value.
    /// </summary>
    [JsonPropertyName("event_board_state")]
    public ChronosEventBoardSnapshot EventBoardState { get; init; } = new();
}
