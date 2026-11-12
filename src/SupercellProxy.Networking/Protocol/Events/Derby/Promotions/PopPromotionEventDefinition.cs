using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Promotions;

/// <summary>Describes the promotion event's weekly collection reset schedule.</summary>
public sealed record PopPromotionEventDefinition
{
    /// <summary>Gets or sets the native weekday, with Monday represented by zero.</summary>
    /// <value>The <c>popPromoWeeklyResetDay</c> weekday on which the weekly promotion counters reset, from 0 for Monday to 6 for Sunday; 0 when the event omits it.</value>
    [JsonPropertyName("popPromoWeeklyResetDay")]
    public int WeeklyResetDay { get; init; }
    /// <summary>Gets or sets the configured reset hour in UTC.</summary>
    /// <value>The <c>popPromoWeeklyResetHourUTC</c> hour of <see cref="WeeklyResetDay"/>, from 0 to 23 in UTC, at which the reset happens.</value>
    [JsonPropertyName("popPromoWeeklyResetHourUTC")]
    public int WeeklyResetHour { get; init; }
}
