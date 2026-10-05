using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Retains the calendar gifts already collected by the player.</summary>
public sealed record CalendarGiftManagerSnapshot
{
    /// <summary>Gets the saved calendar event states.</summary>
    [JsonPropertyName("eventStates")]
    public CalendarGiftEventSnapshot[] Events { get; init; } = [];
}
