using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Contains the participation gates shared by Chronos event definitions.</summary>
public sealed record ChronosEventDefinition
{
    /// <summary>Gets or sets the configured player-level requirement.</summary>
    /// <value>The minimum experience level needed to take part, or <see langword="null"/> when the definition sets no requirement.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }
}
