using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>
/// Represents decoded <c language="csharp">CreatureManagerSnapshot</c> home data.
/// </summary>
public sealed record CreatureManagerSnapshot
{
    /// <summary>Gets or sets a value indicating whether a creature has been caught in the current event.</summary>
    /// <value><see langword="true"/> if the player caught at least one creature since <see cref="LastKnownEventId"/> started; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("anyCreatureCaughtInCurrentEvent")]
    public bool AnyCreatureCaughtInCurrentEvent { get; init; }

    /// <summary>Gets or sets the spawn rules with retained consecutive failed spawn attempts.</summary>
    /// <value>Spawn rule global ids, saved as <c>failstreakGlobalIds</c>, paired by index with <see cref="FailedSpawnCounts"/>.</value>
    [JsonPropertyName("failstreakGlobalIds")]
    public ImmutableArray<int> FailedSpawnRuleIds { get; init; } = [];

    /// <summary>Gets or sets the consecutive failed spawn attempts of each retained rule.</summary>
    /// <value>One fail streak per entry of <see cref="FailedSpawnRuleIds"/>, saved as <c>failstreakCounts</c>.</value>
    [JsonPropertyName("failstreakCounts")]
    public ImmutableArray<int> FailedSpawnCounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">DailyMaxSpawnResetTime</c> value.
    /// </summary>
    /// <value>The Unix time in seconds of the next daily reset of the per-day counters, at the configured reset hour (UTC).</value>
    [JsonPropertyName("dailyMaxSpawnResetTime")]
    public int DailyMaximumSpawnResetTime { get; init; }

    /// <summary>Gets or sets the rules with retained daily spawn counts.</summary>
    /// <value>Spawn rule global ids, saved as <c>dailySpawnGlobalIds</c>, paired by index with <see cref="DailySpawnCounts"/>.</value>
    [JsonPropertyName("dailySpawnGlobalIds")]
    public ImmutableArray<int> DailySpawnRuleIds { get; init; } = [];

    /// <summary>Gets or sets the daily counts paired with the retained rule ids.</summary>
    /// <value>Creatures spawned today per entry of <see cref="DailySpawnRuleIds"/>, saved as <c>dailySpawnCount</c>.</value>
    [JsonPropertyName("dailySpawnCount")]
    public ImmutableArray<int> DailySpawnCounts { get; init; } = [];

    /// <summary>Gets or sets the rules with retained event spawn counts.</summary>
    /// <value>Spawn rule global ids, saved as <c>eventSpawnGlobalIds</c>, paired by index with <see cref="EventSpawnCounts"/>.</value>
    [JsonPropertyName("eventSpawnGlobalIds")]
    public ImmutableArray<int> EventSpawnRuleIds { get; init; } = [];

    /// <summary>Gets or sets the event counts paired with the retained rule ids.</summary>
    /// <value>Creatures spawned during the current event per entry of <see cref="EventSpawnRuleIds"/>, saved as <c>eventSpawnCount</c>.</value>
    [JsonPropertyName("eventSpawnCount")]
    public ImmutableArray<int> EventSpawnCounts { get; init; } = [];

    /// <summary>Gets or sets the daily catch counts retained separately for each farm avatar.</summary>
    /// <value>One entry per visited farm the player caught creatures on today, saved as <c>farmVisitingCatchList</c>; it can contain <see langword="null"/> entries.</value>
    [JsonPropertyName("farmVisitingCatchList")]
    public ImmutableArray<FarmCreatureCatchSnapshot?> FarmVisitingCatchList { get; init; } = [];

    /// <summary>Gets or sets the creature definitions with daily bonus-reward catch counts.</summary>
    /// <value>Creature data global ids, saved as <c>dailyRewardedCatchesGlobalIds</c>, paired by index with <see cref="DailyRewardedCatchCounts"/>.</value>
    [JsonPropertyName("dailyRewardedCatchesGlobalIds")]
    public ImmutableArray<int> DailyRewardedCreatureIds { get; init; } = [];

