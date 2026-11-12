using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>Retains the daily creature catches associated with one farm's avatar.</summary>
public sealed record FarmCreatureCatchSnapshot
{
    /// <summary>Gets or sets the high word of the farm avatar id.</summary>
    /// <value>The high 32 bits of the visited farm's avatar id, saved as <c>farmVisitingCatchAvatarId_hi</c>; combined with <see cref="AvatarIdLow"/> into one <see cref="LongId"/>.</value>
    [JsonPropertyName("farmVisitingCatchAvatarId_hi")]
    public int AvatarIdHigh { get; init; }

    /// <summary>Gets or sets the low word of the farm avatar id.</summary>
    /// <value>The low 32 bits of the visited farm's avatar id, saved as <c>farmVisitingCatchAvatarId_lo</c>.</value>
    [JsonPropertyName("farmVisitingCatchAvatarId_lo")]
    public int AvatarIdLow { get; init; }

    /// <summary>Gets or sets the catch counts paired with the spawn rule ids.</summary>
    /// <value>Creatures caught on this farm today per entry of <see cref="SpawnRuleIds"/>, matched by index.</value>
    [JsonPropertyName("dailyVisitingCatchCounts")]
    public int[] CatchCounts { get; init; } = [];

    /// <summary>Gets or sets the spawn rules whose creatures were caught on this farm.</summary>
    /// <value>Spawn rule global ids paired by index with <see cref="CatchCounts"/>, saved as <c>dailyVisitingCatchGlobalIds</c>.</value>
    [JsonPropertyName("dailyVisitingCatchGlobalIds")]
    public int[] SpawnRuleIds { get; init; } = [];
}
