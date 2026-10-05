using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Retains progress and claimed rewards for one chain event.</summary>
public sealed record ChainOfferSnapshot
{
    /// <summary>Gets the number of claims made in the chain.</summary>
    [JsonPropertyName("chainProgress")]
    public int ChainProgress { get; init; }
    /// <summary>Gets the bit mask of independently claimed steps.</summary>
    [JsonPropertyName("claimedSteps")]
    public ulong ClaimedSteps { get; init; }
    /// <summary>Gets the event identifier.</summary>
    [JsonPropertyName("eventId")]
    public int EventIdentifier { get; init; }
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
