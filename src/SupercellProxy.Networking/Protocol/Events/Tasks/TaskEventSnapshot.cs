using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Saved per-player state of a task event: whether the player has opened it and how each visible task is displayed.
/// </summary>
public sealed record TaskEventSnapshot
{
    /// <summary>
    /// Whether the player has opened the event at least once.
    /// </summary>
    [JsonPropertyName("openedByPlayer")]
    public bool OpenedByPlayer { get; init; }
    /// <summary>
    /// Display state of each task slot in the event's list, in slot order; empty slots are null.
    /// </summary>
    [JsonPropertyName("visibleTasks")]
    public TaskEventEntrySnapshot?[] VisibleTasks { get; init; } = [];
}
