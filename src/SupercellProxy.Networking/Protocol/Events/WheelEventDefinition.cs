using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Describes a special spin-wheel event: its prize configuration, how long the wheel car stays, and who may join.</summary>
public sealed record WheelEventDefinition
{
    /// <summary>Gets the event's minimum player level.</summary>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets how long a closed wheel car stays before leaving, or -1 to keep the wheel's own delay.</summary>
    [JsonPropertyName("waitUntilLeaveSeconds")]
    public int WaitUntilLeaveSeconds { get; init; } = -1;

    /// <summary>Gets the prize configuration, in the same layout as one day of the default wheel configuration.</summary>
    [JsonPropertyName("wheelConfig")]
    public JsonElement? WheelConfiguration { get; init; }
}
