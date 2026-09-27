using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Saved decoration-event voting counters and presentation state.</summary>
public sealed record DecorationVotingSnapshot : ExtensibleDocument
{
    /// <summary>Gets the voting event identity.</summary>
    [JsonPropertyName("eid")]
    public int EventIdentifier { get; init; }

    /// <summary>Gets the voting event variant identity.</summary>
    [JsonPropertyName("evid")]
    public int EventVariantIdentifier { get; init; }

    /// <summary>Gets the featuring state.</summary>
    [JsonPropertyName("featuring")]
    public int Featuring { get; init; }

    /// <summary>Gets the player's voting league.</summary>
    [JsonPropertyName("league")]
    public int League { get; init; }

    /// <summary>Gets the recorded like count.</summary>
    [JsonPropertyName("likes")]
    public int Likes { get; init; }

    /// <summary>Gets the last login timestamp.</summary>
    [JsonPropertyName("loginTimeS")]
    public int LoginTimestamp { get; init; }

    /// <summary>Gets the current candidate pair, when saved.</summary>
    [JsonPropertyName("pair")]
    public DecorationVotingPairSnapshot? Pair { get; init; }

    /// <summary>Gets the vote-quota reset timestamp.</summary>
    [JsonPropertyName("quotaResetTimeS")]
    public int QuotaResetTimestamp { get; init; }

    /// <summary>Gets the likes already shown in a banner.</summary>
    [JsonPropertyName("shownBannerLikes")]
    public int ShownBannerLikes { get; init; }

    /// <summary>Gets the number of likes-banner presentations.</summary>
    [JsonPropertyName("shownBannerLikesTimes")]
    public int ShownBannerLikesTimes { get; init; }

    /// <summary>Gets the timestamp of the shown likes banner.</summary>
    [JsonPropertyName("shownBannerLikesEpochS")]
    public int ShownBannerLikesTimestamp { get; init; }

    /// <summary>Gets the shown banner phase.</summary>
    [JsonPropertyName("shownBannerPhase")]
    public int ShownBannerPhase { get; init; }

    /// <summary>Gets the likes already presented to the player.</summary>
    [JsonPropertyName("shownLikes")]
    public int ShownLikes { get; init; }

    /// <summary>Gets the number of submission-deadline banner presentations.</summary>
    [JsonPropertyName("shownSubmissionDeadlineBannerTimes")]
    public int ShownSubmissionDeadlineBannerTimes { get; init; }

    /// <summary>Gets the voting unlock timestamp.</summary>
    [JsonPropertyName("unlockTime")]
    public int UnlockTimestamp { get; init; }

    /// <summary>Gets the remaining voting quota.</summary>
    [JsonPropertyName("voteQuota")]
    public int VoteQuota { get; init; }

    /// <summary>Gets the saved vote selection.</summary>
    [JsonPropertyName("voteSelection")]
    public int VoteSelection { get; init; }

    /// <summary>Gets the recorded vote count.</summary>
    [JsonPropertyName("voted")]
    public int Voted { get; init; }

    /// <summary>Gets the last pair on which the player voted.</summary>
    [JsonPropertyName("votedPair")]
    public DecorationVotingPairSnapshot? VotedPair { get; init; }
}
