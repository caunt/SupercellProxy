using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>A saved pair of decoration-event voting candidates.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationVotingPairSnapshot
{
    /// <summary>Gets or sets the pair's event identity.</summary>
    /// <value>The live event id of the decoration event the server offered this pair for.</value>
    [JsonPropertyName("eventId")]
    public int EventId { get; init; }

    /// <summary>Gets or sets the pair's event variant identity.</summary>
    /// <value>The variant id of that decoration event, as received with the pair.</value>
    [JsonPropertyName("eventVariantId")]
    public int EventVariantId { get; init; }

    /// <summary>Gets or sets the first candidate.</summary>
    /// <value>The candidate shown on the left (saved as <c>left</c>); a vote for it records selection 0.</value>
    [JsonPropertyName("left")]
    public DecorationVotingCandidateSnapshot Left { get; init; } = new();

    /// <summary>Gets or sets the second candidate.</summary>
    /// <value>The candidate shown on the right (saved as <c>right</c>); a vote for it records selection 1.</value>
    [JsonPropertyName("right")]
    public DecorationVotingCandidateSnapshot Right { get; init; } = new();
}
