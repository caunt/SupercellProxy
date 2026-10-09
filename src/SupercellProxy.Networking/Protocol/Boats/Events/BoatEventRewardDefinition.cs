using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Defines a resource and its weight in a boat event reward pool.</summary>
public sealed record BoatEventRewardDefinition
{
    /// <summary>Gets the awarded quantity.</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>Gets the resource name.</summary>
    [JsonPropertyName("data")]
    public string Data { get; init; } = string.Empty;

    /// <summary>Gets the selection weight.</summary>
    [JsonPropertyName("probability")]
    public int Probability { get; init; }
}
