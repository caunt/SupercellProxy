using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Defines the cargo preferences and rewards of a boat event.</summary>
public sealed record BoatRewardEventDefinition
{
    /// <summary>Gets the wire event type for boat events.</summary>
    public const int EventType = 10;

    /// <summary>Gets the replacement boat-crate reward resource.</summary>
    public string? BoatCrateExpOverride { get; init; }
    /// <summary>Gets the coin reward percentage.</summary>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;
    /// <summary>Gets the fallback completion reward pool.</summary>
    [JsonPropertyName("bonusCustomRewards")]
    public BoatEventRewardDefinition[] CustomRewards { get; init; } = [];
    /// <summary>Gets the easy-order maximum preferred cargo types.</summary>
    [JsonPropertyName("maxForcedCrateTypesDifficultyEasy")]
    public int? EasyMaximumForcedTypes { get; init; }
    /// <summary>Gets the easy-order minimum preferred cargo types.</summary>
    [JsonPropertyName("minForcedCrateTypesDifficultyEasy")]
    public int? EasyMinimumForcedTypes { get; init; }
    /// <summary>Gets the easy-order completion reward pool.</summary>
    [JsonPropertyName("bonusCustomRewardsDifficultyEasy")]
    public BoatEventRewardDefinition[]? EasyRewards { get; init; }
    /// <summary>Gets the experience reward percentage.</summary>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;
    /// <summary>Gets the event's preferred cargo products.</summary>
    [JsonPropertyName("forcedCrateTypes")]
    public string[] ForcedCrateTypes { get; init; } = [];
    /// <summary>Gets the hard-order maximum preferred cargo types.</summary>
    [JsonPropertyName("maxForcedCrateTypesDifficultyHard")]
    public int? HardMaximumForcedTypes { get; init; }
    /// <summary>Gets the hard-order minimum preferred cargo types.</summary>
    [JsonPropertyName("minForcedCrateTypesDifficultyHard")]
    public int? HardMinimumForcedTypes { get; init; }
    /// <summary>Gets the hard-order completion reward pool.</summary>
    [JsonPropertyName("bonusCustomRewardsDifficultyHard")]
    public BoatEventRewardDefinition[]? HardRewards { get; init; }
    /// <summary>Gets the upper percentage of the ordinary cargo-value range.</summary>
    [JsonPropertyName("maxOrderDifficultyPercent")]
    public int MaximumDifficultyPercentage { get; init; } = 100;
    /// <summary>Gets the default maximum number of preferred cargo types.</summary>
    [JsonPropertyName("maxForcedCrateTypes")]
    public int MaximumForcedTypes { get; init; }
    /// <summary>Gets the medium-order maximum preferred cargo types.</summary>
    [JsonPropertyName("maxForcedCrateTypesDifficultyMedium")]
    public int? MediumMaximumForcedTypes { get; init; }
    /// <summary>Gets the medium-order minimum preferred cargo types.</summary>
    [JsonPropertyName("minForcedCrateTypesDifficultyMedium")]
    public int? MediumMinimumForcedTypes { get; init; }
    /// <summary>Gets the medium-order completion reward pool.</summary>
    [JsonPropertyName("bonusCustomRewardsDifficultyMedium")]
    public BoatEventRewardDefinition[]? MediumRewards { get; init; }
    /// <summary>Gets the lower percentage of the ordinary cargo-value range.</summary>
    [JsonPropertyName("minOrderDifficultyPercent")]
    public int MinimumDifficultyPercentage { get; init; }
    /// <summary>Gets the default minimum number of preferred cargo types.</summary>
    [JsonPropertyName("minForcedCrateTypes")]
    public int MinimumForcedTypes { get; init; }
    /// <summary>Gets the event's participation requirements.</summary>
    [JsonPropertyName("requirements")]
    public Events.EventLevelRequirements Requirements { get; init; } = new();
    /// <summary>Gets the seasonal production building whose XP is replaced.</summary>
    [JsonPropertyName("SeasonalObject")]
    public string? SeasonalBuilding { get; init; }
    /// <summary>Gets whether the event reward is displayed during arrival.</summary>
    [JsonPropertyName("showBonusRewardWhenBoatIsArriving")]
    public bool ShowRewardWhenArriving { get; init; }
    /// <summary>Gets whether the event reward is displayed at the dock.</summary>
    [JsonPropertyName("showBonusRewardWhenBoatIsDocked")]
    public bool ShowRewardWhenDocked { get; init; }
}
