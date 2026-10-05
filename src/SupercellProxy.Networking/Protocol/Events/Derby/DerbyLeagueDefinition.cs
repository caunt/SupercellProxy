using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes all native Derby rule overrides for one league.</summary>
public sealed record DerbyLeagueDefinition
{
    /// <summary>Gets the bingo point increase percentage.</summary>
    [JsonPropertyName("bingoPointsPercentage")]
    public int BingoPointsPercentage { get; init; }

    /// <summary>Gets the ordered bingo-line reward sets.</summary>
    [JsonPropertyName("bingoReward")]
    public DerbyRewardSetDefinition?[]? BingoRewards { get; init; }

    /// <summary>Gets the percentage of tasks relevant to a bingo board.</summary>
    [JsonPropertyName("bingoRelevantTaskPercentage")]
    public int BingoTaskPercentage { get; init; }

    /// <summary>Gets the task names excluded from the board.</summary>
    [JsonPropertyName("blackListedTaskNames")]
    public string[]? BlockedTaskNames { get; init; }

    /// <summary>Gets the placement-reward quantity percentage.</summary>
    [JsonPropertyName("boostedTopRewardQuantityPercentage")]
    public int BoostedPlacementRewardPercentage { get; init; }

    /// <summary>Gets the boosted reward item names.</summary>
    [JsonPropertyName("boostedRewardNames")]
    public string[]? BoostedRewardNames { get; init; }

    /// <summary>Gets the extra spawn percentage for the selected rewards.</summary>
    [JsonPropertyName("boostedRewardExtraSpawnPercentage")]
    public int BoostedRewardSpawnPercentage { get; init; }

    /// <summary>Gets the threshold-reward quantity percentage.</summary>
    [JsonPropertyName("boostedThresholdRewardQuantityPercentage")]
    public int BoostedThresholdRewardPercentage { get; init; }

    /// <summary>Gets the ordered bunny reward sets.</summary>
    [JsonPropertyName("bunnyReward")]
    public DerbyRewardSetDefinition?[]? BunnyRewards { get; init; }

    /// <summary>Gets the bunny task bundle names.</summary>
    [JsonPropertyName("bunnyDerbyTaskNames")]
    public string[]? BunnyTaskNames { get; init; }

    /// <summary>Gets the extra bunny-task spawn percentage.</summary>
    [JsonPropertyName("bunnyDerbyTaskExtraSpawnPercentage")]
    public int BunnyTaskSpawnPercentage { get; init; }

    /// <summary>Gets the named bunny threshold configuration.</summary>
    [JsonPropertyName("bunnyThresholdConfiguration")]
    public string? BunnyThresholdConfiguration { get; init; }

    /// <summary>Gets the ordered bunny appearances and catch windows.</summary>
    [JsonPropertyName("bunnyTimeConfiguration")]
    public DerbyBunnyTimeDefinition?[]? BunnyTimes { get; init; }

    /// <summary>Gets the chill task bundle names.</summary>
    [JsonPropertyName("chillDerbyTaskNames")]
    public string[]? ChillTaskNames { get; init; }

    /// <summary>Gets the extra chill-task spawn percentage.</summary>
    [JsonPropertyName("chillDerbyTaskExtraSpawnPercentage")]
    public int ChillTaskSpawnPercentage { get; init; }

    /// <summary>Gets task tags excluded from the board.</summary>
    [JsonPropertyName("excludeTasksWithTags")]
    public string[]? ExcludedTaskTags { get; init; }

    /// <summary>Gets the extra tasks available to purchase.</summary>
    [JsonPropertyName("extraTaskCount")]
    public int ExtraTaskCount { get; init; }

    /// <summary>Gets the diamond price of an extra task.</summary>
    [JsonPropertyName("extraTaskDiamondPrice")]
    public int ExtraTaskPrice { get; init; }

