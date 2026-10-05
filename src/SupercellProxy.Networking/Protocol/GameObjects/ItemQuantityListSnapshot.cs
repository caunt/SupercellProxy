using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents item ids and quantities stored in a collection area.</summary>
public sealed record ItemQuantityListSnapshot
{
    /// <summary>Gets the pending item global ids.</summary>
    [JsonPropertyName("Donations")]
    public int[] ItemGlobalIds { get; init; } = [];

    /// <summary>Gets the pending item quantities.</summary>
    public int[] Amounts { get; init; } = [];
}
