using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Boats.Snapshots;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets whether the boat offers three destinations.</summary>
    [JsonPropertyName("boat_destinations_feature_enabled")]
    public bool BoatDestinationsEnabled { get; init; }
    /// <summary>Gets whether the next boat's preview was opened.</summary>
    [JsonPropertyName("boat_preview_seen")]
    public bool BoatPreviewSeen { get; init; }
    /// <summary>Gets the number of completed orders for each boat difficulty.</summary>
    public BoatDifficultyCompletionSnapshot[] CompletionsPerDifficulty { get; init; } = [];
    /// <summary>Gets whether orders need generation on the next normal update.</summary>
    [JsonPropertyName("create_order_next_tick")]
    public bool CreateBoatOrderNextTick { get; init; }
    /// <summary>Gets the boat's daily completed-order count.</summary>
    public int DailyCompletions { get; init; }
    /// <summary>Gets the return travel duration in simulation updates.</summary>
    public int Eta { get; init; }
    /// <summary>Gets the boat's last daily reset time.</summary>
    public int LastDailyResetTime { get; init; }
    /// <summary>Gets the current boat's sequence number.</summary>
    [JsonPropertyName("running_boat_id")]
    public int RunningBoatId { get; init; }
    /// <summary>Gets the last generated order's sequence number.</summary>
    [JsonPropertyName("running_order_id")]
    public int RunningOrderId { get; init; }
}
