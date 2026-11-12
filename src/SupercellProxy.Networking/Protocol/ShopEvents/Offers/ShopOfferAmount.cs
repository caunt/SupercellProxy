using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Offers;

/// <summary>An amount of one currency in a shop offer price.</summary>
public sealed record ShopOfferAmount
{
    /// <summary>Gets or sets how much of the currency the offer costs.</summary>
    /// <value>The number of units of the currency the enclosing price names, read from JSON <c>amount</c>; 0, the default, makes a diamond price free.</value>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }
}
