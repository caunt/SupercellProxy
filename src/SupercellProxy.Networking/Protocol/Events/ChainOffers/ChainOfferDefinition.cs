namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Defines an event's sequence of rewards and requirements.</summary>
public sealed record ChainOfferDefinition
{
    /// <summary>The native shop-event type used for chain offers.</summary>
    public const int EventType = 56;
    /// <summary>Gets or sets whether tasks progress together or one step at a time.</summary>
    /// <value>The progress mode name as the event document writes it; <c>Single</c> by default.</value>
    public string ProgressType { get; init; } = "Single";
    /// <summary>Gets or sets whether rewards must be claimed in order.</summary>
    /// <value>The claim mode name as the event document writes it; <c>StepByStep</c> by default.</value>
    public string ClaimType { get; init; } = "StepByStep";
    /// <summary>Gets or sets the timed chapters in native order.</summary>
    /// <value>The chapters that split <see cref="Steps"/> into consecutive timed windows; empty by default.</value>
    public ChainOfferChapterDefinition[] Chapters { get; init; } = [];
    /// <summary>Gets or sets the steps in native order.</summary>
    /// <value>Each step's requirements, rewards and optional cost; a step's position is the index claim commands and saved progress use. Empty by default.</value>
    public ChainOfferStepDefinition[] Steps { get; init; } = [];
}
