using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes all native Derby rule overrides for one league.</summary>
public sealed record DerbyLeagueDefinition
{
    /// <summary>Gets or sets the bingo point increase percentage.</summary>
    /// <value>Extra points, in percent, for bingo in this league.</value>
    [JsonPropertyName("bingoPointsPercentage")]
    public int BingoPointsPercentage { get; init; }

    /// <summary>Gets or sets the ordered bingo-line reward sets.</summary>
    /// <value>Reward sets named after rows of the derby rewards table, one per bingo line in order (saved as <c>bingoReward</c>); individual entries may be <see langword="null"/>, and <see langword="null"/> means no bingo rewards.</value>
    [JsonPropertyName("bingoReward")]
    public ImmutableArray<DerbyRewardSetDefinition?>? BingoRewards { get; init; }

    /// <summary>Gets or sets the percentage of tasks relevant to a bingo board.</summary>
    /// <value>The share, in percent, of offered tasks that count toward the bingo board (saved as <c>bingoRelevantTaskPercentage</c>); 0 when the definition omits it.</value>
    [JsonPropertyName("bingoRelevantTaskPercentage")]
    public int BingoTaskPercentage { get; init; }

    /// <summary>Gets or sets the task names excluded from the board.</summary>
    /// <value>Neighbourhood-task names that never appear on this league's board (saved as <c>blackListedTaskNames</c>), or <see langword="null"/> when none are blocked.</value>
    [JsonPropertyName("blackListedTaskNames")]
    public ImmutableArray<string>? BlockedTaskNames { get; init; }

    /// <summary>Gets or sets the placement-reward quantity percentage.</summary>
    /// <value>Percent added to the quantity of a boosted placement reward (saved as <c>boostedTopRewardQuantityPercentage</c>).</value>
    [JsonPropertyName("boostedTopRewardQuantityPercentage")]
    public int BoostedPlacementRewardPercentage { get; init; }

    /// <summary>Gets or sets the boosted reward item names.</summary>
    /// <value>Names of the reward items the boost percentages apply to, or <see langword="null"/> when none are boosted.</value>
    [JsonPropertyName("boostedRewardNames")]
    public ImmutableArray<string>? BoostedRewardNames { get; init; }

    /// <summary>Gets or sets the extra spawn percentage for the selected rewards.</summary>
    /// <value>Percent by which each boosted reward is made more likely to be offered (saved as <c>boostedRewardExtraSpawnPercentage</c>).</value>
    [JsonPropertyName("boostedRewardExtraSpawnPercentage")]
    public int BoostedRewardSpawnPercentage { get; init; }

    /// <summary>Gets or sets the threshold-reward quantity percentage.</summary>
    /// <value>Percent added to the quantity of a boosted threshold reward (saved as <c>boostedThresholdRewardQuantityPercentage</c>).</value>
    [JsonPropertyName("boostedThresholdRewardQuantityPercentage")]
    public int BoostedThresholdRewardPercentage { get; init; }

    /// <summary>Gets or sets the ordered bunny reward sets.</summary>
    /// <value>Reward sets named after rows of the derby rewards table, in order (saved as <c>bunnyReward</c>); individual entries may be <see langword="null"/>, and <see langword="null"/> means no bunny rewards.</value>
    [JsonPropertyName("bunnyReward")]
    public ImmutableArray<DerbyRewardSetDefinition?>? BunnyRewards { get; init; }

    /// <summary>Gets or sets the bunny task bundle names.</summary>
    /// <value>Names of <c>BunnyDerby</c> neighbourhood-task rows (saved as <c>bunnyDerbyTaskNames</c>), or <see langword="null"/> when the league offers none.</value>
    [JsonPropertyName("bunnyDerbyTaskNames")]
    public ImmutableArray<string>? BunnyTaskNames { get; init; }

    /// <summary>Gets or sets the extra bunny-task spawn percentage.</summary>
    /// <value>Percent by which bunny tasks are made more likely to appear on the board; 0 when the definition omits it.</value>
    [JsonPropertyName("bunnyDerbyTaskExtraSpawnPercentage")]
    public int BunnyTaskSpawnPercentage { get; init; }

    /// <summary>Gets or sets the named bunny threshold configuration.</summary>
    /// <value>The name of the bunny threshold setup that decides how many bunny tasks a neighbourhood gets for its member count, or <see langword="null"/> when the definition omits it.</value>
    [JsonPropertyName("bunnyThresholdConfiguration")]
    public string? BunnyThresholdConfiguration { get; init; }

