using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.FarmPass;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>The player's progress in one neighbourhood event: task slots, task sets, points, rewards and perks.</summary>
public sealed record NeighborhoodObjectStateSnapshot
{
    /// <summary>Gets the zero-based visual milestones whose decorations were claimed.</summary>
    public int[] ClaimedVisualTiers { get; init; } = [];
    /// <summary>Gets the perks collected during this event.</summary>
    public FarmPassPerkSnapshot[] Perks { get; init; } = [];
    /// <summary>Gets the collected reward-tier indices.</summary>
    public int[] ClaimedRewardsTiers { get; init; } = [];
    /// <summary>Gets reward groups corresponding to the collected tier indices.</summary>
    public FarmPassRewardGroup[] ClaimedRewards { get; init; } = [];
    /// <summary>Gets the saved leaderboard score ids.</summary>
    [JsonPropertyName("LeaderboardScoreIds")]
    public long[] LeaderboardScoreIds { get; init; } = [];

    /// <summary>Gets scores corresponding to the saved leaderboard ids.</summary>
    public int[] LeaderboardScores { get; init; } = [];

    /// <summary>Gets the neighborhood-wide event points.</summary>
    public int NeighborhoodPoints { get; init; }

    /// <summary>Task slots of the event; null marks an empty slot.</summary>
    public NeighborhoodObjectTaskSnapshot?[] Tasks { get; init; } = [];

    /// <summary>Gets the saved number of triggered neighborhood perks.</summary>
    public int PerkTriggerCount { get; init; }

    /// <summary>Tasks the player finished in this event, which new tasks avoid repeating.</summary>
    public CompletedNeighborhoodTaskSnapshot[] CompletedTasks { get; init; } = [];

    /// <summary>Task sets the player may still draw; each set queues every task group of the event once.</summary>
    public int RemainingTaskSets { get; init; }

    /// <summary>Task groups of the current set still waiting to fill a slot, in shuffled order; the last one is used next.</summary>
    public string[] PendingTaskGroups { get; init; } = [];

    /// <summary>Gets points earned from completed personal tasks.</summary>
    public int TotalPersonalPoints { get; init; }

    /// <summary>Event weeks whose task sets were already granted; each new week grants the event's weekly task sets once.</summary>
    public int WeeklyTaskQuotasGained { get; init; }
}
