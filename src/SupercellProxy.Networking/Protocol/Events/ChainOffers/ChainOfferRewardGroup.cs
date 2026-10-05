namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Defines a primary reward and its ordered eligibility fallbacks.</summary>
public sealed record ChainOfferRewardGroup
{
    /// <summary>Gets the first fallback.</summary>
    public ChainOfferRewardDefinition? Fallback1 { get; init; }
    /// <summary>Gets the second fallback.</summary>
    public ChainOfferRewardDefinition? Fallback2 { get; init; }
    /// <summary>Gets the third fallback.</summary>
    public ChainOfferRewardDefinition? Fallback3 { get; init; }
    /// <summary>Gets the primary reward.</summary>
    public ChainOfferRewardDefinition? Primary { get; init; }
}
