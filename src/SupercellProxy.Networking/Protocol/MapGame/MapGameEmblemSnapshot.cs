using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>A player's saved Valley emblem, built from a background, a pattern and a symbol.</summary>
public sealed record MapGameEmblemSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the emblem background the player picked (saved as <c>bg</c>).</summary>
    /// <value>The number of the chosen background option; 0 by default.</value>
    [JsonPropertyName("bg")]
    public int Background { get; init; }

    /// <summary>Gets or sets the emblem pattern the player picked (saved as <c>pat</c>).</summary>
    /// <value>The number of the chosen pattern drawn over the background; 0 by default.</value>
    [JsonPropertyName("pat")]
    public int Pattern { get; init; }

    /// <summary>Gets or sets the emblem symbol the player picked (saved as <c>sym</c>).</summary>
    /// <value>The number of the chosen symbol shown on top of the pattern; 0 by default.</value>
    [JsonPropertyName("sym")]
    public int Symbol { get; init; }
}
