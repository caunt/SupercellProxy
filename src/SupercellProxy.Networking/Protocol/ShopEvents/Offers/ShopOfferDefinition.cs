using System.Collections.Immutable;
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

    /// <summary>Gets or sets the goods the offer gives, in native order.</summary>
    /// <value>One reward entry per deliverable in the definition's <c>Deliverables</c> array, each naming its category in <see cref="ChainOfferRewardDefinition.Type"/>; empty when the definition lists none.</value>
    public ImmutableArray<ChainOfferRewardDefinition> Deliverables { get; init; } = [];

    /// <summary>Gets or sets the offer's title as the player sees it.</summary>
    /// <value>The JSON <c>displayName</c> text, or <see langword="null"/> when the definition leaves it out.</value>
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; init; }

    /// <summary>Gets or sets the in-game currency price, or <see langword="null"/> for offers sold for real money.</summary>
    /// <value>The JSON <c>ingameCurrencyPrice</c> object; with <see cref="PackageName"/> set to <see cref="InGameCurrencyPackage"/> and a price that <see cref="ShopOfferPrice.IsFree"/>, the offer is a free shop gift.</value>
    [JsonPropertyName("ingameCurrencyPrice")]
    public ShopOfferPrice? InGameCurrencyPrice { get; init; }

    /// <summary>Gets or sets the store package the offer is sold as.</summary>
    /// <value>The JSON <c>packageName</c>, such as <see cref="InGameCurrencyPackage"/> for offers paid in diamonds or gacha keys; <see langword="null"/> when absent.</value>
    [JsonPropertyName("packageName")]
    public string? PackageName { get; init; }

    /// <summary>Gets or sets how many times the offer can be bought.</summary>
    /// <value>The JSON <c>NumTimesPurchasable</c> count; 1 when the definition leaves it out.</value>
    [JsonPropertyName("NumTimesPurchasable")]
    public int PurchaseLimit { get; init; } = 1;
}
