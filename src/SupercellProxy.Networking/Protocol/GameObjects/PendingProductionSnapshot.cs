using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Preserves a queued production record and its product identity.</summary>
public sealed record PendingProductionSnapshot : ExtensibleDocument
{
    /// <summary>Gets the product's native data identifier.</summary>
    [JsonPropertyName("ID")]
    public int DataGlobalIdentifier { get; init; }
}
