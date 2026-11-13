using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Boats.Snapshots;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets or sets a value indicating whether the boat offers three destinations (saved as <c>boat_destinations_feature_enabled</c>).</summary>
    /// <value><see langword="true"/> if the docked boat lets the player choose among destination orders; otherwise, <see langword="false"/>.</value>
    /// <remarks><see langword="false"/> means the older single-order boat.</remarks>
    [JsonPropertyName("boat_destinations_feature_enabled")]
    public bool BoatDestinationsEnabled { get; init; }

    /// <summary>Gets or sets a value indicating whether the next boat's preview was opened (saved as <c>boat_preview_seen</c>).</summary>
    /// <value><see langword="true"/> if the player looked at the announced next boat, which clears its "new" marker; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("boat_preview_seen")]
    public bool BoatPreviewSeen { get; init; }

    /// <summary>Gets or sets a value indicating whether the current cargo deadline has already been extended (saved as <c>time_extended</c>).</summary>
    /// <value><see langword="true"/> if the player bought the time extension for the docked boat; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("time_extended")]
    public bool BoatTimeExtended { get; init; }

    /// <summary>Gets or sets a value indicating whether the visited owner's boat supports an extension offer (saved as <c>time_extension_feature_enabled</c>).</summary>
    /// <value><see langword="true"/> if an expired boat offers extra time for diamonds; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("time_extension_feature_enabled")]
    public bool BoatTimeExtensionEnabled { get; init; }

    /// <summary>Gets or sets the number of completed orders for each boat difficulty.</summary>
    /// <value>One count per difficulty id for the current day; empty by default.</value>
    public ImmutableArray<BoatDifficultyCompletionSnapshot> CompletionsPerDifficulty { get; init; } = [];

    /// <summary>Gets or sets a value indicating whether orders need generation on the next normal update (saved as <c>create_order_next_tick</c>).</summary>
    /// <value><see langword="true"/> if the boat's next update builds new orders; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("create_order_next_tick")]
    public bool CreateBoatOrderNextTick { get; init; }

    /// <summary>Gets or sets the boat's daily completed-order count.</summary>
    /// <value>The number of boat orders completed since <see cref="LastDailyResetTime"/>.</value>
    public int DailyCompletions { get; init; }

    /// <summary>Gets or sets the return travel duration in simulation updates.</summary>
    /// <value>The away time at 1800 updates per minute.</value>
    public int Eta { get; init; }

    /// <summary>Gets or sets the boat's last daily reset time.</summary>
    /// <value>A Unix timestamp in seconds of the boat's last daily boundary, at the configured reset hour (UTC); 0 when never reset.</value>
    public int LastDailyResetTime { get; init; }

    /// <summary>Gets or sets the event whose arrival requires refreshing the current order bonuses.</summary>
    /// <value>The live event id saved as <c>pending_boat_order_bonus_refresh_event_id</c>; 0 when no refresh is pending.</value>
    [JsonPropertyName("pending_boat_order_bonus_refresh_event_id")]
    public int PendingBoatBonusRefreshEventId { get; init; }

    /// <summary>Gets or sets the current boat's sequence number.</summary>
    /// <value>The running boat number saved as <c>running_boat_id</c>; it grows by one per boat.</value>
    [JsonPropertyName("running_boat_id")]
    public int RunningBoatId { get; init; }

    /// <summary>Gets or sets the last generated order's sequence number.</summary>
    /// <value>The running order number saved as <c>running_order_id</c>.</value>
    [JsonPropertyName("running_order_id")]
    public int RunningOrderId { get; init; }
}
