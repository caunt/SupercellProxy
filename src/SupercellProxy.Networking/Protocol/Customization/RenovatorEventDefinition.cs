using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Describes the server-provided stock, schedule and purchase rules for a renovator event.</summary>
public sealed record RenovatorEventDefinition
{
    /// <summary>Identifies the native global renovator event type.</summary>
    public const int EventType = 25;

    /// <summary>Gets or sets the per-category reroll allowance; minus one keeps the normal allowance.</summary>
    /// <value>Rerolls allowed per category per stock, read from <c>allowedRerolls</c>; -1 by default, which uses the configured allowance. Any other value also makes <see cref="RerollPrices"/> the price list.</value>
    [JsonPropertyName("allowedRerolls")]
    public int AllowedRerolls { get; init; } = -1;

    /// <summary>Gets or sets the buy-back discount percentage; minus one keeps the normal percentage.</summary>
    /// <value>The percentage taken off parts the player owned before, read from <c>buyBackDiscountPercentage</c>; -1 by default, which uses the configured buy-back discount.</value>
    [JsonPropertyName("buyBackDiscountPercentage")]
    public int BuyBackDiscountPercentage { get; init; } = -1;

    /// <summary>Gets or sets the stock day relative to Monday; minus one keeps the normal day.</summary>
    /// <value>Days after Monday the renovator arrives (0 is Monday), read from <c>dayOfWeek</c>; -1 by default, which uses the configured spawn day.</value>
    [JsonPropertyName("dayOfWeek")]
    public int DayOfWeek { get; init; } = -1;

    /// <summary>Gets or sets the category-specific purchase discounts.</summary>
    /// <value>One discount per named category, read from <c>discounts</c>; <see langword="null"/> when the event discounts nothing.</value>
    [JsonPropertyName("discounts")]
    public ImmutableArray<RenovatorDiscountDefinition>? Discounts { get; init; }

    /// <summary>Gets or sets the ordered, asset-qualified parts to offer instead of random stock.</summary>
    /// <value>Names such as <c>Truck.Name</c>, <c>Home.Name</c> or <c>RoadsideShop.Name</c>, read from <c>forcedStock</c>; <see langword="null"/> when absent.</value>
    [JsonPropertyName("forcedStock")]
    public ImmutableArray<string>? ForcedStock { get; init; }

    /// <summary>Gets or sets a value indicating whether this event permits rerolling stock.</summary>
    /// <value><see langword="true"/> if the event document's <c>rerollAllowed</c> is set; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag is carried as read; the reroll count itself comes from <see cref="AllowedRerolls"/>.</remarks>
    [JsonPropertyName("rerollAllowed")]
    public bool RerollAllowed { get; init; }

    /// <summary>Gets or sets the diamond prices for successive rerolls of a category.</summary>
    /// <value>The price of the first, second and later rerolls, read from <c>rerollPrices</c>; used only when <see cref="AllowedRerolls"/> is not -1.</value>
    [JsonPropertyName("rerollPrices")]
    public ImmutableArray<int>? RerollPrices { get; init; }

    /// <summary>Gets or sets the stock reset hour; minus one keeps the normal hour.</summary>
    /// <value>The UTC hour of the stock day the renovator arrives, read from <c>resetHour</c>; -1 by default, which uses the configured spawn hour.</value>
    [JsonPropertyName("resetHour")]
    public int ResetHour { get; init; } = -1;

    /// <summary>Gets or sets the visit duration in hours; minus one keeps the normal duration.</summary>
    /// <value>How many hours the renovator stays after arriving, read from <c>stayTimeHours</c>; -1 by default, which uses the configured stay time.</value>
    [JsonPropertyName("stayTimeHours")]
    public int StayTimeHours { get; init; } = -1;
}
