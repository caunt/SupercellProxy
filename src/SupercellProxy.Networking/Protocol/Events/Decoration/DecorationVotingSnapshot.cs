using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Saved decoration-event voting counters and presentation state.</summary>
public sealed record DecorationVotingSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the voting event identity.</summary>
    /// <value>The live event id the saved voting state belongs to (saved as <c>eid</c>).</value>
    [JsonPropertyName("eid")]
    public int EventId { get; init; }

    /// <summary>Gets or sets the voting event variant identity.</summary>
    /// <value>The variant id of that event (saved as <c>evid</c>).</value>
    [JsonPropertyName("evid")]
    public int EventVariantId { get; init; }

    /// <summary>Gets or sets the featuring state.</summary>
    /// <value>A native featuring number; its values are not yet identified.</value>
    [JsonPropertyName("featuring")]
    public int Featuring { get; init; }

    /// <summary>Gets or sets the player's voting league.</summary>
    /// <value>The league number saved for the player.</value>
    [JsonPropertyName("league")]
    public int League { get; init; }

    /// <summary>Gets or sets the recorded like count.</summary>
    /// <value>The number of likes saved for the player's own design.</value>
    [JsonPropertyName("likes")]
    public int Likes { get; init; }

    /// <summary>Gets or sets the last login timestamp.</summary>
    /// <value>Unix seconds of the login saved with the voting state (saved as <c>loginTimeS</c>).</value>
    [JsonPropertyName("loginTimeS")]
    public int LoginTimestamp { get; init; }

    /// <summary>Gets or sets the current candidate pair, when saved.</summary>
    /// <value>The pair the player can vote on next, or <see langword="null"/> when none is waiting.</value>
    [JsonPropertyName("pair")]
    public DecorationVotingPairSnapshot? Pair { get; init; }

    /// <summary>Gets or sets the vote-quota reset timestamp.</summary>
    /// <value>Unix seconds at which an exhausted vote quota resets (saved as <c>quotaResetTimeS</c>); 0 while the quota is not exhausted.</value>
    [JsonPropertyName("quotaResetTimeS")]
    public int QuotaResetTimestamp { get; init; }

    /// <summary>Gets or sets the likes already shown in a banner.</summary>
    /// <value>The like count the last likes banner displayed, kept so the client only announces newer likes.</value>
    [JsonPropertyName("shownBannerLikes")]
    public int ShownBannerLikes { get; init; }

    /// <summary>Gets or sets the number of likes-banner presentations.</summary>
    /// <value>How many times the client has shown the likes banner; display bookkeeping only.</value>
    [JsonPropertyName("shownBannerLikesTimes")]
    public int ShownBannerLikesTimes { get; init; }

    /// <summary>Gets or sets the Unix seconds when the client last showed the player the banner announcing their design's likes.</summary>
    /// <value>Saved as <c>shownBannerLikesEpochS</c>; 0 when the banner has not been shown.</value>
    [JsonPropertyName("shownBannerLikesEpochS")]
    public int ShownBannerLikesTimestamp { get; init; }

    /// <summary>Gets or sets the event phase whose announcement banner the client has already shown, so it is not shown again.</summary>
    /// <value>A native phase number, presumably on the same scale as the event's phases (1 before designing, 2 designing, 4 voting, 5 finished), though this is not verified.</value>
    [JsonPropertyName("shownBannerPhase")]
    public int ShownBannerPhase { get; init; }

    /// <summary>Gets or sets the likes already presented to the player.</summary>
    /// <value>The like count the client last displayed outside the banner, kept for its interface.</value>
    [JsonPropertyName("shownLikes")]
    public int ShownLikes { get; init; }

    /// <summary>Gets or sets the number of submission-deadline banner presentations.</summary>
    /// <value>How many times the client warned the player that the design submission deadline is near; display bookkeeping only.</value>
    [JsonPropertyName("shownSubmissionDeadlineBannerTimes")]
    public int ShownSubmissionDeadlineBannerTimes { get; init; }

    /// <summary>Gets or sets the voting unlock timestamp.</summary>
    /// <value>The saved <c>unlockTime</c>, presumably Unix seconds at which voting opened for the player.</value>
    [JsonPropertyName("unlockTime")]
    public int UnlockTimestamp { get; init; }

    /// <summary>Gets or sets the remaining voting quota.</summary>
    /// <value>How many votes the player may still cast in the current batch.</value>
    [JsonPropertyName("voteQuota")]
    public int VoteQuota { get; init; }

    /// <summary>Gets or sets the saved vote selection.</summary>
    /// <value>Which candidate of <see cref="VotedPair"/> the player chose: 0 for the left candidate, 1 for the right.</value>
    [JsonPropertyName("voteSelection")]
    public int VoteSelection { get; init; }

    /// <summary>Gets or sets the recorded vote count.</summary>
    /// <value>The total number of votes the player has cast in the event.</value>
    [JsonPropertyName("voted")]
    public int Voted { get; init; }

    /// <summary>Gets or sets the last pair on which the player voted.</summary>
    /// <value>The pair the latest vote was cast on, or <see langword="null"/> before the first vote.</value>
    [JsonPropertyName("votedPair")]
    public DecorationVotingPairSnapshot? VotedPair { get; init; }
}
