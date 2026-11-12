namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Identifies a chain step's purchase category.</summary>
public sealed record ChainOfferCostDefinition
{
    /// <summary>Gets or sets the native purchase category discriminator.</summary>
    /// <value>The category name as native writes it, or <see langword="null"/>; the set of values is not yet identified.</value>
    public string? Type { get; init; }
}
