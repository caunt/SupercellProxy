using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Defines native truck-order generation and delivery event settings.</summary>
public sealed record TruckOrderEventDefinition
{
    /// <summary>Gets the truck-bonus type in the serialized event collection.</summary>
    public const int EventType = 8;

    /// <summary>Gets or sets the coin percentage applied when the order is dispatched.</summary>
    /// <value>
    /// Saved as <c>bonusCoins</c>; 100 (the default) leaves coins unchanged.
    /// </value>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;

    /// <summary>Gets or sets the experience percentage applied when the order is dispatched.</summary>
    /// <value>
    /// Saved as <c>bonusXP</c>; 100 (the default) leaves experience unchanged.
    /// </value>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;

    /// <summary>Gets or sets the percentage of the ordinary order-value range.</summary>
    /// <value>Saved as <c>maxOrderDifficulty</c>; a percentage that defaults to 100.</value>
    [JsonPropertyName("maxOrderDifficulty")]
    public int MaximumDifficulty { get; init; } = 100;

    /// <summary>Gets or sets the event's custom reward pool.</summary>
    /// <value>Saved as <c>bonusCustomRewards</c>; weighted entries, and <see cref="RewardSet"/> is used only when it is empty.</value>
    [JsonPropertyName("bonusCustomRewards")]
    public TruckOrderEventRewardDefinition[] CustomRewards { get; init; } = [];

    /// <summary>Gets or sets the goods eligible for seasonal currency; an empty list accepts every order.</summary>
    /// <value>
    /// Saved as <c>eligibleSeasonalGoods</c>; seasonal goods row names. Empty by default.
    /// </value>
    [JsonPropertyName("eligibleSeasonalGoods")]
    public string[] EligibleSeasonalGoods { get; init; } = [];

    /// <summary>Gets or sets the minimum difficulty percentage eligible for an item bonus.</summary>
    /// <value>
    /// Saved as <c>bonusRewardMinDifficulty</c>; a percentage of the order-value range of the player's level, 0 at the minimum and 100 at the
    /// maximum.
    /// </value>
    [JsonPropertyName("bonusRewardMinDifficulty")]
    public int MinimumBonusDifficulty { get; init; }

    /// <summary>Gets or sets the named reward set, when configured.</summary>
    /// <value>Saved as <c>bonusRewardSet</c>; a row name in the car order reward set table, used when <see cref="CustomRewards"/> is empty, or <see langword="null"/> when there is none.</value>
    [JsonPropertyName("bonusRewardSet")]
    public string? RewardSet { get; init; }

    /// <summary>Gets or sets the seasonal resource awarded by ordinary truck orders.</summary>
    /// <value>Saved as <c>seasonalCurrency</c>; a seasonal currency row name, or <see langword="null"/> or empty when the event pays none.</value>
    [JsonPropertyName("seasonalCurrency")]
    public string? SeasonalCurrency { get; init; }

    /// <summary>Gets or sets the explicit seasonal quantity.</summary>
    /// <value>
    /// Saved as <c>seasonalCurrencyAmount</c>; a fixed amount per order when positive.
    /// </value>
    [JsonPropertyName("seasonalCurrencyAmount")]
    public int SeasonalCurrencyAmount { get; init; }
}
