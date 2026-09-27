using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Special farm-visitor trades supplied by an active game event.</summary>
public sealed record FarmVisitorSpecialOffersEventDefinition
{
    /// <summary>Gets the event's ordered visitor trades.</summary>
    public FarmVisitorSpecialOffer[] Deliverables { get; init; } = [];

    /// <summary>Gets the percentage chance of choosing a special trade.</summary>
    [JsonPropertyName("probabilityOfRequestingSpecialItems")]
    public int ProbabilityOfRequestingSpecialItems { get; init; }

    /// <summary>Gets whether the event chooses a random trade.</summary>
    [JsonPropertyName("random")]
    public bool RandomSelection { get; init; }

    /// <summary>Gets whether a trade may be offered only once.</summary>
    [JsonPropertyName("showOnlyOnce")]
    public bool ShowOnlyOnce { get; init; }
}
