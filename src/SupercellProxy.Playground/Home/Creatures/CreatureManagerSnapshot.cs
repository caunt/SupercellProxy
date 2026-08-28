using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">CreatureManagerSnapshot</c> home data.
/// </summary>
internal sealed record CreatureManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">DailyMaxSpawnResetTime</c> value.
    /// </summary>
    [JsonPropertyName("dailyMaxSpawnResetTime")]
    public int DailyMaxSpawnResetTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FarmVisitingCatchList</c> value.
    /// </summary>
    [JsonPropertyName("farmVisitingCatchList")]
    public JsonElement[] FarmVisitingCatchList { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">LastKnownEventId</c> value.
    /// </summary>
    [JsonPropertyName("lastKnownEventId")]
    public int LastKnownEventId { get; init; }
}
