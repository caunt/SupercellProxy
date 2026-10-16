using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// One item of a seasonal event's gift catalogue: what it costs and what it gives.
/// </summary>
public sealed record SeasonalCatalogueGiftSnapshot
{
    /// <summary>
    /// How much of the payment currency the item costs (saved as <c>pa</c>).
    /// </summary>
    [JsonPropertyName("pa")]
    public int PaymentAmount { get; init; }

    /// <summary>
    /// Global id of the currency the item is paid with, normally the event's seasonal currency (saved as <c>pd</c>).
    /// </summary>
    [JsonPropertyName("pd")]
    public int PaymentGlobalId { get; init; }

    /// <summary>
    /// How many times the item can be bought (saved as <c>ntp</c>).
    /// </summary>
    [JsonPropertyName("ntp")]
    public int PurchaseLimit { get; init; }

    /// <summary>
    /// How many of the reward one purchase gives (saved as <c>a</c>).
    /// </summary>
    [JsonPropertyName("a")]
    public int RewardAmount { get; init; }

    /// <summary>
    /// Global id of what the purchase gives, such as a decoration or goods (saved as <c>d</c>).
    /// </summary>
    [JsonPropertyName("d")]
    public int RewardGlobalId { get; init; }
}
