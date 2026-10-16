using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Live event describing one Farm Pass season: its baby pet, road, levels and rewards.</summary>
public sealed record FarmPassEventDefinition
{
    /// <summary>Name of the season's baby pet table entry.</summary>
    [JsonPropertyName("farmPassBabyPet")]
    public string? BabyPet { get; init; }

    /// <summary>Diamond options of the season; null when it has none.</summary>
    [JsonPropertyName("diamondSinksConfig")]
    public FarmPassDiamondSinks? DiamondSinks { get; init; }

    /// <summary>The season's levels in order, each with the points that reach it and its free and premium rewards.</summary>
    [JsonPropertyName("rewardTiers")]
    public FarmPassRewardTierDefinition[] RewardTiers { get; init; } = [];

    /// <summary>Name of the season's Farm Pass road table entry.</summary>
    [JsonPropertyName("farmPassRoad")]
    public string? Road { get; init; }

    /// <summary>Season point marks that split the season into stages, compared with the player's season points.</summary>
    [JsonPropertyName("farmPassThresholds")]
    public int[] Thresholds { get; init; } = [];
}
