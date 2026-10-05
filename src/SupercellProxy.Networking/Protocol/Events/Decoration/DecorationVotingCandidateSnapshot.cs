using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>One saved decoration-event voting candidate.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationVotingCandidateSnapshot
{
    /// <summary>Gets the high 32 bits of the candidate's avatar identity.</summary>
    [JsonPropertyName("avatarId_hi")]
    public int AvatarIdHigh { get; init; }

    /// <summary>Gets the low 32 bits of the candidate's avatar identity.</summary>
    [JsonPropertyName("avatarId_lo")]
    public int AvatarIdLow { get; init; }

    /// <summary>Gets the number of completed decoration challenges.</summary>
    [JsonPropertyName("challengesComplete")]
    public int ChallengesComplete { get; init; }

    /// <summary>Gets the saved design JSON text.</summary>
    [JsonPropertyName("designData")]
    public string DesignData { get; init; } = string.Empty;

    /// <summary>Gets the candidate's farm name.</summary>
    [JsonPropertyName("name")]
    public string FarmName { get; init; } = string.Empty;

    /// <summary>Gets the candidate's featuring group.</summary>
    [JsonPropertyName("featuringGroup")]
    public int FeaturingGroup { get; init; }

    /// <summary>Gets the candidate's voting league.</summary>
    [JsonPropertyName("league")]
    public int League { get; init; }

    /// <summary>Gets the saved RNF like count.</summary>
    [JsonPropertyName("RNFLikes")]
    public int RnfLikes { get; init; }
}
