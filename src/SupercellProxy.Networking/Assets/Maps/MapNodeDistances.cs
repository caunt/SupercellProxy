using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>Precomputed road distances from one Valley board node to the nodes around it.</summary>
public sealed record MapNodeDistances
{
    /// <summary>
    /// Nodes grouped by how many roads away they are: the first group is one road away, the second two roads away, and so on.
    /// Nodes missing from every group are out of the precomputed range.
    /// </summary>
    [JsonPropertyName("Dist")]
    public int[][] Groups { get; init; } = [];

    /// <summary>Node id the distances are measured from.</summary>
    [JsonPropertyName("Id")]
    public int Id { get; init; }
}
