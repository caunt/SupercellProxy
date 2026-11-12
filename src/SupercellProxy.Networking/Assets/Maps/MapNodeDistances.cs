using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>Precomputed road distances from one Valley board node to the nodes around it.</summary>
public sealed record MapNodeDistances
{
    /// <summary>
    /// Gets or sets the nodes grouped by how many roads away they are: the first group is one road away, the second two roads away,
    /// and so on.
    /// </summary>
    /// <value>Node ids from the JSON <c>Dist</c> array, where the group at index <c>n</c> lists the nodes <c>n + 1</c> roads away; nodes
    /// missing from every group are out of the precomputed range.</value>
    [JsonPropertyName("Dist")]
    public int[][] Groups { get; init; } = [];

    /// <summary>Gets or sets the node id the distances are measured from.</summary>
    /// <value>The <see cref="MapNodeDefinition.Id"/> of the source node, read from the JSON <c>Id</c> field; the
    /// <see cref="Groups"/> count roads from this node.</value>
    [JsonPropertyName("Id")]
    public int Id { get; init; }
}
