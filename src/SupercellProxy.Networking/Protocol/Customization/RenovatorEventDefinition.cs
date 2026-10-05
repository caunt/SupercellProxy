using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Describes the server-provided stock, schedule and purchase rules for a renovator event.</summary>
public sealed record RenovatorEventDefinition
{
    /// <summary>Identifies the native global renovator event type.</summary>
    public const int EventType = 25;

    /// <summary>Gets the per-category reroll allowance; minus one keeps the normal allowance.</summary>
    [JsonPropertyName("allowedRerolls")]
    public int AllowedRerolls { get; init; } = -1;

    /// <summary>Gets the buy-back discount percentage; minus one keeps the normal percentage.</summary>
    [JsonPropertyName("buyBackDiscountPercentage")]
    public int BuyBackDiscountPercentage { get; init; } = -1;

    /// <summary>Gets the stock day relative to Monday; minus one keeps the normal day.</summary>
    [JsonPropertyName("dayOfWeek")]
    public int DayOfWeek { get; init; } = -1;

    /// <summary>Gets the category-specific purchase discounts.</summary>
    [JsonPropertyName("discounts")]
    public RenovatorDiscountDefinition[]? Discounts { get; init; }

    /// <summary>Gets the ordered, asset-qualified parts to offer instead of random stock.</summary>
    [JsonPropertyName("forcedStock")]
    public string[]? ForcedStock { get; init; }

    /// <summary>Gets whether this event permits rerolling stock.</summary>
    [JsonPropertyName("rerollAllowed")]
    public bool RerollAllowed { get; init; }

    /// <summary>Gets the diamond prices for successive rerolls of a category.</summary>
    [JsonPropertyName("rerollPrices")]
    public int[]? RerollPrices { get; init; }

    /// <summary>Gets the stock reset hour; minus one keeps the normal hour.</summary>
    [JsonPropertyName("resetHour")]
    public int ResetHour { get; init; } = -1;

    /// <summary>Gets the visit duration in hours; minus one keeps the normal duration.</summary>
    [JsonPropertyName("stayTimeHours")]
    public int StayTimeHours { get; init; } = -1;
}
