using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Live event describing one Farm Pass season: its baby pet, road, levels and rewards.</summary>
public sealed record FarmPassEventDefinition
{
    /// <summary>Gets or sets the name of the season's baby pet table entry.</summary>
    /// <value>A row name of the baby pets table, read from <c>farmPassBabyPet</c>.</value>
    [JsonPropertyName("farmPassBabyPet")]
    public string? BabyPet { get; init; }

    /// <summary>Gets or sets the diamond options of the season; <see langword="null"/> when it has none.</summary>
    /// <value>The <c>diamondSinksConfig</c> object of the event document.</value>
    [JsonPropertyName("diamondSinksConfig")]
    public FarmPassDiamondSinks? DiamondSinks { get; init; }

    /// <summary>Gets or sets the season's levels in order, each with the points that reach it and its free and premium rewards.</summary>
    /// <value>One entry per level, first level first; empty when the event lists no tiers.</value>
    [JsonPropertyName("rewardTiers")]
    public FarmPassRewardTierDefinition[] RewardTiers { get; init; } = [];

    /// <summary>Gets or sets the name of the season's Farm Pass road table entry.</summary>
    /// <value>A row name of the Farm Pass road table, read from <c>farmPassRoad</c>.</value>
    [JsonPropertyName("farmPassRoad")]
    public string? Road { get; init; }

    /// <summary>Gets or sets the season point marks that split the season into stages.</summary>
    /// <value>Ascending season-point totals read from <c>farmPassThresholds</c>; empty when the event defines no stages.</value>
    [JsonPropertyName("farmPassThresholds")]
    public int[] Thresholds { get; init; } = [];
}
