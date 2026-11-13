using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Retains the counters used by a chain step's requirements.</summary>
public sealed record ChainOfferStepSnapshot
{
    /// <summary>Gets or sets accumulated gameplay progress.</summary>
    /// <value>The count of matching farm-task actions recorded for this step; starts at 0.</value>
    [JsonPropertyName("farmTaskAmount")]
    public int FarmTaskAmount { get; init; }

    /// <summary>Gets or sets whether the step's reward sequence has been acknowledged.</summary>
    /// <value><see langword="true"/> if the player acknowledged the reward sequence of this claimed step; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("hasSeenSequence")]
    public bool HasSeenSequence { get; init; }

    /// <summary>Gets or sets the last displayed gameplay progress.</summary>
    /// <value>The farm-task amount the client last showed for this step, kept for its interface.</value>
    [JsonPropertyName("lastSeenAmount")]
    public int LastSeenAmount { get; init; }

    /// <summary>Gets or sets when the preceding step was claimed.</summary>
    /// <value>The Unix time in seconds at which the previous step of a StepByStep offer was claimed, or 0 when it has not been claimed.</value>
    [JsonPropertyName("previousStepClaimTimestamp")]
    public int PreviousStepClaimTimestamp { get; init; }
}
