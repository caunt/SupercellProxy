using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>One reward of a Farm Pass level, named by its table entry.</summary>
public sealed record FarmPassRewardDefinition
{
    /// <summary>How many of the reward the level gives.</summary>
    [JsonPropertyName("rewardAmount")]
    public int Amount { get; init; }

    /// <summary>How many of the fallback reward are given instead.</summary>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackAmount { get; init; }

    /// <summary>Name of the reward given instead when the player cannot get the main one.</summary>
    [JsonPropertyName("fallbackReward")]
    public string? FallbackReward { get; init; }

    /// <summary>Name of the rewarded table entry.</summary>
    [JsonPropertyName("reward")]
    public string? Reward { get; init; }
}
