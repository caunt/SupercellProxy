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
    /// Gets or sets a value indicating whether every visible task makes progress, rather than only the pinned one.
    /// </summary>
    /// <value><see langword="true"/> if all tasks on the visible list count deliveries at the same time; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    [JsonPropertyName("AllTasksAlwaysActive")]
    public bool AllTasksAlwaysActive { get; init; }

    /// <summary>
    /// Gets or sets the message shown once the event is over.
    /// </summary>
    /// <value>The <c>completedText</c> string of the document, or <see langword="null"/> when the event defines no closing message.</value>
    [JsonPropertyName("completedText")]
    public string? CompletedText { get; init; }

    /// <summary>
    /// Gets or sets the rewards granted once the points track is complete.
    /// </summary>
    /// <value>The <c>eventCompletedRewards</c> entries, each with its points and fallback; an empty array when the event has none. <see cref="ClaimEventTaskRewardCommand"/> claims from this list instead of <see cref="Rewards"/> when its <see cref="ClaimEventTaskRewardCommand.EventCompletedReward"/> flag is set.</value>
    [JsonPropertyName("eventCompletedRewards")]
    public TaskEventRewardDefinition[] EventCompletedRewards { get; init; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether a task whose timer ran out may be renewed.
    /// </summary>
    /// <value><see langword="true"/> if the player may renew an expired task instead of losing it; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    [JsonPropertyName("ExpiredTaskRenewEnabled")]
    public bool ExpiredTaskRenewEnabled { get; init; }

    /// <summary>
    /// Gets or sets how many tasks of one production building the task list may hold at once.
    /// </summary>
    /// <value>One cap per listed building from <c>machineLimits</c>; buildings that are not listed are not capped, and the array is empty by default.</value>
    [JsonPropertyName("machineLimits")]
    public TaskEventMachineLimit[] MachineLimits { get; init; } = [];

    /// <summary>
    /// Gets or sets the points the tasks created in one day add up to at least.
    /// </summary>
    /// <value>The <c>MinPointsPerDay</c> floor on the day's total task points; 0 when the event omits it, which sets no floor.</value>
    [JsonPropertyName("MinPointsPerDay")]
    public int MinimumPointsPerDay { get; init; }

    /// <summary>
    /// Gets or sets the level the player needs to take part.
    /// </summary>
    /// <value>The <c>requirements</c> object with the minimum experience level, or <see langword="null"/> when the event sets no level requirement.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>
    /// Gets or sets the rewards of the points track.
    /// </summary>
    /// <value>The <c>rewards</c> entries in track order, each unlocked by its event points; an empty array when the event defines none.</value>
    [JsonPropertyName("rewards")]
    public TaskEventRewardDefinition[] Rewards { get; init; } = [];

    /// <summary>
    /// Gets or sets the table edits the event makes while it runs; it brings its own task table this way.
    /// </summary>
    /// <value>The <c>csvChange</c> set, which replaces rows such as those of the birthday event task table while the event runs, or <see langword="null"/> when the event changes no tables.</value>
    [JsonPropertyName("csvChange")]
    public DataTableChangeSet? TableChanges { get; init; }

    /// <summary>
    /// Gets or sets whether the event has a daily task quota or unlimited tasks.
    /// </summary>
    /// <value>The <c>TaskCountType</c> object naming the mode and, for limited tasks, the daily quota; <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("TaskCountType")]
    public TaskEventCountType? TaskCountType { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether unused daily quota carries over to the next day.
    /// </summary>
    /// <value><see langword="true"/> if a new day adds its quota to what is left from the previous day; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    [JsonPropertyName("TaskQuotaRollover")]
    public bool TaskQuotaRollover { get; init; }

    /// <summary>
    /// Gets or sets how a task is replaced.
    /// </summary>
    /// <value>The <c>TaskReplacement</c> object naming the replacement mode and, for bought replacements, its diamond price; <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("TaskReplacement")]
    public TaskEventReplacement? TaskReplacement { get; init; }

    /// <summary>
    /// Gets or sets the title shown above the task list.
    /// </summary>
    /// <value>The <c>tasksTitle</c> string, or <see langword="null"/> when the event sets no title.</value>
    [JsonPropertyName("tasksTitle")]
    public string? TasksTitle { get; init; }

    /// <summary>
    /// Gets or sets how many tasks the list shows at once.
    /// </summary>
    /// <value>The <c>VisibleTaskCount</c> number of task slots on the board; 0 when the event omits it.</value>
    [JsonPropertyName("VisibleTaskCount")]
    public int VisibleTaskCount { get; init; }
}
