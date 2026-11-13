using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Defines the dated gifts placed on Greg's farm.</summary>
public sealed record CalendarGiftEventDefinition
{
    /// <summary>Gets the native shop-event type for calendar gifts.</summary>
    public const int EventType = 14;

    /// <summary>Gets or sets the gifts in their placement order.</summary>
    /// <value>One definition per box, read from <c>giftItems</c>; a gift's position in this list is its index in the saved collection flags.</value>
    [JsonPropertyName("giftItems")]
    public ImmutableArray<CalendarGiftDefinition> Gifts { get; init; } = [];

    /// <summary>Gets or sets a value indicating whether scenery below gift boxes is removed.</summary>
    /// <value><see langword="true"/> if objects under each box's footprint are cleared before the box is placed; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("removeObjectsUnderGifts")]
    public bool RemoveObjectsUnderGifts { get; init; }

    /// <summary>Gets or sets the event's level requirement.</summary>
    /// <value>The requirements read from <c>requirements</c>, or <see langword="null"/> for none.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }
}
