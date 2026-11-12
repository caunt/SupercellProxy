using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Records which gifts have been collected from one calendar.</summary>
public sealed record CalendarGiftEventSnapshot
{
    /// <summary>Gets or sets the calendar event id.</summary>
    /// <value>The live event id this calendar is bound to, saved as <c>EventId</c>.</value>
    [JsonPropertyName("EventId")]
    public int EventId { get; init; }
    /// <summary>Gets or sets the collection flag for each gift in native order.</summary>
    /// <value>One flag per entry of the event's gift list, <see langword="true"/> once that gift was collected; empty by default.</value>
    public bool[] Gifts { get; init; } = [];
}
