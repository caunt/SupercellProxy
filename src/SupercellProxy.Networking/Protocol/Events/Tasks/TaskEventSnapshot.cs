using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Saved per-player state of a task event: the visible tasks, today's quota, the event points and the reward tracks.
/// </summary>
public sealed record TaskEventSnapshot
{
    /// <summary>
    /// Gets or sets the number of tasks finished today, collected or replaced.
    /// </summary>
    /// <value>How many tasks have left the list as finished during the current event day.</value>
    [JsonPropertyName("completedCountToday")]
    public int CompletedCountToday { get; init; }

    /// <summary>
    /// Gets or sets the number of tasks finished since the event started.
    /// </summary>
    /// <value>The running total of finished tasks over the whole event.</value>
    [JsonPropertyName("completedTasksTotal")]
    public int CompletedTasksTotal { get; init; }

    /// <summary>
    /// Gets or sets the number of extra task slots bought today.
    /// </summary>
    /// <value>How many extra slots the player has paid diamonds for in the current event day.</value>
    [JsonPropertyName("dailyPurchasedQueueCount")]
    public int DailyPurchasedQueueCount { get; init; }

    /// <summary>
    /// Gets or sets the index of the event day the state belongs to, counted from the event start.
    /// </summary>
    /// <value>Whole days elapsed since the event's start time, where 0 is the first day.</value>
    [JsonPropertyName("dayIndex")]
    public int DayIndex { get; init; }

    /// <summary>
    /// Gets or sets the number of tasks discarded today.
    /// </summary>
    /// <value>How many tasks the player has thrown away during the current event day.</value>
    [JsonPropertyName("discardedCountToday")]
    public int DiscardedCountToday { get; init; }

    /// <summary>
    /// Gets or sets the rewards granted once the event's points track is complete.
    /// </summary>
    /// <value>The saved event-completed rewards with their claim markers, in document order; an empty array when the event has none.</value>
    [JsonPropertyName("eventCompletedRewards")]
    public TaskEventRewardSnapshot[] EventCompletedRewards { get; init; } = [];

    /// <summary>
    /// Gets or sets the number of new tasks still allowed today; it may be negative.
    /// </summary>
    /// <value>The number of new tasks the player may still receive in the current day; saved values can be below 0.</value>
    [JsonPropertyName("freeTasksRemainingToday")]
    public int FreeTasksRemainingToday { get; init; }

    /// <summary>
    /// Gets or sets the number of tasks the event allows up to now.
    /// </summary>
    /// <value>The cumulative cap on finished tasks granted by all event days so far, saved as <c>maxCompletableTasks</c>.</value>
    [JsonPropertyName("maxCompletableTasks")]
    public int MaximumCompletableTasks { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the player has opened the event at least once.
    /// </summary>
    /// <value><see langword="true"/> if the player has opened the task event's screen; otherwise, <see langword="false"/>.</value>
    /// <remarks>An event not yet viewed keeps the default <see langword="false"/>.</remarks>
    [JsonPropertyName("openedByPlayer")]
    public bool OpenedByPlayer { get; init; }

    /// <summary>
    /// Gets or sets the event points collected from finished tasks.
    /// </summary>
    /// <value>The player's event points total, on the scale of the <see cref="Rewards"/> thresholds.</value>
    [JsonPropertyName("points")]
    public int Points { get; init; }

    /// <summary>
    /// Gets or sets the rewards of the event's points track.
    /// </summary>
    /// <value>The saved track rewards in document order, each with its points threshold and claim markers; an empty array when none are saved.</value>
    [JsonPropertyName("rewards")]
    public TaskEventRewardSnapshot[] Rewards { get; init; } = [];

    /// <summary>
    /// Gets or sets the tasks replaced today.
    /// </summary>
    /// <value>Copies of the tasks that left the list today; empty by default.</value>
    [JsonPropertyName("taskHistory")]
    public TaskEventEntrySnapshot[] TaskHistory { get; init; } = [];

    /// <summary>
    /// Gets or sets the points of the tasks created today.
    /// </summary>
    /// <value>The sum of event points carried by tasks spawned in the current day.</value>
    [JsonPropertyName("totalCreatedPointsToday")]
    public int TotalCreatedPointsToday { get; init; }

    /// <summary>
    /// Gets or sets the number of tasks created since the event started.
    /// </summary>
    /// <value>The running count of spawned tasks over the whole event.</value>
    [JsonPropertyName("totalCreationCount")]
    public int TotalCreationCount { get; init; }

    /// <summary>
    /// Gets or sets the daily task quota; 0 for an event with unlimited tasks.
    /// </summary>
    /// <value>The number of new tasks one day grants under a limited-tasks event, or 0 when tasks are unlimited.</value>
    [JsonPropertyName("totalTaskCount")]
    public int TotalTaskCount { get; init; }

    /// <summary>
    /// Gets or sets how many tasks the list shows at once.
    /// </summary>
    /// <value>The number of task slots, saved as <c>visibleTaskCount</c>.</value>
    [JsonPropertyName("visibleTaskCount")]
    public int VisibleTaskCount { get; init; }

    /// <summary>
    /// Gets or sets the tasks of the list in slot order.
    /// </summary>
    /// <value>One entry per visible slot, where an empty slot is <see langword="null"/>; an empty array before the list is first filled.</value>
    [JsonPropertyName("visibleTasks")]
    public TaskEventEntrySnapshot?[] VisibleTasks { get; init; } = [];
}
