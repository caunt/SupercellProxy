using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Retains progress and claimed rewards for one chain event.</summary>
public sealed record ChainOfferSnapshot
{
    /// <summary>Gets the completed prefix retained by older sequential-offer saves.</summary>
    [JsonPropertyName("chainProgress")]
    public int ChainProgress { get; init; }
    /// <summary>Gets the bit mask of claimed steps.</summary>
    [JsonPropertyName("claimedSteps")]
    public ulong ClaimedSteps { get; init; }
    /// <summary>Gets the event id.</summary>
    [JsonPropertyName("eventId")]
    public int EventId { get; init; }
    /// <summary>Gets the last displayed chain progress.</summary>
    [JsonPropertyName("lastSeenChainProgress")]
    public int LastSeenChainProgress { get; init; }
    /// <summary>Gets the last displayed step progress.</summary>
    [JsonPropertyName("lastSeenStepProgress")]
    public int LastSeenStepProgress { get; init; }
    /// <summary>Gets the last display timestamp.</summary>
    [JsonPropertyName("lastSeenTime")]
    public int LastSeenTime { get; init; }
    /// <summary>Gets the progress for each step.</summary>
    [JsonPropertyName("steps")]
    public ChainOfferStepSnapshot[] Steps { get; init; } = [];
    /// <summary>Gets the first granted resource from each claimed step.</summary>
    [JsonPropertyName("rewards")]
    public ChainOfferCollectedReward[] Rewards { get; init; } = [];
}
