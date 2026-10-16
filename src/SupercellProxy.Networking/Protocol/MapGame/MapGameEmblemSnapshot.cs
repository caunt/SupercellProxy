using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>A player's saved Valley emblem, built from a background, a pattern and a symbol.</summary>
public sealed record MapGameEmblemSnapshot : ExtensibleDocument
{
    /// <summary>Emblem background the player picked (saved as <c>bg</c>).</summary>
    [JsonPropertyName("bg")]
    public int Background { get; init; }

    /// <summary>Emblem pattern the player picked (saved as <c>pat</c>).</summary>
    [JsonPropertyName("pat")]
    public int Pattern { get; init; }

    /// <summary>Emblem symbol the player picked (saved as <c>sym</c>).</summary>
    [JsonPropertyName("sym")]
    public int Symbol { get; init; }
}
