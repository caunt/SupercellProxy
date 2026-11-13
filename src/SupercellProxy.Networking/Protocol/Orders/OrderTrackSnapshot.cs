using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Saved progress of an order track: delivered orders earn points, each milestone's points unlock its rewards, and the track
/// starts over after its reset period.
/// </summary>
public sealed record OrderTrackSnapshot
{
    /// <summary>Gets or sets the saved account-specific calendar offset in seconds.</summary>
    /// <value>
    /// Saved as <c>calOff</c>; seconds after the daily reset hour at which this account's calendar-based track resets, below one day. -1
    /// (the default) means not set yet.
    /// </value>
    [JsonPropertyName("calOff")]
    public int CalendarOffsetSeconds { get; init; } = -1;

    /// <summary>Gets or sets the points earned toward the current milestone.</summary>
    /// <value>Saved as <c>completed</c>; compare with <see cref="Target"/>, the points the milestone needs.</value>
    [JsonPropertyName("completed")]
    public int Completed { get; init; }

    /// <summary>Gets or sets whether the player has seen this boat-track cycle's introduction.</summary>
    /// <value><see langword="true"/> if the introduction of the cycle that started at <see cref="ResetTimestamp"/> was shown; otherwise, <see langword="false"/>.</value>
    /// <remarks>Only the boat track uses the flag.</remarks>
    [JsonPropertyName("cycleIntroSeen")]
    public bool CycleIntroSeen { get; init; }

    /// <summary>Gets or sets the position of the current milestone in <see cref="Rewards"/>.</summary>
    /// <value>Saved as <c>idx</c>; a zero-based index.</value>
    [JsonPropertyName("idx")]
    public int Index { get; init; }

    /// <summary>
    /// Gets or sets the points from a finished boat order that are not added to the track yet; the client adds them with a separate settle command.
    /// </summary>
    /// <value>Saved as <c>pendingCompletionPoints</c>; 0 when nothing is waiting.</value>
    [JsonPropertyName("pendingCompletionPoints")]
    public int PendingCompletionPoints { get; init; }

    /// <summary>Gets or sets the boat order associated with pending completion points.</summary>
    /// <value>Saved as <c>pendingCompletionPointsOrderId</c>; the id of the last order whose points were queued, or -1 (the default) when none was.</value>
    [JsonPropertyName("pendingCompletionPointsOrderId")]
    public int PendingCompletionPointsOrderId { get; init; } = -1;

    /// <summary>Gets or sets the boat associated with pending completion points.</summary>
    /// <value>Saved as <c>pendingCompletionPointsBoatId</c>; the id of the boat of the last queued order, or -1 (the default) when none was.</value>
    [JsonPropertyName("pendingCompletionPointsBoatId")]
    public int PendingCompletionPointsBoatId { get; init; } = -1;

    /// <summary>Gets or sets the Unix time in seconds when the current track started.</summary>
    /// <value>Saved as <c>reset</c>; seconds since 1970-01-01 UTC.</value>
    [JsonPropertyName("reset")]
    public int ResetTimestamp { get; init; }

    /// <summary>
    /// Gets or sets whether the current milestone's rewards have already been granted (saved as <c>claimed</c>).
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the milestone paid out and no later milestone followed, as on the last milestone of a track that is not
    /// endless; otherwise, <see langword="false"/>.
    /// </value>
    [JsonPropertyName("claimed")]
    public bool RewardClaimed { get; init; }

    /// <summary>Gets or sets the rewards of each milestone; one list per milestone.</summary>
    /// <value>Saved as <c>rewards</c>; one array per milestone, as many as the track has point requirements.</value>
    [JsonPropertyName("rewards")]
    public ImmutableArray<ImmutableArray<OrderTrackReward>> Rewards { get; init; } = [];

    /// <summary>Gets or sets the milestone rewards awaiting delivery after the previous track expired.</summary>
    /// <value>Saved as <c>savedUnclaimedRewards</c>; milestone rewards waiting to be sent by the postman. Empty by default.</value>
    [JsonPropertyName("savedUnclaimedRewards")]
    public ImmutableArray<ImmutableArray<OrderTrackReward>> SavedUnclaimedRewards { get; init; } = [];

    /// <summary>Gets or sets the points the current milestone needs; 0 when the track has no further milestone.</summary>
    /// <value>Saved as <c>target</c>; the track row's required points for milestone <see cref="Index"/>.</value>
    [JsonPropertyName("target")]
    public int Target { get; init; }

    /// <summary>Gets or sets the global id of the track's table entry; 0 when no track runs.</summary>
    /// <value>Saved as <c>track</c>; a global id in the order track table, whose rows list each milestone's points and reward pool.</value>
    [JsonPropertyName("track")]
    public int TrackId { get; init; }
}
