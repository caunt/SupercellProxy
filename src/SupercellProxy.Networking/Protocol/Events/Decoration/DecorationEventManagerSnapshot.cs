using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>
/// The saved state of the player's part in the decoration (design) event: the design they submitted, its likes, and the intro
/// tutorial. Everything except the tutorial resets when a different decoration event starts.
/// </summary>
public sealed record DecorationEventManagerSnapshot
{
    /// <summary>
    /// Gets or sets how many event challenges the player had completed when they submitted their design (saved as <c>challengesOnSub</c>).
    /// </summary>
    /// <value>A challenge count captured at submission time; 0 before a submission.</value>
    [JsonPropertyName("challengesOnSub")]
    public int ChallengesOnSubmission { get; init; }

    /// <summary>
    /// Gets or sets the variant of the running decoration event on the player's event board (saved as <c>eventVariantId</c>).
    /// </summary>
    /// <value>The variant id of the board entry of the event named by <see cref="EventOwnerId"/>, or 0 when that event is not on the board; despite its name it holds a variant id, not an event id.</value>
    [JsonPropertyName("eventVariantId")]
    public int EventId { get; init; }

    /// <summary>
    /// Gets or sets the id of the decoration event this state belongs to (saved as <c>eventId</c>).
    /// </summary>
    /// <value>The live event id of the current decoration event (type 44), or 0 when none is running.</value>
    [JsonPropertyName("eventId")]
    public int EventOwnerId { get; init; }

    /// <summary>
    /// Gets or sets the group the player's design was placed in for featuring; 0 when it is not featured.
    /// </summary>
    /// <value>The featuring group number assigned to the player's design.</value>
    [JsonPropertyName("featuringGroup")]
    public int FeaturingGroup { get; init; }

    /// <summary>
    /// Gets or sets the previous decoration event the player took part in.
    /// </summary>
    /// <value>The live event id of that earlier event, or 0 when there is none.</value>
    [JsonPropertyName("lastEventId")]
    public int LastEventId { get; init; }

    /// <summary>
    /// Gets or sets how far the player got in that previous event, as native numbers event states.
    /// </summary>
    /// <value>The native state number recorded for <see cref="LastEventId"/>, or 0 when there is none; the meaning of the individual numbers is not yet identified.</value>
    [JsonPropertyName("lastEventState")]
    public int LastEventState { get; init; }

    /// <summary>
    /// Gets or sets the likes the player's submitted design has received.
    /// </summary>
    /// <value>The like count as last saved; 0 before a submission.</value>
    [JsonPropertyName("likes")]
    public int Likes { get; init; }

    /// <summary>
    /// Gets or sets the event whose rewards the player can still reclaim; 0 when nothing is pending.
    /// </summary>
    /// <value>A live event id, saved as <c>pendingReclaimEventId</c>.</value>
    [JsonPropertyName("pendingReclaimEventId")]
    public int PendingReclaimEventId { get; init; }

    /// <summary>
    /// Gets or sets when the player submitted their design, as a Unix time stamp; 0 when not yet submitted.
    /// </summary>
    /// <value>Seconds since the Unix epoch of the submission.</value>
    [JsonPropertyName("submissionTime")]
    public int SubmissionTime { get; init; }
    /// <summary>
    /// Gets or sets the progress through the decoration event's intro tutorial.
    /// </summary>
    /// <value>The last intro event and screen reached, or <see langword="null"/> when the save has no tutorial state.</value>
    [JsonPropertyName("tutorial")]
    public DecorationEventTutorialSnapshot? Tutorial { get; init; }
}
