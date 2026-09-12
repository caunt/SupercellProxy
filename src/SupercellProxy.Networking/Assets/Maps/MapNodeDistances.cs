using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>Represents the decoded MapNodeDistances JSON contract.</summary>
public sealed record MapNodeDistances
{
    /// <summary>Gets the Groups value.</summary>
    [JsonPropertyName("Dist")]
    public int[][] Groups { get; init; } = [];

    /// <summary>Gets the Identifier value.</summary>
    [JsonPropertyName("Id")]
    public int Identifier { get; init; }

}
