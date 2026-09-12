using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>Represents the decoded MapDefinition JSON contract.</summary>
public sealed record MapDefinition
{
    /// <summary>Gets the Adjacency value.</summary>
    [JsonPropertyName("Adjacency")]
    public MapEdgeDefinition[] Adjacency { get; init; } = [];

    /// <summary>Gets the Distances value.</summary>
    [JsonPropertyName("Distances")]
    public MapNodeDistances[]? Distances { get; init; }

    /// <summary>Gets the Nodes value.</summary>
    [JsonPropertyName("Nodes")]
    public MapNodeDefinition[] Nodes { get; init; } = [];

}