    /// <summary>Gets the hot-potato task bundle names.</summary>
    [JsonPropertyName("hotPotatoTaskNames")]
    public string[]? HotPotatoTaskNames { get; init; }

    /// <summary>Gets the extra hot-potato-task spawn percentage.</summary>
    [JsonPropertyName("hotPotatoTaskExtraSpawnPercentage")]
    public int HotPotatoTaskSpawnPercentage { get; init; }

    /// <summary>Gets task tags included on the board.</summary>
    [JsonPropertyName("includeTasksWithTags")]
    public string[]? IncludedTaskTags { get; init; }

    /// <summary>Gets optional promotion and demotion overrides.</summary>
    [JsonPropertyName("derbyLeagueConfig")]
    public DerbyLeagueChangeDefinition? LeagueChanges { get; init; }

    /// <summary>Gets the limit of boosters per threshold.</summary>
    [JsonPropertyName("maxBoostersInThreshold")]
    public int MaximumBoostersPerThreshold { get; init; } = 1;

    /// <summary>Gets the bunny-task limit.</summary>
    [JsonPropertyName("maxBunnyDerbyTasks")]
    public int MaximumBunnyTasks { get; init; }

    /// <summary>Gets the maximum mystery tasks.</summary>
    [JsonPropertyName("maxMysteryTasks")]
    public int MaximumMysteryTasks { get; init; }

    /// <summary>Gets the extra spawn percentage of maximum-point tasks.</summary>
    [JsonPropertyName("maxPointTasksExtraSpawnPercentage")]
    public int MaximumPointTaskSpawnPercentage { get; init; }

    /// <summary>Gets the same-type bunny-task limit.</summary>
    [JsonPropertyName("maxSameTypeBunnyDerbyTasks")]
    public int MaximumSameTypeBunnyTasks { get; init; }

    /// <summary>Gets the same-type chill-task limit.</summary>
    [JsonPropertyName("maxSameTypeChillDerbyTasks")]
    public int MaximumSameTypeChillTasks { get; init; }

    /// <summary>Gets the same-type hot-potato-task limit.</summary>
    [JsonPropertyName("maxSameTypeHotPotatoTasks")]
    public int MaximumSameTypeHotPotatoTasks { get; init; }

    /// <summary>Gets the player task limit; zero retains the ordinary league limit.</summary>
    [JsonPropertyName("maxTasksPerPlayer")]
    public int MaximumTasksPerPlayer { get; init; }

    /// <summary>Gets the mystery task bundle names.</summary>
    [JsonPropertyName("mysteryTaskNames")]
    public string[]? MysteryTaskNames { get; init; }

    /// <summary>Gets the points awarded for a mystery task.</summary>
    [JsonPropertyName("mysteryTaskPoints")]
    public int MysteryTaskPoints { get; init; }

    /// <summary>Gets the mystery-task spawn increase percentage.</summary>
    [JsonPropertyName("mysteryTaskExtraSpawnPercentage")]
    public int MysteryTaskSpawnPercentage { get; init; }

    /// <summary>Gets the themed task names.</summary>
    [JsonPropertyName("themedTaskNames")]
    public string[]? ThemedTaskNames { get; init; }

    /// <summary>Gets the point increase percentage of themed tasks.</summary>
    [JsonPropertyName("themedTaskExtraPointsPercentage")]
    public int ThemedTaskPointsPercentage { get; init; }

    /// <summary>Gets the spawn increase percentage of themed tasks.</summary>
    [JsonPropertyName("themedTaskExtraSpawnPercentage")]
    public int ThemedTaskSpawnPercentage { get; init; }

    /// <summary>Gets the named score-threshold configuration.</summary>
    [JsonPropertyName("thresholdConfiguration")]
    public string? ThresholdConfiguration { get; init; }

    /// <summary>Gets the threshold count; nonpositive values retain the ordinary league count.</summary>
    [JsonPropertyName("thresholdCount")]
    public int ThresholdCount { get; init; } = -1;
}
