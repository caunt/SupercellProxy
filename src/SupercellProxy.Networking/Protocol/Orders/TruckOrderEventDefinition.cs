using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Defines native truck-order generation and delivery event settings.</summary>
public sealed record TruckOrderEventDefinition
{
    /// <summary>Gets the native truck-event type.</summary>
    public const int EventType = 3;

    /// <summary>Gets the coin percentage applied when the order is dispatched.</summary>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;

    /// <summary>Gets the experience percentage applied when the order is dispatched.</summary>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;

    /// <summary>Gets the percentage of the ordinary order-value range.</summary>
    [JsonPropertyName("maxOrderDifficulty")]
    public int MaximumDifficulty { get; init; } = 100;

    /// <summary>Gets the event's custom reward pool.</summary>
    [JsonPropertyName("bonusCustomRewards")]
    public TruckOrderEventRewardDefinition[] CustomRewards { get; init; } = [];

    /// <summary>Gets the goods eligible for seasonal currency; an empty list accepts every order.</summary>
    [JsonPropertyName("eligibleSeasonalGoods")]
    public string[] EligibleSeasonalGoods { get; init; } = [];

    /// <summary>Gets the minimum difficulty percentage eligible for an item bonus.</summary>
    [JsonPropertyName("bonusRewardMinDifficulty")]
    public int MinimumBonusDifficulty { get; init; }

    /// <summary>Gets the named reward set, when configured.</summary>
    [JsonPropertyName("bonusRewardSet")]
    public string? RewardSet { get; init; }

    /// <summary>Gets the seasonal resource awarded by ordinary truck orders.</summary>
    [JsonPropertyName("seasonalCurrency")]
    public string? SeasonalCurrency { get; init; }

    /// <summary>Gets the explicit seasonal quantity; nonpositive quantities use native XP scaling.</summary>
    [JsonPropertyName("seasonalCurrencyAmount")]
    public int SeasonalCurrencyAmount { get; init; }
}
