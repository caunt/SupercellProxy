namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>One product request and its event payment.</summary>
public sealed record FarmVisitorSpecialOffer
{
    /// <summary>Gets the visitor's requested product.</summary>
    public FarmVisitorSpecialOfferSide Deliverables { get; init; } = new();

    /// <summary>Gets the reward granted for that product.</summary>
    public FarmVisitorSpecialOfferSide Payments { get; init; } = new();
}
