using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Records which gifts have been collected from one calendar.</summary>
public sealed record CalendarGiftEventSnapshot
{
    /// <summary>Gets the calendar event identifier.</summary>
    [JsonPropertyName("EventId")]
    public int EventIdentifier { get; init; }
    /// <summary>Gets the collection flag for each gift in native order.</summary>
    public bool[] Gifts { get; init; } = [];
}
