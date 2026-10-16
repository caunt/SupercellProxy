using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventsSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventsSnapshot
{
    /// <summary>Gets retained player-specific event selections and state.</summary>
    [JsonPropertyName("av_datas")]
    [JsonConverter(typeof(ChronosEventPlayerDataConverter))]
    public ChronosEventPlayerData[] PlayerData { get; init; } = [];

    /// <summary>
    /// Scheduled (non-targeted) events the player has already looked at, so they are not announced again.
    /// </summary>
    [JsonPropertyName("seen_active_event_ids")]
    public int[] SeenActiveEventIds { get; init; } = [];

    /// <summary>
    /// Targeted events the player has looked at, one entry per event and impression.
    /// </summary>
    [JsonPropertyName("seen_events")]
    public SeenEventSnapshot[] SeenEvents { get; init; } = [];

    /// <summary>
    /// The player's event board and the events on it.
    /// </summary>
    [JsonPropertyName("event_board_state")]
    public ChronosEventBoardSnapshot EventBoardState { get; init; } = new();
}
