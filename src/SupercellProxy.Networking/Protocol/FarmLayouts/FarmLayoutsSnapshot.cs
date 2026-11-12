using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Represents decoded FarmLayoutsSnapshot state.</summary>
public sealed record FarmLayoutsSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the player's saved farm layouts, one per layout slot; empty slots are <see langword="null"/>.</summary>
    /// <value>Up to five layout documents in slot order, each kept as its raw JSON; empty when the player has saved no layout.</value>
    [JsonPropertyName("Layouts")]
    public EncodedDocumentValue?[] Layouts { get; init; } = [];
}
