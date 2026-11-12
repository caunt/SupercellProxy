using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Events;

/// <summary>Defines a resource and its weight in a boat event reward pool.</summary>
public sealed record BoatEventRewardDefinition
{
    /// <summary>Gets or sets the awarded quantity.</summary>
    /// <value>How many units of <see cref="Data"/> the entry rewards, read from <c>amount</c>.</value>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>Gets or sets the resource name.</summary>
    /// <value>The data-table name of the rewarded resource, read from <c>data</c>; empty by default.</value>
    [JsonPropertyName("data")]
    public string Data { get; init; } = string.Empty;

    /// <summary>Gets or sets the selection weight.</summary>
    /// <value>The entry's relative weight in the pool, read from <c>probability</c>.</value>
    [JsonPropertyName("probability")]
    public int Probability { get; init; }
}
