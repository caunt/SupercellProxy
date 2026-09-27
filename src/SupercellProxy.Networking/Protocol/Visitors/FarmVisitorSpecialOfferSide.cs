namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>One typed resource on either side of a special visitor trade.</summary>
public sealed record FarmVisitorSpecialOfferSide
{
    /// <summary>Gets the money resource when this side uses money.</summary>
    public FarmVisitorSpecialOfferResource? Money { get; init; }

    /// <summary>Gets the product resource when this side uses products.</summary>
    public FarmVisitorSpecialOfferResource? Products { get; init; }

    /// <summary>Gets the native resource-category code.</summary>
    public string Type { get; init; } = string.Empty;
}
