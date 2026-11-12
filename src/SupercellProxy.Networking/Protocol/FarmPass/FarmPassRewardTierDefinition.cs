using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>One level of a Farm Pass season: the season points that reach it and what it rewards.</summary>
public sealed record FarmPassRewardTierDefinition
{
    /// <summary>Gets or sets the total season points at which the level is reached.</summary>
    /// <value>A cumulative point total counted from the season start, read from <c>farmPoints</c>.</value>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>Gets or sets the baby pets every player gets at this level.</summary>
    /// <value>Groups of baby pet rewards named by row; empty when the free track has no baby pet here.</value>
    [JsonPropertyName("freeBabyPetRewards")]
    public FarmPassRewardGroupDefinition[] FreeBabyPetRewards { get; init; } = [];

    /// <summary>Gets or sets the rewards every player gets at this level.</summary>
    /// <value>Alternative reward groups for the free track; empty by default.</value>
    [JsonPropertyName("freeRewards")]
    public FarmPassRewardGroupDefinition[] FreeRewards { get; init; } = [];

    /// <summary>Gets or sets the baby pets only players who bought the premium pass get at this level.</summary>
    /// <value>Groups of baby pet rewards named by row; empty when the premium track has no baby pet here.</value>
    [JsonPropertyName("premiumBabyPetRewards")]
    public FarmPassRewardGroupDefinition[] PremiumBabyPetRewards { get; init; } = [];

    /// <summary>Gets or sets the rewards only players who bought the premium pass get at this level.</summary>
    /// <value>Alternative reward groups for the premium track; empty by default.</value>
    [JsonPropertyName("premiumRewards")]
    public FarmPassRewardGroupDefinition[] PremiumRewards { get; init; } = [];
}
