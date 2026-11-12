using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// One item of a seasonal event's gift catalogue: what it costs and what it gives.
/// </summary>
public sealed record SeasonalCatalogueGiftSnapshot
{
    /// <summary>
    /// Gets or sets how much of the payment currency the item costs (saved as <c>pa</c>).
    /// </summary>
    /// <value>The price per purchase in units of the currency named by <see cref="PaymentGlobalId"/>.</value>
    [JsonPropertyName("pa")]
    public int PaymentAmount { get; init; }

    /// <summary>
    /// Gets or sets the global id of the currency the item is paid with, normally the event's seasonal currency (saved as <c>pd</c>).
    /// </summary>
    /// <value>A data-table global id that identifies both the table and the row of the payment currency.</value>
    [JsonPropertyName("pd")]
    public int PaymentGlobalId { get; init; }

    /// <summary>
    /// Gets or sets how many times the item can be bought (saved as <c>ntp</c>).
    /// </summary>
    /// <value>The purchase cap of the item; the event's purchase counts record how many were bought.</value>
    [JsonPropertyName("ntp")]
    public int PurchaseLimit { get; init; }

    /// <summary>
    /// Gets or sets how many of the reward one purchase gives (saved as <c>a</c>).
    /// </summary>
    /// <value>The quantity of <see cref="RewardGlobalId"/> granted per purchase.</value>
    [JsonPropertyName("a")]
    public int RewardAmount { get; init; }

    /// <summary>
    /// Gets or sets the global id of what the purchase gives, such as a decoration or goods (saved as <c>d</c>).
    /// </summary>
    /// <value>A data-table global id that identifies both the table and the row of the granted resource.</value>
    [JsonPropertyName("d")]
    public int RewardGlobalId { get; init; }
}
