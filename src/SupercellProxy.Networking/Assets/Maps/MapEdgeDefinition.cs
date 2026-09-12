using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>Represents the decoded MapEdgeDefinition JSON contract.</summary>
public sealed record MapEdgeDefinition
{
    /// <summary>Gets the End value.</summary>
    [JsonPropertyName("end")]
    public int End { get; init; }

    /// <summary>Gets the Start value.</summary>
    [JsonPropertyName("start")]
    public int Start { get; init; }

}
