namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Identifies a chain step's purchase category.</summary>
public sealed record ChainOfferCostDefinition
{
    /// <summary>Gets the native purchase category discriminator.</summary>
    public string? Type { get; init; }
}
