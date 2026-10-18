namespace SupercellProxy.Networking.Protocol.ShopEvents.Offers;

/// <summary>Prices a shop offer in one in-game currency, named by <see cref="Type"/>.</summary>
public sealed record ShopOfferPrice
{
    /// <summary>Gets the price in diamonds, when <see cref="Type"/> is "Diamonds".</summary>
    public ShopOfferAmount? Diamonds { get; init; }
    /// <summary>Gets the price in gacha keys, when <see cref="Type"/> is "GachaCurrency".</summary>
    public ShopOfferAmount? GachaCurrency { get; init; }
    /// <summary>Whether the offer costs nothing: a diamond price of zero.</summary>
    public bool IsFree => Type == "Diamonds" && Diamonds is { Amount: 0 };

    /// <summary>Gets the currency the price is paid in.</summary>
    public string? Type { get; init; }
}
