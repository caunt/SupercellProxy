using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.FarmPass;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>The player's progress in one neighbourhood event: task slots, task sets, points, rewards and perks.</summary>
public sealed record NeighborhoodObjectStateSnapshot
{
    /// <summary>Gets or sets the zero-based visual milestones whose decorations were claimed.</summary>
    /// <value>Indices into the event's visual levels in claim order. Empty by default.</value>
    public ImmutableArray<int> ClaimedVisualTiers { get; init; } = [];

    /// <summary>Gets or sets the perks collected during this event.</summary>
    /// <value>One entry per perk the event granted, such as a bonus production output chance.</value>
    public ImmutableArray<FarmPassPerkSnapshot> Perks { get; init; } = [];

    /// <summary>Gets or sets the collected reward-tier indices.</summary>
    /// <value>Zero-based indices into the event's distinct point thresholds, in claim order.</value>
    public ImmutableArray<int> ClaimedRewardsTiers { get; init; } = [];

    /// <summary>Gets or sets the reward groups corresponding to the collected tier indices.</summary>
    /// <value>One group per entry of <see cref="ClaimedRewardsTiers"/>, at the same position, listing the items and amounts that claim granted.</value>
    public ImmutableArray<FarmPassRewardGroup> ClaimedRewards { get; init; } = [];

    /// <summary>Gets or sets the saved leaderboard score ids.</summary>
    /// <value>64-bit ids in ascending order, paired by position with <see cref="LeaderboardScores"/>. What the ids name is not identified.</value>
    [JsonPropertyName("LeaderboardScoreIds")]
    public ImmutableArray<long> LeaderboardScoreIds { get; init; } = [];

    /// <summary>Gets or sets the scores corresponding to the saved leaderboard ids.</summary>
    /// <value>One score per entry of <see cref="LeaderboardScoreIds"/>, at the same position.</value>
    public ImmutableArray<int> LeaderboardScores { get; init; } = [];

    /// <summary>Gets or sets the neighborhood-wide event points.</summary>
    /// <value>The neighbourhood's combined points as last sent by the server.</value>
    public int NeighborhoodPoints { get; init; }

    /// <summary>Gets or sets the task slots of the event; <see langword="null"/> marks an empty slot.</summary>
    /// <value>One entry per task slot of the event, in slot order.</value>
    public ImmutableArray<NeighborhoodObjectTaskSnapshot?> Tasks { get; init; } = [];

    /// <summary>Gets or sets the saved number of triggered neighborhood perks.</summary>
    /// <value>How many times a perk paid out, such as an extra production output.</value>
    public int PerkTriggerCount { get; init; }

    /// <summary>Gets or sets the tasks the player finished in this event.</summary>
    /// <value>Turned-in tasks in completion order; empty by default.</value>
    public ImmutableArray<CompletedNeighborhoodTaskSnapshot> CompletedTasks { get; init; } = [];

    /// <summary>Gets or sets the task sets the player may still draw.</summary>
    /// <value>A count of the task sets still available to the player.</value>
    public int RemainingTaskSets { get; init; }

    /// <summary>Gets or sets the task groups of the current set still waiting to fill a slot.</summary>
    /// <value>Task group names from the event's task set; empty by default.</value>
    public ImmutableArray<string> PendingTaskGroups { get; init; } = [];

    /// <summary>Gets or sets the points earned from completed personal tasks.</summary>
    /// <value>The sum of the task points of every task the player turned in.</value>
    public int TotalPersonalPoints { get; init; }

    /// <summary>Gets or sets the event weeks whose task sets were already granted.</summary>
    /// <value>The number of event weeks, counted from the event start with the first week as 1, that have granted their task sets.</value>
    public int WeeklyTaskQuotasGained { get; init; }
}
