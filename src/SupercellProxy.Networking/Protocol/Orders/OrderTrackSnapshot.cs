using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Saved progress of an order track: delivered orders earn points, each milestone's points unlock its rewards, and the track
/// starts over after its reset period.
/// </summary>
public sealed record OrderTrackSnapshot
{
    /// <summary>Gets the saved account-specific calendar offset in seconds.</summary>
    [JsonPropertyName("calOff")]
    public int CalendarOffsetSeconds { get; init; } = -1;

    /// <summary>Points earned toward the current milestone.</summary>
    [JsonPropertyName("completed")]
    public int Completed { get; init; }

    /// <summary>Gets whether the player has seen this boat-track cycle's introduction.</summary>
    [JsonPropertyName("cycleIntroSeen")]
    public bool CycleIntroSeen { get; init; }

    /// <summary>Position of the current milestone in <see cref="Rewards"/>.</summary>
    [JsonPropertyName("idx")]
    public int Index { get; init; }

    /// <summary>
    /// Points from a finished boat order that are not added to the track yet; the client adds them with a separate settle command,
    /// or when the track resets.
    /// </summary>
    [JsonPropertyName("pendingCompletionPoints")]
    public int PendingCompletionPoints { get; init; }

    /// <summary>Gets the boat order associated with pending completion points.</summary>
    [JsonPropertyName("pendingCompletionPointsOrderId")]
    public int PendingCompletionPointsOrderId { get; init; } = -1;

    /// <summary>Gets the boat associated with pending completion points.</summary>
    [JsonPropertyName("pendingCompletionPointsBoatId")]
    public int PendingCompletionPointsBoatId { get; init; } = -1;

    /// <summary>Unix time in seconds when the current track started; the reset period counts from it.</summary>
    [JsonPropertyName("reset")]
    public int ResetTimestamp { get; init; }

    /// <summary>
    /// Gets whether the current milestone's rewards have already been granted.
    /// </summary>
    [JsonPropertyName("claimed")]
    public bool RewardClaimed { get; init; }

    /// <summary>Rewards of each milestone, drawn when the track starts; one list per milestone.</summary>
    [JsonPropertyName("rewards")]
    public OrderTrackReward[][] Rewards { get; init; } = [];

    /// <summary>Gets milestone rewards awaiting delivery after the previous track expired.</summary>
    [JsonPropertyName("savedUnclaimedRewards")]
    public OrderTrackReward[][] SavedUnclaimedRewards { get; init; } = [];

    /// <summary>Points the current milestone needs; 0 when the track has no further milestone.</summary>
    [JsonPropertyName("target")]
    public int Target { get; init; }

    /// <summary>Global id of the track's table entry; 0 when no track runs.</summary>
    [JsonPropertyName("track")]
    public int TrackId { get; init; }
}
