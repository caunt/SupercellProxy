using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>A saved pair of decoration-event voting candidates.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DecorationVotingPairSnapshot
{
    /// <summary>Gets the pair's event identity.</summary>
    [JsonPropertyName("eventId")]
    public int EventId { get; init; }

    /// <summary>Gets the pair's event variant identity.</summary>
    [JsonPropertyName("eventVariantId")]
    public int EventVariantId { get; init; }

    /// <summary>Gets the first candidate.</summary>
    [JsonPropertyName("left")]
    public DecorationVotingCandidateSnapshot Left { get; init; } = new();

    /// <summary>Gets the second candidate.</summary>
    [JsonPropertyName("right")]
    public DecorationVotingCandidateSnapshot Right { get; init; } = new();
}
