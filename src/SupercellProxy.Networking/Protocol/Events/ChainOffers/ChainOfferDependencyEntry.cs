namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Contains one native chain-step requirement.</summary>
public sealed record ChainOfferDependencyEntry
{
    /// <summary>Gets the requirement.</summary>
    public ChainOfferDependencyDefinition? Dependency { get; init; }
}
