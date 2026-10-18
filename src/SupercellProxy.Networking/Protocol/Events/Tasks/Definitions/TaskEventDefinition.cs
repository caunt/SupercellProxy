using System.Text.Json.Serialization;

using SupercellProxy.Networking.Assets.Tables;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// The document of a task event (live-event category <c>BirthdayTasks</c>): its task quota, task list rules, reward tracks and
/// the task table it brings.
/// </summary>
public sealed record TaskEventDefinition
{
    /// <summary>
    /// Whether every visible task makes progress, rather than only the pinned one.
    /// </summary>
    [JsonPropertyName("AllTasksAlwaysActive")]
    public bool AllTasksAlwaysActive { get; init; }

    /// <summary>
    /// Message shown once the event is over.
    /// </summary>
    [JsonPropertyName("completedText")]
    public string? CompletedText { get; init; }

    /// <summary>
    /// Rewards granted once the points track is complete.
    /// </summary>
    [JsonPropertyName("eventCompletedRewards")]
    public TaskEventRewardDefinition[] EventCompletedRewards { get; init; } = [];

    /// <summary>
    /// Whether a task whose timer ran out may be renewed.
    /// </summary>
    [JsonPropertyName("ExpiredTaskRenewEnabled")]
    public bool ExpiredTaskRenewEnabled { get; init; }

    /// <summary>
    /// How many tasks of one production building the task list may hold at once.
    /// </summary>
    [JsonPropertyName("machineLimits")]
    public TaskEventMachineLimit[] MachineLimits { get; init; } = [];

    /// <summary>
    /// Points the tasks created in one day add up to at least; a late new task of the day carries the missing points.
    /// </summary>
    [JsonPropertyName("MinPointsPerDay")]
    public int MinimumPointsPerDay { get; init; }

    /// <summary>
    /// Level the player needs to take part.
    /// </summary>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>
    /// Rewards of the points track.
    /// </summary>
    [JsonPropertyName("rewards")]
    public TaskEventRewardDefinition[] Rewards { get; init; } = [];

    /// <summary>
    /// Table edits the event makes while it runs; it brings its own task table this way.
    /// </summary>
    [JsonPropertyName("csvChange")]
    public DataTableChangeSet? TableChanges { get; init; }

    /// <summary>
    /// Whether the event has a daily task quota or unlimited tasks.
    /// </summary>
    [JsonPropertyName("TaskCountType")]
    public TaskEventCountType? TaskCountType { get; init; }

    /// <summary>
    /// Whether unused daily quota carries over to the next day.
    /// </summary>
    [JsonPropertyName("TaskQuotaRollover")]
    public bool TaskQuotaRollover { get; init; }

    /// <summary>
    /// How a task is replaced.
    /// </summary>
    [JsonPropertyName("TaskReplacement")]
    public TaskEventReplacement? TaskReplacement { get; init; }

    /// <summary>
    /// Title shown above the task list.
    /// </summary>
    [JsonPropertyName("tasksTitle")]
    public string? TasksTitle { get; init; }

    /// <summary>
    /// How many tasks the list shows at once.
    /// </summary>
    [JsonPropertyName("VisibleTaskCount")]
    public int VisibleTaskCount { get; init; }
}
