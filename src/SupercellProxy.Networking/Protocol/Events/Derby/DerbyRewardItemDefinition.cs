using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes one item and its weight in an event's Derby reward choices.</summary>
public sealed record DerbyRewardItemDefinition
{
    /// <summary>Gets the item quantity.</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>Gets the case-sensitive asset row name.</summary>
    [JsonPropertyName("data")]
    public string? Data { get; init; }

    /// <summary>Gets the choice weight.</summary>
    [JsonPropertyName("probability")]
    public int Weight { get; init; }
}
