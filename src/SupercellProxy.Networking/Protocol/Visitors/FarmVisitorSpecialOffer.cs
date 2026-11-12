namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>One product request and its event payment.</summary>
public sealed record FarmVisitorSpecialOffer
{
    /// <summary>Gets or sets the visitor's requested product.</summary>
    /// <value>Goods side of the trade the visitor asks for; an empty side by default.</value>
    public FarmVisitorSpecialOfferSide Deliverables { get; init; } = new();

    /// <summary>Gets or sets the reward granted for that product.</summary>
    /// <value>Money or goods the visitor pays when the requested product is delivered; an empty side by default.</value>
    public FarmVisitorSpecialOfferSide Payments { get; init; } = new();
}
