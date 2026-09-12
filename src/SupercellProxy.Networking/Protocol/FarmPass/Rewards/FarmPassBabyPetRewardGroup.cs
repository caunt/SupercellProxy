using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>Represents decoded FarmPassBabyPetRewardGroup state.</summary>
public sealed record FarmPassBabyPetRewardGroup : ExtensibleDocument
{
    /// <summary>Gets the Collected value.</summary>
    [JsonPropertyName("rewardCollected")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Collected { get; init; }

    /// <summary>Gets the Rewards value.</summary>
    [JsonPropertyName("rewards")]
    public FarmPassBabyPetReward[] Rewards { get; init; } = [];

}
