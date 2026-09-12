using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents decoded MapGameEmblemSnapshot state.</summary>
public sealed record MapGameEmblemSnapshot : ExtensibleDocument
{
    /// <summary>Gets the Background value.</summary>
    [JsonPropertyName("bg")]
    public int Background { get; init; }

    /// <summary>Gets the Pattern value.</summary>
    [JsonPropertyName("pat")]
    public int Pattern { get; init; }

    /// <summary>Gets the Symbol value.</summary>
    [JsonPropertyName("sym")]
    public int Symbol { get; init; }

}
