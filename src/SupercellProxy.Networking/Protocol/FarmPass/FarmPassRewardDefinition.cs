using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>One reward of a Farm Pass level, named by its table entry.</summary>
public sealed record FarmPassRewardDefinition
{
    /// <summary>Gets or sets how many of the reward the level gives.</summary>
    /// <value>The <c>rewardAmount</c> count of the event document.</value>
    [JsonPropertyName("rewardAmount")]
    public int Amount { get; init; }

    /// <summary>Gets or sets how many of the fallback reward are given instead.</summary>
    /// <value>The <c>fallbackRewardAmount</c> count that goes with <see cref="FallbackReward"/>; 0 when the event names no fallback.</value>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackAmount { get; init; }

    /// <summary>Gets or sets the name of the fallback reward given instead of the main one.</summary>
    /// <value>A reward table row name read from <c>fallbackReward</c>, or <see langword="null"/> when the event names no fallback.</value>
    [JsonPropertyName("fallbackReward")]
    public string? FallbackReward { get; init; }

    /// <summary>Gets or sets the name of the rewarded table entry.</summary>
    /// <value>A reward table row name read from <c>reward</c>, or <see langword="null"/> when absent.</value>
    [JsonPropertyName("reward")]
    public string? Reward { get; init; }
}
