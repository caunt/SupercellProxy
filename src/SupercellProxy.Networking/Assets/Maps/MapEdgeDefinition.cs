using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>A road between two Valley board locations. The game treats it as two-way; the file stores each road once.</summary>
public sealed record MapEdgeDefinition
{
    /// <summary>Gets or sets the node id at the far end of the road.</summary>
    /// <value>A <see cref="MapNodeDefinition.Id"/> read from the JSON <c>end</c> field; 0 when the field is absent.</value>
    [JsonPropertyName("end")]
    public int End { get; init; }

    /// <summary>Gets or sets the node id the road leaves from.</summary>
    /// <value>A <see cref="MapNodeDefinition.Id"/> read from the JSON <c>start</c> field; since roads are two-way, which end is the
    /// start only reflects how the file stores it.</value>
    [JsonPropertyName("start")]
    public int Start { get; init; }
}
