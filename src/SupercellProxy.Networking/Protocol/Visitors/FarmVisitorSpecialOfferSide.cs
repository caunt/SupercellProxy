namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>One typed resource on either side of a special visitor trade.</summary>
public sealed record FarmVisitorSpecialOfferSide
{
    /// <summary>Gets or sets the money resource when this side uses money.</summary>
    /// <value>Resource read when <see cref="Type"/> is <c>"0"</c>, or <see langword="null"/> when this side is not paid in money.</value>
    public FarmVisitorSpecialOfferResource? Money { get; init; }

    /// <summary>Gets or sets the product resource when this side uses products.</summary>
    /// <value>Resource read when <see cref="Type"/> is <c>"2"</c>, or <see langword="null"/> when this side holds no product.</value>
    public FarmVisitorSpecialOfferResource? Products { get; init; }

    /// <summary>Gets or sets the native resource-category code.</summary>
    /// <value>Category as a decimal string: <c>"0"</c> selects <see cref="Money"/> and <c>"2"</c> selects <see cref="Products"/>; other codes are kept as read. Empty by default.</value>
    public string Type { get; init; } = string.Empty;
}
