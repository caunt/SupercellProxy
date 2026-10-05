using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Defines the dated gifts placed on Greg's farm.</summary>
public sealed record CalendarGiftEventDefinition
{
    /// <summary>Gets the native shop-event type for calendar gifts.</summary>
    public const int EventType = 14;
    /// <summary>Gets the gifts in their placement order.</summary>
    [JsonPropertyName("giftItems")]
    public CalendarGiftDefinition[] Gifts { get; init; } = [];
    /// <summary>Gets whether scenery below gift boxes is removed.</summary>
    [JsonPropertyName("removeObjectsUnderGifts")]
    public bool RemoveObjectsUnderGifts { get; init; }
    /// <summary>Gets the event's level requirement.</summary>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }
}
