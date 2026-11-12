using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// One saved task of a task event: a task-table row, the player's progress on it and its place in the task list.
/// </summary>
public sealed record TaskEventEntrySnapshot
{
    /// <summary>
    /// Gets or sets a value indicating whether the progress reached the task's goal.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="Progress"/> has reached the goal of the task row; otherwise, <see langword="false"/>.</value>
    /// <remarks>A task still in progress keeps the default <see langword="false"/>.</remarks>
    [JsonPropertyName("Complete")]
    public bool Complete { get; init; }

    /// <summary>
    /// Gets or sets the player level when the task was created.
    /// </summary>
    /// <value>The experience level the player had at the moment the task was spawned.</value>
    [JsonPropertyName("PlayerLevelAtTaskStart")]
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>
    /// Gets or sets the event points the task gives instead of its table points.
    /// </summary>
    /// <value>The points granted on collection in place of the row's own points; 0 when the table points apply.</value>
    [JsonPropertyName("PointsOverride")]
    public int PointsOverride { get; init; }

    /// <summary>
    /// Gets or sets how much of the task's goal is done.
    /// </summary>
    /// <value>The count of goal units delivered or produced so far; 0 for a new task.</value>
    [JsonPropertyName("Progress")]
    public int Progress { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the player has seen the finished task.
    /// </summary>
    /// <value><see langword="true"/> if the player has already been shown that the task is finished; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("Seen")]
    public bool Seen { get; init; }

    /// <summary>
    /// Gets or sets the global id of the task's table row.
    /// </summary>
    /// <value>The global data identifier of the event task row that defines the goal and points; 0 for an empty slot.</value>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataId { get; init; }

    /// <summary>
    /// Gets or sets the place of the task in the list.
    /// </summary>
    /// <value>The native state number: 0 empty, 1 open, 2 pinned, 3 finished, 4 failed because its timer ran out, 5 due to be replaced, 6 new and not yet seen.</value>
    [JsonPropertyName("TaskState")]
    public int TaskState { get; init; }

    /// <summary>
    /// Gets or sets the Unix time when a pinned task runs out.
    /// </summary>
    /// <value>Seconds since the Unix epoch at which a pinned task's timer expires; 0 when the task has no timer.</value>
    [JsonPropertyName("TaskTimestamp")]
    public int TaskTimestamp { get; init; }
}
