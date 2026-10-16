using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>
/// A Valley board as stored in the <c>data/valley_maps</c> JSON files: the locations on the board, the roads between them and
/// precomputed road distances.
/// </summary>
public sealed record MapDefinition
{
    /// <summary>Road connections between locations; each one links two node ids and can be travelled both ways.</summary>
    [JsonPropertyName("Adjacency")]
    public MapEdgeDefinition[] Adjacency { get; init; } = [];

    /// <summary>
    /// Road distances from some starting nodes to every other node, precomputed by the map export. Null when the file does not
    /// include them, in which case distances have to be walked from <see cref="Adjacency"/>.
    /// </summary>
    [JsonPropertyName("Distances")]
    public MapNodeDistances[]? Distances { get; init; }

    /// <summary>Every location on the board: buildings, gas stations, empty plots and pawn start points.</summary>
    [JsonPropertyName("Nodes")]
    public MapNodeDefinition[] Nodes { get; init; } = [];
}
