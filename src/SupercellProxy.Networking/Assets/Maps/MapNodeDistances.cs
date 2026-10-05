using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>Represents the decoded MapNodeDistances JSON contract.</summary>
public sealed record MapNodeDistances
{
    /// <summary>Gets the Groups value.</summary>
    [JsonPropertyName("Dist")]
    public int[][] Groups { get; init; } = [];

    /// <summary>Gets the Id value.</summary>
    [JsonPropertyName("Id")]
    public int Id { get; init; }
}
