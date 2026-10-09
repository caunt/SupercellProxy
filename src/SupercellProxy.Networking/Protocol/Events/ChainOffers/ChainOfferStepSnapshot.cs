using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Retains the counters used by a chain step's requirements.</summary>
public sealed record ChainOfferStepSnapshot
{
    /// <summary>Gets accumulated gameplay progress.</summary>
    [JsonPropertyName("farmTaskAmount")]
    public int FarmTaskAmount { get; init; }
    /// <summary>Gets whether the step's reward sequence has been acknowledged.</summary>
    [JsonPropertyName("hasSeenSequence")]
    public bool HasSeenSequence { get; init; }
    /// <summary>Gets the last displayed gameplay progress.</summary>
    [JsonPropertyName("lastSeenAmount")]
    public int LastSeenAmount { get; init; }
    /// <summary>Gets when the preceding step was claimed.</summary>
    [JsonPropertyName("previousStepClaimTimestamp")]
    public int PreviousStepClaimTimestamp { get; init; }
}
