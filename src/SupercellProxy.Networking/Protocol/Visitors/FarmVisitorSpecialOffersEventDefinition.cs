using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Special farm-visitor trades supplied by an active game event.</summary>
public sealed record FarmVisitorSpecialOffersEventDefinition
{
    /// <summary>Gets or sets the event's ordered visitor trades.</summary>
    /// <value>Trades the event can offer, in event order. Empty by default.</value>
    public ImmutableArray<FarmVisitorSpecialOffer> Deliverables { get; init; } = [];

    /// <summary>Gets or sets the percentage chance of choosing a special trade.</summary>
    /// <value>Percent from 0 to 100, read from <c>probabilityOfRequestingSpecialItems</c>. 0 by default.</value>
    [JsonPropertyName("probabilityOfRequestingSpecialItems")]
    public int ProbabilityOfRequestingSpecialItems { get; init; }

    /// <summary>Gets or sets whether the event chooses a random trade.</summary>
    /// <value><see langword="true"/> if each visitor draws a random entry from <see cref="Deliverables"/>; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("random")]
    public bool RandomSelection { get; init; }

    /// <summary>Gets or sets whether a trade may be offered only once.</summary>
    /// <value><see langword="true"/> if each trade may appear only once for the farm; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    [JsonPropertyName("showOnlyOnce")]
    public bool ShowOnlyOnce { get; init; }
}
