using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>A road between two Valley board locations. The game treats it as two-way; the file stores each road once.</summary>
public sealed record MapEdgeDefinition
{
    /// <summary>Node id at the far end of the road.</summary>
    [JsonPropertyName("end")]
    public int End { get; init; }

    /// <summary>Node id the road leaves from.</summary>
    [JsonPropertyName("start")]
    public int Start { get; init; }
}
