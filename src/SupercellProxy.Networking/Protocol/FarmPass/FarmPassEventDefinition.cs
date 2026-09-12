using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Represents the decoded FarmPassEventDefinition JSON contract.</summary>
public sealed record FarmPassEventDefinition
{
    /// <summary>Gets the BabyPet value.</summary>
    [JsonPropertyName("farmPassBabyPet")]
    public string? BabyPet { get; init; }

    /// <summary>Gets the DiamondSinks value.</summary>
    [JsonPropertyName("diamondSinksConfig")]
    public FarmPassDiamondSinks? DiamondSinks { get; init; }

    /// <summary>Gets the RewardTiers value.</summary>
    [JsonPropertyName("rewardTiers")]
    public FarmPassRewardTierDefinition[] RewardTiers { get; init; } = [];

    /// <summary>Gets the Road value.</summary>
    [JsonPropertyName("farmPassRoad")]
    public string? Road { get; init; }

    /// <summary>Gets the Thresholds value.</summary>
    [JsonPropertyName("farmPassThresholds")]
    public int[] Thresholds { get; init; } = [];

}
