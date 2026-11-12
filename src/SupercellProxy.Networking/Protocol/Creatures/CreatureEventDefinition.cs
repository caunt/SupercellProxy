using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>Describes the creature spawn rules selected by a seasonal event.</summary>
public sealed record CreatureEventDefinition
{
    /// <summary>Gets or sets the rule names from the creature-spawn-rules table, in native order.</summary>
    /// <value>The rule names read from <c>spawnRules</c>, each a row name of the creature spawn rules table. Empty by default.</value>
    [JsonPropertyName("spawnRules")]
    public string[] SpawnRules { get; init; } = [];
}
