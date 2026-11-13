using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventsSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventsSnapshot
{
    /// <summary>Gets or sets retained player-specific event selections and state.</summary>
    /// <value>One entry per event that saved its own player document (saved as <c>av_datas</c>, an array alternating event ids and JSON strings); empty when no event stored any.</value>
    [JsonPropertyName("av_datas")]
    [JsonConverter(typeof(ChronosEventPlayerDataConverter))]
    public ImmutableArray<ChronosEventPlayerData> PlayerData { get; init; } = [];

    /// <summary>
    /// Gets or sets the scheduled (non-targeted) events the player has already looked at, so they are not announced again.
    /// </summary>
    /// <value>Event ids in the order they were first seen, each listed once.</value>
    [JsonPropertyName("seen_active_event_ids")]
    public ImmutableArray<int> SeenActiveEventIds { get; init; } = [];

    /// <summary>
    /// Gets or sets the targeted events the player has looked at, one entry per event and impression.
    /// </summary>
    /// <value>Records of targeted (scheduling type 1) events in saved order; empty by default.</value>
    [JsonPropertyName("seen_events")]
    public ImmutableArray<SeenEventSnapshot> SeenEvents { get; init; } = [];

    /// <summary>
    /// Gets or sets the player's event board and the events on it.
    /// </summary>
    /// <value>The board's events and liked event types; an empty board when the save has none.</value>
    [JsonPropertyName("event_board_state")]
    public ChronosEventBoardSnapshot EventBoardState { get; init; } = new();
}
