using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Retains progress and claimed rewards for one chain event.</summary>
public sealed record ChainOfferSnapshot
{
    /// <summary>Gets or sets the bit mask of acknowledged chapter introductions.</summary>
    /// <value>Bit <c>n</c> is set once the player acknowledged the intro of chapter <c>n</c>; 0 when no intro has been acknowledged, and only the lowest 16 bits are used.</value>
    [JsonPropertyName("acknowledgedChapterIntros")]
    public ulong AcknowledgedChapterIntros { get; init; }
    /// <summary>Gets or sets the completed prefix retained by older sequential-offer saves.</summary>
    /// <value>The number of leading steps an older save recorded as claimed; newer saves keep it at 0.</value>
    [JsonPropertyName("chainProgress")]
    public int ChainProgress { get; init; }
    /// <summary>Gets or sets the bit mask of claimed steps.</summary>
    /// <value>Bit <c>n</c> is set once step <c>n</c> of the offer's definition has been claimed, so an offer holds at most 64 steps; 0 when nothing is claimed yet.</value>
    [JsonPropertyName("claimedSteps")]
    public ulong ClaimedSteps { get; init; }
    /// <summary>Gets or sets the live event this chain offer belongs to.</summary>
    /// <value>The id of the shop live event (type 56) whose definition supplies this offer's steps; it keys the offer within the chain offer manager.</value>
    [JsonPropertyName("eventId")]
    public int EventId { get; init; }
    /// <summary>Gets or sets the last displayed chain progress.</summary>
    /// <value>The chain progress the client last showed the player, kept so its interface can animate newer progress.</value>
    [JsonPropertyName("lastSeenChainProgress")]
    public int LastSeenChainProgress { get; init; }
    /// <summary>Gets or sets the last displayed step progress.</summary>
    /// <value>The step progress the client last showed the player, kept for its interface only.</value>
    [JsonPropertyName("lastSeenStepProgress")]
    public int LastSeenStepProgress { get; init; }
    /// <summary>Gets or sets the last display timestamp.</summary>
    /// <value>The time, as the client saved it, at which the player last viewed the offer; display bookkeeping only.</value>
    [JsonPropertyName("lastSeenTime")]
    public int LastSeenTime { get; init; }
    /// <summary>Gets or sets the progress for each step.</summary>
    /// <value>One entry per step of the offer's definition, in the same order; empty by default.</value>
    [JsonPropertyName("steps")]
    public ChainOfferStepSnapshot[] Steps { get; init; } = [];
    /// <summary>Gets or sets the first granted resource from each claimed step.</summary>
    /// <value>One entry per step, in definition order; the entry of a step not claimed yet is empty (id and amount 0).</value>
    [JsonPropertyName("rewards")]
    public ChainOfferCollectedReward[] Rewards { get; init; } = [];
}
