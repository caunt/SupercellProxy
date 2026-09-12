using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Represents decoded FarmLayoutsSnapshot state.</summary>
public sealed record FarmLayoutsSnapshot : ExtensibleDocument
{
    /// <summary>Gets the Layouts value.</summary>
    [JsonPropertyName("Layouts")]
    public EncodedDocumentValue?[] Layouts { get; init; } = [];

}
