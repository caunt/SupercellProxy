using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Saved per-player state of a task event: the visible tasks, today's quota, the event points and the reward tracks.
/// </summary>
public sealed record TaskEventSnapshot
{
    /// <summary>
    /// Tasks finished today, collected or replaced.
    /// </summary>
    [JsonPropertyName("completedCountToday")]
    public int CompletedCountToday { get; init; }

    /// <summary>
    /// Tasks finished since the event started.
    /// </summary>
    [JsonPropertyName("completedTasksTotal")]
    public int CompletedTasksTotal { get; init; }

    /// <summary>
    /// Extra task slots bought today.
    /// </summary>
    [JsonPropertyName("dailyPurchasedQueueCount")]
    public int DailyPurchasedQueueCount { get; init; }

    /// <summary>
    /// Index of the event day the state belongs to, counted from the event start.
    /// </summary>
    [JsonPropertyName("dayIndex")]
    public int DayIndex { get; init; }

    /// <summary>
    /// Tasks discarded today.
    /// </summary>
    [JsonPropertyName("discardedCountToday")]
    public int DiscardedCountToday { get; init; }

    /// <summary>
    /// Rewards granted once the event's points track is complete.
    /// </summary>
    [JsonPropertyName("eventCompletedRewards")]
    public TaskEventRewardSnapshot[] EventCompletedRewards { get; init; } = [];

    /// <summary>
    /// New tasks still allowed; it may be negative.
    /// </summary>
    [JsonPropertyName("freeTasksRemainingToday")]
    public int FreeTasksRemainingToday { get; init; }

    /// <summary>
    /// Tasks the event allows up to now.
    /// </summary>
    [JsonPropertyName("maxCompletableTasks")]
    public int MaximumCompletableTasks { get; init; }

    /// <summary>
    /// Whether the player has opened the event at least once.
    /// </summary>
    [JsonPropertyName("openedByPlayer")]
    public bool OpenedByPlayer { get; init; }

    /// <summary>
    /// Event points collected from finished tasks.
    /// </summary>
    [JsonPropertyName("points")]
    public int Points { get; init; }

    /// <summary>
    /// Rewards of the event's points track.
    /// </summary>
    [JsonPropertyName("rewards")]
    public TaskEventRewardSnapshot[] Rewards { get; init; } = [];

    /// <summary>
    /// Tasks replaced today; new tasks avoid repeating them.
    /// </summary>
    [JsonPropertyName("taskHistory")]
    public TaskEventEntrySnapshot[] TaskHistory { get; init; } = [];

    /// <summary>
    /// Points of the tasks created today.
    /// </summary>
    [JsonPropertyName("totalCreatedPointsToday")]
    public int TotalCreatedPointsToday { get; init; }

    /// <summary>
    /// Tasks created since the event started.
    /// </summary>
    [JsonPropertyName("totalCreationCount")]
    public int TotalCreationCount { get; init; }

    /// <summary>
    /// Daily task quota; 0 for an event with unlimited tasks.
    /// </summary>
    [JsonPropertyName("totalTaskCount")]
    public int TotalTaskCount { get; init; }

    /// <summary>
    /// How many tasks the list shows at once.
    /// </summary>
    [JsonPropertyName("visibleTaskCount")]
    public int VisibleTaskCount { get; init; }

    /// <summary>
    /// Tasks of the list in slot order; empty slots are null.
    /// </summary>
    [JsonPropertyName("visibleTasks")]
    public TaskEventEntrySnapshot?[] VisibleTasks { get; init; } = [];
}
