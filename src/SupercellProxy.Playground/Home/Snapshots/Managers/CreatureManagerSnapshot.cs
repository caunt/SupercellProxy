using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>CreatureManagerSnapshot</c> home data.
/// </summary>
public sealed record CreatureManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c>DailyMaxSpawnResetTime</c> value.
    /// </summary>
    [JsonPropertyName("dailyMaxSpawnResetTime")]
    public int DailyMaxSpawnResetTime { get; init; }

    /// <summary>
    /// Gets or sets the <c>FarmVisitingCatchList</c> value.
    /// </summary>
    [JsonPropertyName("farmVisitingCatchList")]
    public JsonElement[] FarmVisitingCatchList { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>LastKnownEventId</c> value.
    /// </summary>
    [JsonPropertyName("lastKnownEventId")]
    public int LastKnownEventId { get; init; }
}
