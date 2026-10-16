using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>A set of Farm Pass level rewards collected together, with its collected flag.</summary>
public sealed record FarmPassRewardGroup
{
    /// <summary>Whether the player already collected the set.</summary>
    [JsonPropertyName("rewardCollected")]
    public bool Collected { get; set; }

    /// <summary>What the set gives, each resolved to its table entry and amount.</summary>
    [JsonPropertyName("rewards")]
    public FarmPassRewardItem[] Rewards { get; init; } = [];
}
