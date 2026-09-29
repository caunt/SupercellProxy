
namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Represents decoded <c language="csharp">NeighborhoodObjectStateSnapshot</c> home data.
/// </summary>
public sealed record NeighborhoodObjectStateSnapshot
{
    /// <summary>Gets the saved leaderboard score identifiers.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("LeaderboardScoreIds")]
    public long[] LeaderboardScoreIdentifiers { get; init; } = [];

    /// <summary>Gets scores corresponding to the saved leaderboard identifiers.</summary>
    public int[] LeaderboardScores { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public NeighborhoodObjectTaskSnapshot?[] Tasks { get; init; } = [];

    /// <summary>Gets the neighborhood-wide event points.</summary>
    public int NeighborhoodPoints { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompletedTasks</c> value.
    /// </summary>
    public CompletedNeighborhoodTaskSnapshot[] CompletedTasks { get; init; } = [];

    /// <summary>Gets the saved number of triggered neighborhood perks.</summary>
    public int PerkTriggerCount { get; init; }

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
