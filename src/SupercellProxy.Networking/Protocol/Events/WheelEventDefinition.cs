using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Describes a special spin-wheel event: its prize configuration, how long the wheel car stays, and who may join.</summary>
public sealed record WheelEventDefinition
{
    /// <summary>Gets or sets the event's level requirement.</summary>
    /// <value>The <c>requirements</c> object of the event document, or <see langword="null"/> when the event sets no minimum level.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets or sets how long a closed wheel car stays on the farm before it drives off while this event runs.</summary>
    /// <value>A delay in seconds that replaces the wheel's <c>WaitUntilLeaveSeconds</c> table value; defaults to -1, which keeps the table delay.</value>
    [JsonPropertyName("waitUntilLeaveSeconds")]
    public int WaitUntilLeaveSeconds { get; init; } = -1;

    /// <summary>Gets or sets the prize setup the wheel uses while this event runs, instead of the week day's default setup.</summary>
    /// <value>The raw <c>wheelConfig</c> JSON, laid out like one day of the default wheel configuration, or <see langword="null"/> when the event keeps the default prizes.</value>
    [JsonPropertyName("wheelConfig")]
    public JsonElement? WheelConfiguration { get; init; }
}
