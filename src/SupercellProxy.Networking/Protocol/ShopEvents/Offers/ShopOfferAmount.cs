using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Offers;

/// <summary>An amount of one currency in a shop offer price.</summary>
public sealed record ShopOfferAmount
{
    /// <summary>Gets how much of the currency the offer costs.</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }
}
