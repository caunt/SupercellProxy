using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Describes the level required to participate in an event.</summary>
public sealed record EventLevelRequirements
{
    /// <summary>Gets or sets the lowest experience level a player needs to take part in the event.</summary>
    /// <value>The <c>minLevel</c> from the event JSON; 0 when the event omits it.</value>
    [JsonPropertyName("minLevel")]
    public int MinimumLevel { get; init; }
}
