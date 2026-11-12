namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Describes an elapsed-time requirement for a chain reward.</summary>
public sealed record ChainOfferTimeDefinition
{
    /// <summary>Gets or sets the required elapsed seconds.</summary>
    /// <value>The number of seconds that must have passed since the event started before the requirement holds.</value>
    public int Time { get; init; }
}
