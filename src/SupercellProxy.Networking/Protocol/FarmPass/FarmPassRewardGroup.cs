using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Defines the Farm Pass Reward Group contract.
/// </summary>
public sealed record FarmPassRewardGroup
{

    /// <summary>
    /// Gets the Collected value.
    /// </summary>
    [JsonPropertyName("rewardCollected")]
    public bool Collected { get; set; }
    /// <summary>
    /// Gets the Rewards value.
    /// </summary>
    [JsonPropertyName("rewards")]
    public FarmPassRewardItem[] Rewards { get; init; } = [];
}
