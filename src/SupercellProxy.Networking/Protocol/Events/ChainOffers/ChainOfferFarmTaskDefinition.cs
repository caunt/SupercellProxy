namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Describes gameplay progress needed for a chain reward.</summary>
public sealed record ChainOfferFarmTaskDefinition
{
    /// <summary>Gets the required progress.</summary>
    public int Amount { get; init; }
    /// <summary>Gets the optional product or building filter.</summary>
    public string? Good { get; init; }
    /// <summary>Gets the native task type.</summary>
    public string? Type { get; init; }
}
