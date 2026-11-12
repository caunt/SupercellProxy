using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>
/// A Valley board as stored in the <c>data/valley_maps</c> JSON files: the locations on the board, the roads between them and
/// precomputed road distances.
/// </summary>
public sealed record MapDefinition
{
    /// <summary>Gets or sets the road connections between locations; each one links two node ids and can be travelled both ways.</summary>
    /// <value>Each road once, read from the file's <c>Adjacency</c> array; empty when the file lists no roads.</value>
    [JsonPropertyName("Adjacency")]
    public MapEdgeDefinition[] Adjacency { get; init; } = [];

    /// <summary>
    /// Gets or sets the road distances from some starting nodes to every other node, precomputed by the map export.
    /// </summary>
    /// <value>One entry per source node that has precomputed distances; <see langword="null"/> when the file does not include them,
    /// in which case distances have to be walked from <see cref="Adjacency"/>.</value>
    [JsonPropertyName("Distances")]
    public MapNodeDistances[]? Distances { get; init; }

    /// <summary>Gets or sets every location on the board: buildings, gas stations, empty plots and pawn start points.</summary>
    /// <value>The nodes in file order, each identified by <see cref="MapNodeDefinition.Id"/>; empty when the file lists none.</value>
    [JsonPropertyName("Nodes")]
    public MapNodeDefinition[] Nodes { get; init; } = [];
}
