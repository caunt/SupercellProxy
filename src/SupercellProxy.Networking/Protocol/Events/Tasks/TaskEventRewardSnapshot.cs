using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// One saved reward of a task event's points track, with the fallback the event offers when the reward cannot be received.
/// </summary>
public sealed record TaskEventRewardSnapshot
{
    /// <summary>
    /// Whether the reward was collected.
    /// </summary>
    [JsonPropertyName("claimed")]
    public bool Claimed { get; init; }

    /// <summary>
    /// Whether the fallback took the reward's place.
    /// </summary>
    [JsonPropertyName("claimedFallback")]
    public bool ClaimedFallback { get; init; }

    /// <summary>
    /// How many of the fallback reward the player gets in its place.
    /// </summary>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackRewardAmount { get; init; }

    /// <summary>
    /// Global id of the fallback reward; 0 when the event has none.
    /// </summary>
    [JsonPropertyName("fallbackRewardId")]
    public int FallbackRewardId { get; init; }

    /// <summary>
    /// Event points the reward needs.
    /// </summary>
    [JsonPropertyName("points")]
    public int Points { get; init; }

    /// <summary>
    /// How many of the reward the player gets.
    /// </summary>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }

    /// <summary>
    /// Global id of the reward.
    /// </summary>
    [JsonPropertyName("rewardId")]
    public int RewardId { get; init; }
}
