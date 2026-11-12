namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Defines the availability window for a chain-offer chapter.</summary>
public sealed record ChainOfferChapterDefinition
{
    /// <summary>Gets or sets the chapter's duration in seconds.</summary>
    /// <value>The length of the chapter's window, which ends at the event start plus <see cref="StartOffset"/> plus this value.</value>
    public int Duration { get; init; }

    /// <summary>Gets or sets the chapter's offset from the event start, in seconds.</summary>
    /// <value>How long after the event's start time the chapter opens; 0 opens it with the event.</value>
    public int StartOffset { get; init; }

    /// <summary>Gets or sets how many of the offer's steps, following the earlier chapters' steps, belong to the chapter.</summary>
    /// <value>A step count; the chapter's first step is the sum of the earlier chapters' counts.</value>
    public int StepCount { get; init; }
}
