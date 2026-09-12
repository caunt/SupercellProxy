using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>
/// Defines the Decoration Event Manager Snapshot contract.
/// </summary>
public sealed record DecorationEventManagerSnapshot
{

    /// <summary>
    /// Gets the Challenges On Submission value.
    /// </summary>
    [JsonPropertyName("challengesOnSub")]
    public int ChallengesOnSubmission { get; init; }

    /// <summary>
    /// Gets the Event Identifier value.
    /// </summary>
    [JsonPropertyName("eventVariantId")]
    public int EventIdentifier { get; init; }

    /// <summary>
    /// Gets the Event Owner Id value.
    /// </summary>
    [JsonPropertyName("eventId")]
    public int EventOwnerIdentifier { get; init; }

    /// <summary>
    /// Gets the Featuring Group value.
    /// </summary>
    [JsonPropertyName("featuringGroup")]
    public int FeaturingGroup { get; init; }

    /// <summary>
    /// Gets the Last Event Id value.
    /// </summary>
    [JsonPropertyName("lastEventId")]
    public int LastEventIdentifier { get; init; }

    /// <summary>
    /// Gets the Last Event State value.
    /// </summary>
    [JsonPropertyName("lastEventState")]
    public int LastEventState { get; init; }

    /// <summary>
    /// Gets the Likes value.
    /// </summary>
    [JsonPropertyName("likes")]
    public int Likes { get; init; }

    /// <summary>
    /// Gets the Pending Reclaim Event Id value.
    /// </summary>
    [JsonPropertyName("pendingReclaimEventId")]
    public int PendingReclaimEventIdentifier { get; init; }

    /// <summary>
    /// Gets the Submission Time value.
    /// </summary>
    [JsonPropertyName("submissionTime")]
    public int SubmissionTime { get; init; }
    /// <summary>
    /// Gets the Tutorial value.
    /// </summary>
    [JsonPropertyName("tutorial")]
    public DecorationEventTutorialSnapshot? Tutorial { get; init; }
}
