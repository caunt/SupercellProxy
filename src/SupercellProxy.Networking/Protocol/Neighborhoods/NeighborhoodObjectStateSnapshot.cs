
namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Represents decoded <c language="csharp">NeighborhoodObjectStateSnapshot</c> home data.
/// </summary>
public sealed record NeighborhoodObjectStateSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public NeighborhoodObjectTaskSnapshot?[] Tasks { get; init; } = [];

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

    /// <summary>
    /// Gets or sets the <c language="csharp">WeeklyTaskQuotasGained</c> value.
    /// </summary>
    public int WeeklyTaskQuotasGained { get; init; }
}
