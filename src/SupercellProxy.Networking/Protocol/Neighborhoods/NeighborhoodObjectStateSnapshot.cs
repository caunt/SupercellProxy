
namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Represents decoded <c language="csharp">NeighborhoodObjectStateSnapshot</c> home data.
/// </summary>
public sealed record NeighborhoodObjectStateSnapshot
{
    /// <summary>Gets the zero-based visual milestones whose decorations were claimed.</summary>
    public int[] ClaimedVisualTiers { get; init; } = [];
    /// <summary>Gets the perks collected during this event.</summary>
    public FarmPass.FarmPassPerkSnapshot[] Perks { get; init; } = [];
    /// <summary>Gets the collected reward-tier indices.</summary>
    public int[] ClaimedRewardsTiers { get; init; } = [];
    /// <summary>Gets reward groups corresponding to the collected tier indices.</summary>
    public FarmPass.FarmPassRewardGroup[] ClaimedRewards { get; init; } = [];
    /// <summary>Gets the saved leaderboard score ids.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("LeaderboardScoreIds")]
    public long[] LeaderboardScoreIds { get; init; } = [];

    /// <summary>Gets scores corresponding to the saved leaderboard ids.</summary>
    public int[] LeaderboardScores { get; init; } = [];

    /// <summary>Gets the neighborhood-wide event points.</summary>
    public int NeighborhoodPoints { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public NeighborhoodObjectTaskSnapshot?[] Tasks { get; init; } = [];

    /// <summary>Gets the saved number of triggered neighborhood perks.</summary>
    public int PerkTriggerCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompletedTasks</c> value.
    /// </summary>
    public CompletedNeighborhoodTaskSnapshot[] CompletedTasks { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">RemainingTaskSets</c> value.
    /// </summary>
    public int RemainingTaskSets { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PendingTaskGroups</c> value.
    /// </summary>
    public string[] PendingTaskGroups { get; init; } = [];

    /// <summary>Gets points earned from completed personal tasks.</summary>
    public int TotalPersonalPoints { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">WeeklyTaskQuotasGained</c> value.
    /// </summary>
    public int WeeklyTaskQuotasGained { get; init; }
}
