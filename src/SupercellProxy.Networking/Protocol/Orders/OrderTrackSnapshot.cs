using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Defines the Order Track Snapshot contract.
/// </summary>
public sealed record OrderTrackSnapshot
{
    /// <summary>Gets the saved account-specific calendar offset in seconds.</summary>
    [JsonPropertyName("calOff")]
    public int CalendarOffsetSeconds { get; init; } = -1;

    /// <summary>
    /// Gets the Completed value.
    /// </summary>
    [JsonPropertyName("completed")]
    public int Completed { get; init; }
    /// <summary>Gets whether the player has seen this boat-track cycle's introduction.</summary>
    [JsonPropertyName("cycleIntroSeen")]
    public bool CycleIntroSeen { get; init; }

    /// <summary>
    /// Gets the Index value.
    /// </summary>
    [JsonPropertyName("idx")]
    public int Index { get; init; }

    /// <summary>
    /// Gets the Pending Completion Points value.
    /// </summary>
    [JsonPropertyName("pendingCompletionPoints")]
    public int PendingCompletionPoints { get; init; }

    /// <summary>Gets the boat order associated with pending completion points.</summary>
    [JsonPropertyName("pendingCompletionPointsOrderId")]
    public int PendingCompletionPointsOrderId { get; init; } = -1;

    /// <summary>Gets the boat associated with pending completion points.</summary>
    [JsonPropertyName("pendingCompletionPointsBoatId")]
    public int PendingCompletionPointsBoatId { get; init; } = -1;

    /// <summary>
    /// Gets the Reset Timestamp value.
    /// </summary>
    [JsonPropertyName("reset")]
    public int ResetTimestamp { get; init; }

    /// <summary>
    /// Gets whether the current milestone's rewards have already been granted.
    /// </summary>
    [JsonPropertyName("claimed")]
    public bool RewardClaimed { get; init; }

    /// <summary>
    /// Gets the Rewards value.
    /// </summary>
    [JsonPropertyName("rewards")]
    public OrderTrackReward[][] Rewards { get; init; } = [];

    /// <summary>Gets milestone rewards awaiting delivery after the previous track expired.</summary>
    [JsonPropertyName("savedUnclaimedRewards")]
    public OrderTrackReward[][] SavedUnclaimedRewards { get; init; } = [];

    /// <summary>
    /// Gets the Target value.
    /// </summary>
    [JsonPropertyName("target")]
    public int Target { get; init; }

    /// <summary>
    /// Gets the Track Id value.
    /// </summary>
    [JsonPropertyName("track")]
    public int TrackId { get; init; }
}
