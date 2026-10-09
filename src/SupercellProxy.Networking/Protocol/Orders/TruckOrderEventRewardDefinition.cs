using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Defines one resource in a truck event's custom weighted reward pool.</summary>
public sealed record TruckOrderEventRewardDefinition
{
    /// <summary>Gets the resource quantity.</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>Gets the native resource name.</summary>
    [JsonPropertyName("data")]
    public string Data { get; init; } = string.Empty;

    /// <summary>Gets the selection weight.</summary>
    [JsonPropertyName("probability")]
    public int Probability { get; init; }
}
