using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>A set of baby pet rewards of a Farm Pass level, collected together.</summary>
public sealed record FarmPassBabyPetRewardGroup : ExtensibleDocument
{
    /// <summary>Whether the player already collected the set; null when not saved yet.</summary>
    [JsonPropertyName("rewardCollected")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Collected { get; set; }

    /// <summary>Baby pet rewards in the set.</summary>
    [JsonPropertyName("rewards")]
    public FarmPassBabyPetReward[] Rewards { get; init; } = [];
}
