using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>One location on a Valley board, such as a building plot, a gas station or an empty spot.</summary>
public sealed record MapNodeDefinition
{
    /// <summary>
    /// Name of the row in the map game nodes table that defines what stands here (for example <c>GasStation2x2</c>); that row carries
    /// the export variants and quest tags. Null for nodes without a table row.
    /// </summary>
    [JsonPropertyName("Data")]
    public string? Data { get; init; }

    /// <summary>Number that roads and precomputed distances use to refer to this location.</summary>
    [JsonPropertyName("Id")]
    public int Id { get; init; }
    /// <summary>Gets whether the node is a starting location for newly introduced Valley pawns.</summary>
    [JsonPropertyName("Start")]
    public bool IsStart { get; init; }

    /// <summary>
    /// Kind of location, such as <c>GasStation</c>; <c>Empty</c> marks a plot with nothing on it, which quests never count.
    /// </summary>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }

    /// <summary>
    /// Picks which of the node row's export variants is shown here, taken modulo the row's variant count; the chosen variant also
    /// decides the node's quest tags.
    /// </summary>
    [JsonPropertyName("Variant")]
    public int Variant { get; init; }
}