    /// <summary>Gets or sets the ordered bunny appearances and catch windows.</summary>
    /// <value>One timing entry per bunny in order (saved as <c>bunnyTimeConfiguration</c>); individual entries may be <see langword="null"/>, and <see langword="null"/> means no bunnies.</value>
    [JsonPropertyName("bunnyTimeConfiguration")]
    public ImmutableArray<DerbyBunnyTimeDefinition?>? BunnyTimes { get; init; }

    /// <summary>Gets or sets the chill task bundle names.</summary>
    /// <value>Names of <c>ChillDerby</c> neighbourhood-task rows (saved as <c>chillDerbyTaskNames</c>), or <see langword="null"/> when the league offers none.</value>
    [JsonPropertyName("chillDerbyTaskNames")]
    public ImmutableArray<string>? ChillTaskNames { get; init; }

    /// <summary>Gets or sets the extra chill-task spawn percentage.</summary>
    /// <value>Percent by which chill tasks are made more likely to appear on the board; 0 when the definition omits it.</value>
    [JsonPropertyName("chillDerbyTaskExtraSpawnPercentage")]
    public int ChillTaskSpawnPercentage { get; init; }

    /// <summary>Gets or sets task tags excluded from the board.</summary>
    /// <value>Task tags whose tasks are left off the board (saved as <c>excludeTasksWithTags</c>), or <see langword="null"/> when the definition omits them.</value>
    [JsonPropertyName("excludeTasksWithTags")]
    public ImmutableArray<string>? ExcludedTaskTags { get; init; }

    /// <summary>Gets or sets the extra tasks available to purchase.</summary>
    /// <value>How many additional tasks a player can buy beyond the allowance in this league; 0 when the definition omits it.</value>
    [JsonPropertyName("extraTaskCount")]
    public int ExtraTaskCount { get; init; }

    /// <summary>Gets or sets the diamond price of an extra task.</summary>
    /// <value>Diamonds charged for each extra task (saved as <c>extraTaskDiamondPrice</c>); 0 when the definition omits it.</value>
    [JsonPropertyName("extraTaskDiamondPrice")]
    public int ExtraTaskPrice { get; init; }

    /// <summary>Gets or sets the hot-potato task bundle names.</summary>
    /// <value>Names of <c>HotPotato</c> neighbourhood-task rows, or <see langword="null"/> when the league offers none.</value>
    [JsonPropertyName("hotPotatoTaskNames")]
    public ImmutableArray<string>? HotPotatoTaskNames { get; init; }

    /// <summary>Gets or sets the extra hot-potato-task spawn percentage.</summary>
    /// <value>Percent by which hot-potato tasks are made more likely to appear on the board; 0 when the definition omits it.</value>
    [JsonPropertyName("hotPotatoTaskExtraSpawnPercentage")]
    public int HotPotatoTaskSpawnPercentage { get; init; }

    /// <summary>Gets or sets task tags included on the board.</summary>
    /// <value>Task tags whose tasks the board draws from (saved as <c>includeTasksWithTags</c>), or <see langword="null"/> when the definition omits them.</value>
    [JsonPropertyName("includeTasksWithTags")]
    public ImmutableArray<string>? IncludedTaskTags { get; init; }

    /// <summary>Gets or sets optional promotion and demotion overrides.</summary>
    /// <value>The league-change override saved as <c>derbyLeagueConfig</c>, or <see langword="null"/> when the definition omits it.</value>
    [JsonPropertyName("derbyLeagueConfig")]
    public DerbyLeagueChangeDefinition? LeagueChanges { get; init; }

    /// <summary>Gets or sets the limit of boosters per threshold.</summary>
    /// <value>How many booster rewards one threshold's reward choices may hold; 1 when the definition omits it.</value>
    [JsonPropertyName("maxBoostersInThreshold")]
    public int MaximumBoostersPerThreshold { get; init; } = 1;

    /// <summary>Gets or sets the bunny-task limit.</summary>
    /// <value>The cap on bunny tasks in this league's derby (saved as <c>maxBunnyDerbyTasks</c>); 0 when the definition omits it.</value>
    [JsonPropertyName("maxBunnyDerbyTasks")]
    public int MaximumBunnyTasks { get; init; }

    /// <summary>Gets or sets how many mystery tasks a derby in this league can offer at most.</summary>
    /// <value>A mystery task count (saved as <c>maxMysteryTasks</c>); 0 disables mystery tasks.</value>
    [JsonPropertyName("maxMysteryTasks")]
    public int MaximumMysteryTasks { get; init; }

