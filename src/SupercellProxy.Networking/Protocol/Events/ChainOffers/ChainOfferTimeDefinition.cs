namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Describes an elapsed-time requirement for a chain reward.</summary>
public sealed record ChainOfferTimeDefinition
{
    /// <summary>Gets the required elapsed seconds.</summary>
    public int Time { get; init; }
}
