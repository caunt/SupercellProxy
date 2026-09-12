using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Maps;

/// <summary>Represents the decoded MapNodeDefinition JSON contract.</summary>
public sealed record MapNodeDefinition
{
    /// <summary>Gets the Data value.</summary>
    [JsonPropertyName("Data")]
    public string? Data { get; init; }

    /// <summary>Gets the Identifier value.</summary>
    [JsonPropertyName("Id")]
    public int Identifier { get; init; }

    /// <summary>Gets the Type value.</summary>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }

    /// <summary>Gets the Variant value.</summary>
    [JsonPropertyName("Variant")]
    public int Variant { get; init; }

}
