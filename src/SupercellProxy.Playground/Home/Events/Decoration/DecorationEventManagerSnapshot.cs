using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

internal sealed record DecorationEventManagerSnapshot
{
    [JsonPropertyName("tutorial")]
    public DecorationEventTutorialSnapshot? Tutorial { get; init; }

    [JsonPropertyName("challengesOnSub")]
    public int ChallengesOnSubmission { get; init; }

    [JsonPropertyName("submissionTime")]
    public int SubmissionTime { get; init; }

    [JsonPropertyName("lastEventId")]
    public int LastEventId { get; init; }

    [JsonPropertyName("eventId")]
    public int EventOwnerId { get; init; }

    [JsonPropertyName("eventVariantId")]
    public int EventIdentifier { get; init; }

    [JsonPropertyName("likes")]
    public int Likes { get; init; }

    [JsonPropertyName("featuringGroup")]
    public int FeaturingGroup { get; init; }

    [JsonPropertyName("pendingReclaimEventId")]
    public int PendingReclaimEventId { get; init; }

    [JsonPropertyName("lastEventState")]
    public int LastEventState { get; init; }
}
