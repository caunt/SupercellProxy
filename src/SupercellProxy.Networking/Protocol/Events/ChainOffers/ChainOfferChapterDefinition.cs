namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Defines the availability window for a chain-offer chapter.</summary>
public sealed record ChainOfferChapterDefinition
{
    /// <summary>Gets the chapter's duration in seconds.</summary>
    public int Duration { get; init; }

    /// <summary>Gets the chapter's offset from the event start, in seconds.</summary>
    public int StartOffset { get; init; }

    /// <summary>Gets how many of the offer's steps, following the earlier chapters' steps, belong to the chapter.</summary>
    public int StepCount { get; init; }
}
