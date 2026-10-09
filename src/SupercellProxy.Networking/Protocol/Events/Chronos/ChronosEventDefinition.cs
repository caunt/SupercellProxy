using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Contains the participation gates shared by Chronos event definitions.</summary>
public sealed record ChronosEventDefinition
{
    /// <summary>Gets the configured player-level requirement.</summary>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }
}
