using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes one item and its weight in an event's Derby reward choices.</summary>
public sealed record DerbyRewardItemDefinition
{
    /// <summary>Gets or sets the item quantity.</summary>
    /// <value>The <c>amount</c> of the item this choice gives; 0 when the JSON omits it.</value>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>Gets or sets the case-sensitive asset row name.</summary>
    /// <value>The <c>data</c> string naming the rewarded item's asset row, matched exactly against the asset tables, or <see langword="null"/> when the JSON omits it.</value>
    [JsonPropertyName("data")]
    public string? Data { get; init; }

    /// <summary>Gets or sets the choice weight.</summary>
    /// <value>The relative weight read from the <c>probability</c> JSON field, relative to the other choices of the same reward; 0 when the field is omitted.</value>
    [JsonPropertyName("probability")]
    public int Weight { get; init; }
}
