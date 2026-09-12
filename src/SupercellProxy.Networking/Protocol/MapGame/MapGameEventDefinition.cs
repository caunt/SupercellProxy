using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents the decoded MapGameEventDefinition JSON contract.</summary>
public sealed record MapGameEventDefinition
{
    /// <summary>Gets the Pause value.</summary>
    [JsonPropertyName("pause")]
    public bool Pause { get; init; }

}
