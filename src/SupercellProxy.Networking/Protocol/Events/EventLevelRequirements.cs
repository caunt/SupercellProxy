using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Describes the level required to participate in an event.</summary>
public sealed record EventLevelRequirements
{
    /// <summary>Lowest experience level a player needs to take part in the event.</summary>
    [JsonPropertyName("minLevel")]
    public int MinimumLevel { get; init; }
}
