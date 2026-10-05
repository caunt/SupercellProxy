namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Describes a timed or gameplay requirement for a chain step.</summary>
public sealed record ChainOfferDependencyDefinition
{
    /// <summary>Gets the gameplay action requirement.</summary>
    public ChainOfferFarmTaskDefinition? FarmTask { get; init; }
    /// <summary>Gets the delay from the event's start.</summary>
    public ChainOfferTimeDefinition? TimeSinceEventStart { get; init; }
    /// <summary>Gets the native requirement discriminator.</summary>
    public string? Type { get; init; }
}
