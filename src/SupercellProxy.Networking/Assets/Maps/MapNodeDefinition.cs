using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>Represents the decoded MapNodeDefinition JSON contract.</summary>
public sealed record MapNodeDefinition
{
    /// <summary>Gets the Data value.</summary>
    [JsonPropertyName("Data")]
    public string? Data { get; init; }

    /// <summary>Gets the Id value.</summary>
    [JsonPropertyName("Id")]
    public int Id { get; init; }
    /// <summary>Gets whether the node is a starting location for newly introduced Valley pawns.</summary>
    [JsonPropertyName("Start")]
    public bool IsStart { get; init; }

    /// <summary>Gets the Type value.</summary>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }

    /// <summary>Gets the Variant value.</summary>
    [JsonPropertyName("Variant")]
    public int Variant { get; init; }
}
