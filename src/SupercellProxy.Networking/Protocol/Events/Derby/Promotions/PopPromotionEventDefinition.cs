using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Promotions;

/// <summary>Describes the promotion event's weekly collection reset schedule.</summary>
public sealed record PopPromotionEventDefinition
{
    /// <summary>Gets the native weekday, with Monday represented by zero.</summary>
    [JsonPropertyName("popPromoWeeklyResetDay")]
    public int WeeklyResetDay { get; init; }
    /// <summary>Gets the configured reset hour in UTC.</summary>
    [JsonPropertyName("popPromoWeeklyResetHourUTC")]
    public int WeeklyResetHour { get; init; }
}
