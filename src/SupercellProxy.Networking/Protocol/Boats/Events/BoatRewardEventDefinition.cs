using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events;

namespace SupercellProxy.Networking.Protocol.Boats.Events;

/// <summary>Defines the cargo preferences and rewards of a boat event.</summary>
public sealed record BoatRewardEventDefinition
{
    /// <summary>Gets the wire event type for boat events.</summary>
    public const int EventType = 10;

    /// <summary>Gets or sets the replacement boat-crate reward resource.</summary>
    /// <value>The data-table name of the resource paid instead of experience for crates of the matching <see cref="SeasonalBuilding"/>'s goods; <see langword="null"/> or empty keeps experience.</value>
    public string? BoatCrateExpOverride { get; init; }
    /// <summary>Gets or sets the coin reward percentage.</summary>
    /// <value>The percentage of the normal crate coin reward paid during the event, read from <c>bonusCoins</c>; 100 by default, which leaves coins unchanged.</value>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;
    /// <summary>Gets or sets the fallback completion reward pool.</summary>
    /// <value>The weighted rewards for an order whose difficulty has no pool of its own, read from <c>bonusCustomRewards</c>; empty by default.</value>
    [JsonPropertyName("bonusCustomRewards")]
    public BoatEventRewardDefinition[] CustomRewards { get; init; } = [];
    /// <summary>Gets or sets the easy-order maximum preferred cargo types.</summary>
    /// <value>The upper count of <see cref="ForcedCrateTypes"/> products put into an easy order, read from <c>maxForcedCrateTypesDifficultyEasy</c>; <see langword="null"/> falls back to <see cref="MaximumForcedTypes"/>.</value>
    [JsonPropertyName("maxForcedCrateTypesDifficultyEasy")]
    public int? EasyMaximumForcedTypes { get; init; }
    /// <summary>Gets or sets the easy-order minimum preferred cargo types.</summary>
    /// <value>The lower count of <see cref="ForcedCrateTypes"/> products put into an easy order, read from <c>minForcedCrateTypesDifficultyEasy</c>; <see langword="null"/> falls back to <see cref="MinimumForcedTypes"/>.</value>
    [JsonPropertyName("minForcedCrateTypesDifficultyEasy")]
    public int? EasyMinimumForcedTypes { get; init; }
    /// <summary>Gets or sets the easy-order completion reward pool.</summary>
    /// <value>The weighted rewards for an easy order, read from <c>bonusCustomRewardsDifficultyEasy</c>; <see langword="null"/> falls back to <see cref="CustomRewards"/>.</value>
    [JsonPropertyName("bonusCustomRewardsDifficultyEasy")]
    public BoatEventRewardDefinition[]? EasyRewards { get; init; }
    /// <summary>Gets or sets the experience reward percentage.</summary>
    /// <value>The percentage of the normal crate experience reward paid during the event, read from <c>bonusXP</c>; 100 by default, which leaves experience unchanged.</value>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;
    /// <summary>Gets or sets the event's preferred cargo products.</summary>
    /// <value>The data-table names of products the event pushes into crates, read from <c>forcedCrateTypes</c>. Empty by default.</value>
    [JsonPropertyName("forcedCrateTypes")]
    public string[] ForcedCrateTypes { get; init; } = [];
    /// <summary>Gets or sets the hard-order maximum preferred cargo types.</summary>
    /// <value>The upper count of <see cref="ForcedCrateTypes"/> products put into a hard order, read from <c>maxForcedCrateTypesDifficultyHard</c>; <see langword="null"/> falls back to <see cref="MaximumForcedTypes"/>.</value>
    [JsonPropertyName("maxForcedCrateTypesDifficultyHard")]
    public int? HardMaximumForcedTypes { get; init; }
    /// <summary>Gets or sets the hard-order minimum preferred cargo types.</summary>
    /// <value>The lower count of <see cref="ForcedCrateTypes"/> products put into a hard order, read from <c>minForcedCrateTypesDifficultyHard</c>; <see langword="null"/> falls back to <see cref="MinimumForcedTypes"/>.</value>
    [JsonPropertyName("minForcedCrateTypesDifficultyHard")]
    public int? HardMinimumForcedTypes { get; init; }
    /// <summary>Gets or sets the hard-order completion reward pool.</summary>
    /// <value>The weighted rewards for a hard order, read from <c>bonusCustomRewardsDifficultyHard</c>; <see langword="null"/> falls back to <see cref="CustomRewards"/>.</value>
    [JsonPropertyName("bonusCustomRewardsDifficultyHard")]
    public BoatEventRewardDefinition[]? HardRewards { get; init; }
    /// <summary>Gets or sets the upper percentage of the ordinary cargo-value range.</summary>
    /// <value>A percentage read from <c>maxOrderDifficultyPercent</c>; 100 by default, the full range.</value>
    [JsonPropertyName("maxOrderDifficultyPercent")]
    public int MaximumDifficultyPercentage { get; init; } = 100;
    /// <summary>Gets or sets the default maximum number of preferred cargo types.</summary>
    /// <value>The upper count read from <c>maxForcedCrateTypes</c>, used for any difficulty without its own maximum; 0 by default, which puts no preferred products into crates.</value>
    [JsonPropertyName("maxForcedCrateTypes")]
    public int MaximumForcedTypes { get; init; }
    /// <summary>Gets or sets the medium-order maximum preferred cargo types.</summary>
    /// <value>The upper count of <see cref="ForcedCrateTypes"/> products put into a medium order, read from <c>maxForcedCrateTypesDifficultyMedium</c>; <see langword="null"/> falls back to <see cref="MaximumForcedTypes"/>.</value>
    [JsonPropertyName("maxForcedCrateTypesDifficultyMedium")]
    public int? MediumMaximumForcedTypes { get; init; }
    /// <summary>Gets or sets the medium-order minimum preferred cargo types.</summary>
    /// <value>The lower count of <see cref="ForcedCrateTypes"/> products put into a medium order, read from <c>minForcedCrateTypesDifficultyMedium</c>; <see langword="null"/> falls back to <see cref="MinimumForcedTypes"/>.</value>
    [JsonPropertyName("minForcedCrateTypesDifficultyMedium")]
    public int? MediumMinimumForcedTypes { get; init; }
    /// <summary>Gets or sets the medium-order completion reward pool.</summary>
    /// <value>The weighted rewards for a medium order, read from <c>bonusCustomRewardsDifficultyMedium</c>; <see langword="null"/> falls back to <see cref="CustomRewards"/>.</value>
    [JsonPropertyName("bonusCustomRewardsDifficultyMedium")]
    public BoatEventRewardDefinition[]? MediumRewards { get; init; }
    /// <summary>Gets or sets the lower percentage of the ordinary cargo-value range.</summary>
    /// <value>A percentage read from <c>minOrderDifficultyPercent</c>; 0 by default.</value>
    [JsonPropertyName("minOrderDifficultyPercent")]
    public int MinimumDifficultyPercentage { get; init; }
    /// <summary>Gets or sets the default minimum number of preferred cargo types.</summary>
    /// <value>The lower count read from <c>minForcedCrateTypes</c>, used for any difficulty without its own minimum; 0 by default.</value>
    [JsonPropertyName("minForcedCrateTypes")]
    public int MinimumForcedTypes { get; init; }
    /// <summary>Gets or sets the event's participation requirements.</summary>
    /// <value>The requirements read from <c>requirements</c>, such as the minimum level.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements Requirements { get; init; } = new();
    /// <summary>Gets or sets the seasonal production building whose XP is replaced.</summary>
    /// <value>The data-table name of a seasonal production building, read from <c>SeasonalObject</c>; crates of its goods pay <see cref="BoatCrateExpOverride"/> instead of experience. <see langword="null"/> when the event names none.</value>
    [JsonPropertyName("SeasonalObject")]
    public string? SeasonalBuilding { get; init; }
    /// <summary>Gets or sets a value indicating whether the event reward is displayed during arrival.</summary>
    /// <value><see langword="true"/> if orders carrying this event's reward show it while the boat sails in; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("showBonusRewardWhenBoatIsArriving")]
    public bool ShowRewardWhenArriving { get; init; }
    /// <summary>Gets or sets a value indicating whether the event reward is displayed at the dock.</summary>
    /// <value><see langword="true"/> if orders carrying this event's reward show it while the boat is docked; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("showBonusRewardWhenBoatIsDocked")]
    public bool ShowRewardWhenDocked { get; init; }
}
