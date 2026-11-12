using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>A set of baby pet rewards of a Farm Pass level, collected together.</summary>
public sealed record FarmPassBabyPetRewardGroup : ExtensibleDocument
{
    /// <summary>Gets or sets whether the player already collected the set; <see langword="null"/> when not saved yet.</summary>
    /// <value><see langword="true"/> if the baby pet claim command already took this set; otherwise, <see langword="false"/>, or <see langword="null"/> when the save does not carry the flag.</value>
    [JsonPropertyName("rewardCollected")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Collected { get; set; }

    /// <summary>Gets or sets the baby pet rewards in the set.</summary>
    /// <value>The baby pets of the set, each with its count; empty when the set holds none.</value>
    [JsonPropertyName("rewards")]
    public FarmPassBabyPetReward[] Rewards { get; init; } = [];
}
