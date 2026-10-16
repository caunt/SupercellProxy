using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>
/// The saved state of the player's part in the decoration (design) event: the design they submitted, its likes, and the intro
/// tutorial. Everything except the tutorial resets when a different decoration event starts.
/// </summary>
public sealed record DecorationEventManagerSnapshot
{
    /// <summary>
    /// How many event challenges the player had completed when they submitted their design (saved as <c>challengesOnSub</c>).
    /// </summary>
    [JsonPropertyName("challengesOnSub")]
    public int ChallengesOnSubmission { get; init; }

    /// <summary>
    /// Variant of the running decoration event on the player's event board (saved as <c>eventVariantId</c>).
    /// </summary>
    [JsonPropertyName("eventVariantId")]
    public int EventId { get; init; }

    /// <summary>
    /// Id of the decoration event this state belongs to (saved as <c>eventId</c>); when another event starts, the state resets.
    /// </summary>
    [JsonPropertyName("eventId")]
    public int EventOwnerId { get; init; }

    /// <summary>
    /// Group the player's design was placed in for featuring; 0 when it is not featured.
    /// </summary>
    [JsonPropertyName("featuringGroup")]
    public int FeaturingGroup { get; init; }

    /// <summary>
    /// The previous decoration event the player took part in.
    /// </summary>
    [JsonPropertyName("lastEventId")]
    public int LastEventId { get; init; }

    /// <summary>
    /// How far the player got in that previous event, as native numbers event states.
    /// </summary>
    [JsonPropertyName("lastEventState")]
    public int LastEventState { get; init; }

    /// <summary>
    /// Likes the player's submitted design has received.
    /// </summary>
    [JsonPropertyName("likes")]
    public int Likes { get; init; }

    /// <summary>
    /// Event whose rewards the player can still reclaim; 0 when nothing is pending.
    /// </summary>
    [JsonPropertyName("pendingReclaimEventId")]
    public int PendingReclaimEventId { get; init; }

    /// <summary>
    /// When the player submitted their design, as a Unix time stamp; 0 when not yet submitted.
    /// </summary>
    [JsonPropertyName("submissionTime")]
    public int SubmissionTime { get; init; }
    /// <summary>
    /// Progress through the decoration event's intro tutorial.
    /// </summary>
    [JsonPropertyName("tutorial")]
    public DecorationEventTutorialSnapshot? Tutorial { get; init; }
}
