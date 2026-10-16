using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Represents decoded FarmLayoutsSnapshot state.</summary>
public sealed record FarmLayoutsSnapshot : ExtensibleDocument
{
    /// <summary>The player's saved farm layouts, one per layout slot; empty slots are null.</summary>
    [JsonPropertyName("Layouts")]
    public EncodedDocumentValue?[] Layouts { get; init; } = [];
}
