using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Represents the decoded FarmPassRewardTierDefinition JSON contract.</summary>
public sealed record FarmPassRewardTierDefinition
{
    /// <summary>Gets the FarmPoints value.</summary>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>Gets the FreeBabyPetRewards value.</summary>
    [JsonPropertyName("freeBabyPetRewards")]
    public FarmPassRewardGroupDefinition[] FreeBabyPetRewards { get; init; } = [];

    /// <summary>Gets the FreeRewards value.</summary>
    [JsonPropertyName("freeRewards")]
    public FarmPassRewardGroupDefinition[] FreeRewards { get; init; } = [];

    /// <summary>Gets the PremiumBabyPetRewards value.</summary>
    [JsonPropertyName("premiumBabyPetRewards")]
    public FarmPassRewardGroupDefinition[] PremiumBabyPetRewards { get; init; } = [];

    /// <summary>Gets the PremiumRewards value.</summary>
    [JsonPropertyName("premiumRewards")]
    public FarmPassRewardGroupDefinition[] PremiumRewards { get; init; } = [];

}
