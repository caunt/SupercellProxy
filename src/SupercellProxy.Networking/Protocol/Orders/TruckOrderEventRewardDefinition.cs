using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Defines one resource in a truck event's custom weighted reward pool.</summary>
public sealed record TruckOrderEventRewardDefinition
{
    /// <summary>Gets or sets the resource quantity.</summary>
    /// <value>Saved as <c>amount</c>; how many of the resource the entry gives.</value>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>Gets or sets the native resource name.</summary>
    /// <value>Saved as <c>data</c>; a table row name of any grantable item. Empty by default.</value>
    [JsonPropertyName("data")]
    public string Data { get; init; } = string.Empty;

    /// <summary>Gets or sets the selection weight.</summary>
    /// <value>Saved as <c>probability</c>; a relative weight among the entries, not a percentage.</value>
    [JsonPropertyName("probability")]
    public int Probability { get; init; }
}
