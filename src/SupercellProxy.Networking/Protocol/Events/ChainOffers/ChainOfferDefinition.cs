namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Defines an event's sequence of rewards and requirements.</summary>
public sealed record ChainOfferDefinition
{
    /// <summary>The native shop-event type used for chain offers.</summary>
    public const int EventType = 56;
    /// <summary>Gets whether tasks progress together or one step at a time.</summary>
    public string ProgressType { get; init; } = "Single";
    /// <summary>Gets whether rewards must be claimed in order.</summary>
    public string ClaimType { get; init; } = "StepByStep";
    /// <summary>Gets the timed chapters in native order.</summary>
    public ChainOfferChapterDefinition[] Chapters { get; init; } = [];
    /// <summary>Gets the steps in native order.</summary>
    public ChainOfferStepDefinition[] Steps { get; init; } = [];
}
