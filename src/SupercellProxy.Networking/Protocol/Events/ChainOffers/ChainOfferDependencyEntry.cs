namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Contains one native chain-step requirement.</summary>
public sealed record ChainOfferDependencyEntry
{
    /// <summary>Gets or sets the requirement.</summary>
    /// <value>The wrapped requirement, or <see langword="null"/> when the entry carries none.</value>
    public ChainOfferDependencyDefinition? Dependency { get; init; }
}