    /// <summary>Gets or sets the daily bonus-reward counts paired with creature definitions.</summary>
    /// <value>Catches that paid a bonus reward today per entry of <see cref="DailyRewardedCreatureIds"/>, saved as <c>dailyRewardedCatchesCount</c>.</value>
    [JsonPropertyName("dailyRewardedCatchesCount")]
    public ImmutableArray<int> DailyRewardedCatchCounts { get; init; } = [];

    /// <summary>Gets or sets the creature definitions with event bonus-reward catch counts.</summary>
    /// <value>Creature data global ids, saved as <c>eventRewardedCatchesGlobalIds</c>, paired by index with <see cref="EventRewardedCatchCounts"/>.</value>
    [JsonPropertyName("eventRewardedCatchesGlobalIds")]
    public ImmutableArray<int> EventRewardedCreatureIds { get; init; } = [];

    /// <summary>Gets or sets the event bonus-reward counts paired with creature definitions.</summary>
    /// <value>Catches that paid a bonus reward during the current event per entry of <see cref="EventRewardedCreatureIds"/>, saved as <c>eventRewardedCatchesCount</c>.</value>
    [JsonPropertyName("eventRewardedCatchesCount")]
    public ImmutableArray<int> EventRewardedCatchCounts { get; init; } = [];

    /// <summary>Gets or sets the spawn rules with daily catch counts across farms.</summary>
    /// <value>Spawn rule global ids, saved as <c>dailyVisitingCatchGlobalIds</c>, paired by index with <see cref="DailyCatchCounts"/>.</value>
    [JsonPropertyName("dailyVisitingCatchGlobalIds")]
    public ImmutableArray<int> DailyCatchRuleIds { get; init; } = [];

    /// <summary>Gets or sets the daily catches paired with spawn rule ids.</summary>
    /// <value>Creatures caught today on any farm per entry of <see cref="DailyCatchRuleIds"/>, saved as <c>dailyVisitingCatchCounts</c>.</value>
    [JsonPropertyName("dailyVisitingCatchCounts")]
    public ImmutableArray<int> DailyCatchCounts { get; init; } = [];

    /// <summary>Gets or sets the spawn rules with event catch counts across farms.</summary>
    /// <value>Spawn rule global ids, saved as <c>eventVisitingCatchGlobalIds</c>, paired by index with <see cref="EventCatchCounts"/>.</value>
    [JsonPropertyName("eventVisitingCatchGlobalIds")]
    public ImmutableArray<int> EventCatchRuleIds { get; init; } = [];

    /// <summary>Gets or sets the event catches paired with spawn rule ids.</summary>
    /// <value>Creatures caught during the current event on any farm per entry of <see cref="EventCatchRuleIds"/>, saved as <c>eventVisitingCatchCounts</c>.</value>
    [JsonPropertyName("eventVisitingCatchCounts")]
    public ImmutableArray<int> EventCatchCounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the creature event the per-event spawn counters were counted for.
    /// </summary>
    /// <value>The live event id, saved as <c>lastKnownEventId</c>; 0 when no creature event was running.</value>
    [JsonPropertyName("lastKnownEventId")]
    public int LastKnownEventId { get; init; }

    /// <summary>Gets or sets the retained native switch that suppresses creature spawning.</summary>
    /// <value><see langword="true"/> if the debug flag saved as <c>debugNeverSpawn</c> blocks every spawn; otherwise, <see langword="false"/>.</value>
    /// <remarks>Normal accounts have the value <see langword="false"/>.</remarks>
    [JsonPropertyName("debugNeverSpawn")]
    public bool SpawningDisabled { get; init; }

    /// <summary>Gets or sets a value indicating whether daily spawning is waiting for the own farm.</summary>
    /// <value><see langword="true"/> if the day's creatures still have to be spawned the next time the player is on their own farm; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("wantsFarmDailyCreatureSpawn")]
    public bool WantsFarmDailySpawn { get; init; }
}
