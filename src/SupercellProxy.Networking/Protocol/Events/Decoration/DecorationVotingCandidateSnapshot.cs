using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>One saved decoration-event voting candidate.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationVotingCandidateSnapshot
{
    /// <summary>Gets or sets the high 32 bits of the candidate's avatar identity.</summary>
    /// <value>The high word of the designer's 64-bit avatar id; combined with <see cref="AvatarIdLow"/> it identifies the candidate a vote names.</value>
    [JsonPropertyName("avatarId_hi")]
    public int AvatarIdHigh { get; init; }

    /// <summary>Gets or sets the low 32 bits of the candidate's avatar identity.</summary>
    /// <value>The low word of the designer's 64-bit avatar id, paired with <see cref="AvatarIdHigh"/>.</value>
    [JsonPropertyName("avatarId_lo")]
    public int AvatarIdLow { get; init; }

    /// <summary>Gets or sets the number of completed decoration challenges.</summary>
    /// <value>How many of the event's design challenges the designer had completed, as the server reported with the candidate.</value>
    [JsonPropertyName("challengesComplete")]
    public int ChallengesComplete { get; init; }

    /// <summary>Gets or sets the saved design JSON text.</summary>
    /// <value>The candidate's canvas design as an uncompressed JSON string that deserializes into a <see cref="DecorationCanvasDesignSnapshot"/>; empty when absent.</value>
    [JsonPropertyName("designData")]
    public string DesignData { get; init; } = string.Empty;

    /// <summary>Gets or sets the candidate's farm name.</summary>
    /// <value>The designer's farm name shown with the design (saved as <c>name</c>); empty when not saved.</value>
    [JsonPropertyName("name")]
    public string FarmName { get; init; } = string.Empty;

    /// <summary>Gets or sets the candidate's featuring group.</summary>
    /// <value>The featuring group number the server assigned to the design; 0 when the design is not featured.</value>
    [JsonPropertyName("featuringGroup")]
    public int FeaturingGroup { get; init; }

    /// <summary>Gets or sets the candidate's voting league.</summary>
    /// <value>The league number the server reported for the designer; its range is not yet identified.</value>
    [JsonPropertyName("league")]
    public int League { get; init; }

    /// <summary>Gets or sets the saved RNF like count.</summary>
    /// <value>The number of likes the server reported for the design (saved as <c>RNFLikes</c>); what the RNF prefix stands for is not yet identified.</value>
    [JsonPropertyName("RNFLikes")]
    public int RnfLikes { get; init; }
}
