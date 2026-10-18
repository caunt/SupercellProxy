using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events.ChainOffers;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Offers;

/// <summary>Defines one shop offer, the JSON of a "ShopNew" shop event: what it gives, what it costs and how often it can be bought.</summary>
public sealed record ShopOfferDefinition
{
    /// <summary>The native shop-event type of shop offers.</summary>
    public const int EventType = 1;
    /// <summary>The package of offers bought with in-game currency instead of real money.</summary>
    public const string InGameCurrencyPackage = "com.supercell.soil.ingamecurrency";

    /// <summary>Gets the goods the offer gives, in native order.</summary>
    public ChainOfferRewardDefinition[] Deliverables { get; init; } = [];

    /// <summary>Gets the offer's title as the player sees it.</summary>
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; init; }

    /// <summary>Gets the in-game currency price, or null for offers sold for real money.</summary>
    [JsonPropertyName("ingameCurrencyPrice")]
    public ShopOfferPrice? InGameCurrencyPrice { get; init; }

    /// <summary>Gets the store package the offer is sold as.</summary>
    [JsonPropertyName("packageName")]
    public string? PackageName { get; init; }

    /// <summary>Gets how many times the offer can be bought.</summary>
    [JsonPropertyName("NumTimesPurchasable")]
    public int PurchaseLimit { get; init; } = 1;
}