    /// <summary>Gets or sets the extra spawn percentage of maximum-point tasks.</summary>
    /// <value>Percent by which the highest-point tasks are made more likely to appear (saved as <c>maxPointTasksExtraSpawnPercentage</c>); 0 when the definition omits it.</value>
    [JsonPropertyName("maxPointTasksExtraSpawnPercentage")]
    public int MaximumPointTaskSpawnPercentage { get; init; }

    /// <summary>Gets or sets the same-type bunny-task limit.</summary>
    /// <value>The cap on bunny tasks that share one task type; 0 when the definition omits it.</value>
    [JsonPropertyName("maxSameTypeBunnyDerbyTasks")]
    public int MaximumSameTypeBunnyTasks { get; init; }

    /// <summary>Gets or sets the same-type chill-task limit.</summary>
    /// <value>The cap on chill tasks that share one task type; 0 when the definition omits it.</value>
    [JsonPropertyName("maxSameTypeChillDerbyTasks")]
    public int MaximumSameTypeChillTasks { get; init; }

    /// <summary>Gets or sets the same-type hot-potato-task limit.</summary>
    /// <value>The cap on hot-potato tasks that share one task type; 0 when the definition omits it.</value>
    [JsonPropertyName("maxSameTypeHotPotatoTasks")]
    public int MaximumSameTypeHotPotatoTasks { get; init; }

    /// <summary>Gets or sets the player task limit; zero retains the ordinary league limit.</summary>
    /// <value>Tasks one player may take in the derby (saved as <c>maxTasksPerPlayer</c>); 0 when the definition omits it.</value>
    [JsonPropertyName("maxTasksPerPlayer")]
    public int MaximumTasksPerPlayer { get; init; }

    /// <summary>Gets or sets the mystery task bundle names.</summary>
    /// <value>Names of <c>Mystery</c> neighbourhood-task rows, or <see langword="null"/> when the league offers none.</value>
    [JsonPropertyName("mysteryTaskNames")]
    public ImmutableArray<string>? MysteryTaskNames { get; init; }

    /// <summary>Gets or sets the points awarded for a mystery task.</summary>
    /// <value>Derby points a completed mystery task is worth (saved as <c>mysteryTaskPoints</c>).</value>
    [JsonPropertyName("mysteryTaskPoints")]
    public int MysteryTaskPoints { get; init; }

    /// <summary>Gets or sets the mystery-task spawn increase percentage.</summary>
    /// <value>Percent by which mystery tasks are made more likely to appear (saved as <c>mysteryTaskExtraSpawnPercentage</c>).</value>
    [JsonPropertyName("mysteryTaskExtraSpawnPercentage")]
    public int MysteryTaskSpawnPercentage { get; init; }

    /// <summary>Gets or sets the names of the themed tasks a derby in this league offers; <see langword="null"/> when the league has none.</summary>
    /// <value>Neighbourhood-task row names (saved as <c>themedTaskNames</c>).</value>
    [JsonPropertyName("themedTaskNames")]
    public ImmutableArray<string>? ThemedTaskNames { get; init; }

    /// <summary>Gets or sets the point increase percentage of themed tasks.</summary>
    /// <value>Extra points, in percent, that themed tasks are worth (saved as <c>themedTaskExtraPointsPercentage</c>).</value>
    [JsonPropertyName("themedTaskExtraPointsPercentage")]
    public int ThemedTaskPointsPercentage { get; init; }

    /// <summary>Gets or sets the spawn increase percentage of themed tasks.</summary>
    /// <value>Percent by which themed tasks are made more likely to appear (saved as <c>themedTaskExtraSpawnPercentage</c>).</value>
    [JsonPropertyName("themedTaskExtraSpawnPercentage")]
    public int ThemedTaskSpawnPercentage { get; init; }

    /// <summary>Gets or sets the named score-threshold configuration.</summary>
    /// <value>The name of the threshold setup that gives the points each reward threshold needs for a neighbourhood's member count, or <see langword="null"/> when the definition omits it.</value>
    [JsonPropertyName("thresholdConfiguration")]
    public string? ThresholdConfiguration { get; init; }

    /// <summary>Gets or sets the threshold count; nonpositive values retain the ordinary league count.</summary>
    /// <value>Number of reward thresholds (saved as <c>thresholdCount</c>); -1 when the definition omits it.</value>
    [JsonPropertyName("thresholdCount")]
    public int ThresholdCount { get; init; } = -1;
}
