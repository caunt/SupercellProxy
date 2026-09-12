using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Represents the decoded FarmPassRewardDefinition JSON contract.</summary>
public sealed record FarmPassRewardDefinition
{
    /// <summary>Gets the Amount value.</summary>
    [JsonPropertyName("rewardAmount")]
    public int Amount { get; init; }

    /// <summary>Gets the FallbackAmount value.</summary>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackAmount { get; init; }

    /// <summary>Gets the FallbackReward value.</summary>
    [JsonPropertyName("fallbackReward")]
    public string? FallbackReward { get; init; }

    /// <summary>Gets the Reward value.</summary>
    [JsonPropertyName("reward")]
    public string? Reward { get; init; }

}
