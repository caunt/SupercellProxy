using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// One reward a task event's document defines, with the fallback used when the reward cannot be received.
/// </summary>
public sealed record TaskEventRewardDefinition
{
    /// <summary>
    /// Name of the fallback reward's table row; null when the reward has none.
    /// </summary>
    [JsonPropertyName("fallbackReward")]
    public string? FallbackReward { get; init; }

    /// <summary>
    /// How many of the fallback reward the player gets in its place.
    /// </summary>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackRewardAmount { get; init; }

    /// <summary>
    /// Event points the reward needs.
    /// </summary>
    [JsonPropertyName("points")]
    public int Points { get; init; }

    /// <summary>
    /// Name of the reward's table row.
    /// </summary>
    [JsonPropertyName("reward")]
    public string Reward { get; init; } = string.Empty;

    /// <summary>
    /// How many of the reward the player gets.
    /// </summary>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }
}
