using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>One location on a Valley board, such as a building plot, a gas station or an empty spot.</summary>
public sealed record MapNodeDefinition
{
    /// <summary>
    /// Gets or sets the name of the row in the map game nodes table that defines what stands here (for example <c>GasStation2x2</c>);
    /// that row carries the export variants and quest tags.
    /// </summary>
    /// <value>The row name exactly as written in the JSON <c>Data</c> field; <see langword="null"/> for nodes without a table row.</value>
    [JsonPropertyName("Data")]
    public string? Data { get; init; }

    /// <summary>Gets or sets the number that roads and precomputed distances use to refer to this location.</summary>
    /// <value>The node's id within this board, read from the JSON <c>Id</c> field and matched by
    /// <see cref="MapEdgeDefinition.Start"/>, <see cref="MapEdgeDefinition.End"/> and <see cref="MapNodeDistances.Id"/>.</value>
    [JsonPropertyName("Id")]
    public int Id { get; init; }

    /// <summary>Gets or sets whether the node is a starting location for newly introduced Valley pawns.</summary>
    /// <value><see langword="true"/> if the JSON <c>Start</c> field is present and marks this node as a pawn start point; otherwise,
    /// <see langword="false"/>.</value>
    [JsonPropertyName("Start")]
    public bool IsStart { get; init; }

    /// <summary>
    /// Gets or sets the kind of location, such as <c>GasStation</c>; <c>Empty</c> marks a plot with nothing on it.
    /// </summary>
    /// <value>The type name as written in the JSON <c>Type</c> field; <see langword="null"/> when the file leaves it out.</value>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }

    /// <summary>
    /// Gets or sets which of the node row's export variants is shown here, taken modulo the row's variant count; the chosen variant
    /// also decides the node's quest tags.
    /// </summary>
    /// <value>The index as stored in the JSON <c>Variant</c> field; values past the row's variant count wrap around, and 0 (the
    /// default when the field is absent) picks the first variant.</value>
    [JsonPropertyName("Variant")]
    public int Variant { get; init; }
}
