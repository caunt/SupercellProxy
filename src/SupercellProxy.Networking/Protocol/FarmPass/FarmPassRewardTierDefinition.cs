using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>One level of a Farm Pass season: the season points that reach it and what it rewards.</summary>
public sealed record FarmPassRewardTierDefinition
{
    /// <summary>Total season points at which the level is reached.</summary>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>Baby pets every player gets at this level.</summary>
    [JsonPropertyName("freeBabyPetRewards")]
    public FarmPassRewardGroupDefinition[] FreeBabyPetRewards { get; init; } = [];

    /// <summary>Rewards every player gets at this level.</summary>
    [JsonPropertyName("freeRewards")]
    public FarmPassRewardGroupDefinition[] FreeRewards { get; init; } = [];

    /// <summary>Baby pets only players who bought the premium pass get at this level.</summary>
    [JsonPropertyName("premiumBabyPetRewards")]
    public FarmPassRewardGroupDefinition[] PremiumBabyPetRewards { get; init; } = [];

    /// <summary>Rewards only players who bought the premium pass get at this level.</summary>
    [JsonPropertyName("premiumRewards")]
    public FarmPassRewardGroupDefinition[] PremiumRewards { get; init; } = [];
}
