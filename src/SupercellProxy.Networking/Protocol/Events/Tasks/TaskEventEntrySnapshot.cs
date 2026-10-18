using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// One saved task of a task event: a task-table row, the player's progress on it and its place in the task list.
/// </summary>
public sealed record TaskEventEntrySnapshot
{
    /// <summary>
    /// Whether the progress reached the task's goal.
    /// </summary>
    [JsonPropertyName("Complete")]
    public bool Complete { get; init; }

    /// <summary>
    /// Player level when the task was created.
    /// </summary>
    [JsonPropertyName("PlayerLevelAtTaskStart")]
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>
    /// Event points the task gives instead of its table points; 0 when the table points apply.
    /// </summary>
    [JsonPropertyName("PointsOverride")]
    public int PointsOverride { get; init; }

    /// <summary>
    /// How much of the task's goal is done.
    /// </summary>
    [JsonPropertyName("Progress")]
    public int Progress { get; init; }

    /// <summary>
    /// Whether the player has seen the finished task.
    /// </summary>
    [JsonPropertyName("Seen")]
    public bool Seen { get; init; }

    /// <summary>
    /// Global id of the task's table row; 0 for an empty slot.
    /// </summary>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataId { get; init; }

    /// <summary>
    /// Place of the task in the list, as native numbers it: 0 empty, 1 open, 2 pinned, 3 finished, 4 failed because its timer ran
    /// out, 5 due to be replaced, 6 new and not yet seen.
    /// </summary>
    [JsonPropertyName("TaskState")]
    public int TaskState { get; init; }

    /// <summary>
    /// Unix seconds when a pinned task runs out; 0 when the task has no timer.
    /// </summary>
    [JsonPropertyName("TaskTimestamp")]
    public int TaskTimestamp { get; init; }
}
