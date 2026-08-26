using System.Text.Json;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>NeighborhoodObjectStateSnapshot</c> home data.
/// </summary>
public sealed record NeighborhoodObjectStateSnapshot
{
    /// <summary>
    /// Gets or sets the <c>Tasks</c> value.
    /// </summary>
    public NeighborhoodObjectTaskSnapshot[] Tasks { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>CompletedTasks</c> value.
    /// </summary>
    public JsonElement[] CompletedTasks { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>PendingTaskGroups</c> value.
    /// </summary>
    public string[] PendingTaskGroups { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>RemainingTaskSets</c> value.
    /// </summary>
    public int RemainingTaskSets { get; init; }

    /// <summary>
    /// Gets or sets the <c>WeeklyTaskQuotasGained</c> value.
    /// </summary>
    public int WeeklyTaskQuotasGained { get; init; }
}
