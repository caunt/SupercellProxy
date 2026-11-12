namespace SupercellProxy.Networking.Protocol.ShopEvents.Offers;

/// <summary>Prices a shop offer in one in-game currency, named by <see cref="Type"/>.</summary>
public sealed record ShopOfferPrice
{
    /// <summary>Gets or sets the price in diamonds, when <see cref="Type"/> is "Diamonds".</summary>
    /// <value>The diamond amount from the JSON <c>Diamonds</c> object, or <see langword="null"/> when the definition has none, as for a gacha-key price.</value>
    public ShopOfferAmount? Diamonds { get; init; }
    /// <summary>Gets or sets the price in gacha keys, when <see cref="Type"/> is "GachaCurrency".</summary>
    /// <value>The key amount from the JSON <c>GachaCurrency</c> object, or <see langword="null"/> when the definition has none, as for a diamond price.</value>
    public ShopOfferAmount? GachaCurrency { get; init; }
    /// <summary>Gets whether the offer costs nothing: a diamond price of zero.</summary>
    /// <value><see langword="true"/> if <see cref="Type"/> is exactly "Diamonds" and <see cref="Diamonds"/> holds an amount of 0; otherwise, <see langword="false"/>.</value>
    /// <remarks>An offer whose diamond amount is missing does not count as free.</remarks>
    public bool IsFree => Type == "Diamonds" && Diamonds is { Amount: 0 };

    /// <summary>Gets or sets the currency the price is paid in.</summary>
    /// <value>The case-sensitive currency name, "Diamonds" or "GachaCurrency" in known offers, which selects whether <see cref="Diamonds"/> or <see cref="GachaCurrency"/> applies; <see langword="null"/> when absent.</value>
    public string? Type { get; init; }
}
