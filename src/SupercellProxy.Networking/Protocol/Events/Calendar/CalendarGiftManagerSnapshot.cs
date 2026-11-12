using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Retains the calendar gifts already collected by the player.</summary>
public sealed record CalendarGiftManagerSnapshot
{
    /// <summary>Gets or sets the saved calendar event states.</summary>
    /// <value>The states saved as <c>eventStates</c>, in saved order; empty by default.</value>
    [JsonPropertyName("eventStates")]
    public CalendarGiftEventSnapshot[] Events { get; init; } = [];
}
