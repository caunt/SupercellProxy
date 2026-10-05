using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents item identifiers and quantities stored in a collection area.</summary>
public sealed record ItemQuantityListSnapshot
{
    /// <summary>Gets the pending item global identifiers.</summary>
    [JsonPropertyName("Donations")]
    public int[] ItemGlobalIdentifiers { get; init; } = [];

    /// <summary>Gets the pending item quantities.</summary>
    public int[] Amounts { get; init; } = [];
}
