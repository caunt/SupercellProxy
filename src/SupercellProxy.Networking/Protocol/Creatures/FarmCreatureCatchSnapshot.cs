using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>Retains the daily creature catches associated with one farm's avatar.</summary>
public sealed record FarmCreatureCatchSnapshot
{
    /// <summary>Gets the high word of the farm avatar id.</summary>
    [JsonPropertyName("farmVisitingCatchAvatarId_hi")]
    public int AvatarIdHigh { get; init; }

    /// <summary>Gets the low word of the farm avatar id.</summary>
    [JsonPropertyName("farmVisitingCatchAvatarId_lo")]
    public int AvatarIdLow { get; init; }

    /// <summary>Gets catch counts paired with the spawn rule ids.</summary>
    [JsonPropertyName("dailyVisitingCatchCounts")]
    public int[] CatchCounts { get; init; } = [];

    /// <summary>Gets the spawn rules whose creatures were caught on this farm.</summary>
    [JsonPropertyName("dailyVisitingCatchGlobalIds")]
    public int[] SpawnRuleIds { get; init; } = [];
}
