using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>Describes the creature spawn rules selected by a seasonal event.</summary>
public sealed record CreatureEventDefinition
{
    /// <summary>Gets rule names from the creature-spawn-rules table, in native order.</summary>
    [JsonPropertyName("spawnRules")]
    public string[] SpawnRules { get; init; } = [];
}
