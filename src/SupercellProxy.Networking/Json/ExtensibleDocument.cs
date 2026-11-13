using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Json;

/// <summary>Preserves unrecognized JSON members inside the codec when a schema is only partially decoded.</summary>
public abstract record ExtensibleDocument
{
    /// <summary>Gets the names of retained members whose schemas are not decoded yet.</summary>
    /// <value>A snapshot of the JSON property names that had no matching member when the document was read; empty when every member is known. These members are written back unchanged on serialization.</value>
    [JsonIgnore]
    public IReadOnlyCollection<string> UnknownFields => [.. ExtensionData.Keys];

    [JsonExtensionData]
    [JsonInclude]
    internal IDictionary<string, JsonElement> ExtensionData { get; init; } = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
}
