using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>
/// Represents decoded <c language="csharp">CreatureManagerSnapshot</c> home data.
/// </summary>
public sealed record CreatureManagerSnapshot
{
    /// <summary>Gets whether a creature has been caught in the current event.</summary>
    [JsonPropertyName("anyCreatureCaughtInCurrentEvent")]
    public bool AnyCreatureCaughtInCurrentEvent { get; init; }

    /// <summary>Gets spawn rules with retained consecutive failed spawn attempts.</summary>
    [JsonPropertyName("failstreakGlobalIds")]
    public int[] FailedSpawnRuleIdentifiers { get; init; } = [];

    /// <summary>Gets consecutive failed attempts used to increase each rule's spawn chance.</summary>
    [JsonPropertyName("failstreakCounts")]
    public int[] FailedSpawnCounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">DailyMaxSpawnResetTime</c> value.
    /// </summary>
    [JsonPropertyName("dailyMaxSpawnResetTime")]
    public int DailyMaximumSpawnResetTime { get; init; }

    /// <summary>Gets the rules with retained daily spawn counts.</summary>
    [JsonPropertyName("dailySpawnGlobalIds")]
    public int[] DailySpawnRuleIdentifiers { get; init; } = [];

    /// <summary>Gets the daily counts paired with the retained rule identifiers.</summary>
    [JsonPropertyName("dailySpawnCount")]
    public int[] DailySpawnCounts { get; init; } = [];

    /// <summary>Gets the rules with retained event spawn counts.</summary>
    [JsonPropertyName("eventSpawnGlobalIds")]
    public int[] EventSpawnRuleIdentifiers { get; init; } = [];

    /// <summary>Gets the event counts paired with the retained rule identifiers.</summary>
    [JsonPropertyName("eventSpawnCount")]
    public int[] EventSpawnCounts { get; init; } = [];

    /// <summary>Gets the daily catch counts retained separately for each farm avatar.</summary>
    [JsonPropertyName("farmVisitingCatchList")]
    public FarmCreatureCatchSnapshot?[] FarmVisitingCatchList { get; init; } = [];

    /// <summary>Gets creature definitions with daily bonus-reward catch counts.</summary>
    [JsonPropertyName("dailyRewardedCatchesGlobalIds")]
    public int[] DailyRewardedCreatureIdentifiers { get; init; } = [];

    /// <summary>Gets daily bonus-reward counts paired with creature definitions.</summary>
    [JsonPropertyName("dailyRewardedCatchesCount")]
    public int[] DailyRewardedCatchCounts { get; init; } = [];

    /// <summary>Gets creature definitions with event bonus-reward catch counts.</summary>
    [JsonPropertyName("eventRewardedCatchesGlobalIds")]
    public int[] EventRewardedCreatureIdentifiers { get; init; } = [];

    /// <summary>Gets event bonus-reward counts paired with creature definitions.</summary>
    [JsonPropertyName("eventRewardedCatchesCount")]
    public int[] EventRewardedCatchCounts { get; init; } = [];

    /// <summary>Gets spawn rules with daily catch counts across farms.</summary>
    [JsonPropertyName("dailyVisitingCatchGlobalIds")]
    public int[] DailyCatchRuleIdentifiers { get; init; } = [];

    /// <summary>Gets daily catches paired with spawn rule identifiers.</summary>
    [JsonPropertyName("dailyVisitingCatchCounts")]
    public int[] DailyCatchCounts { get; init; } = [];

    /// <summary>Gets spawn rules with event catch counts across farms.</summary>
    [JsonPropertyName("eventVisitingCatchGlobalIds")]
    public int[] EventCatchRuleIdentifiers { get; init; } = [];

    /// <summary>Gets event catches paired with spawn rule identifiers.</summary>
    [JsonPropertyName("eventVisitingCatchCounts")]
    public int[] EventCatchCounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">LastKnownEventId</c> value.
    /// </summary>
    [JsonPropertyName("lastKnownEventId")]
    public int LastKnownEventIdentifier { get; init; }

    /// <summary>Gets the retained native switch that suppresses creature spawning.</summary>
    [JsonPropertyName("debugNeverSpawn")]
    public bool SpawningDisabled { get; init; }

    /// <summary>Gets whether daily spawning is waiting for the own farm.</summary>
    [JsonPropertyName("wantsFarmDailyCreatureSpawn")]
    public bool WantsFarmDailySpawn { get; init; }
}
