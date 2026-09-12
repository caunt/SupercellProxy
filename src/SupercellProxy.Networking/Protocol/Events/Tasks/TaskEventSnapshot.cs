using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Defines the Task Event Snapshot contract.
/// </summary>
public sealed record TaskEventSnapshot
{

    /// <summary>
    /// Gets the Opened By Player value.
    /// </summary>
    [JsonPropertyName("openedByPlayer")]
    public bool OpenedByPlayer { get; init; }
    /// <summary>
    /// Gets the Visible Tasks value.
    /// </summary>
    [JsonPropertyName("visibleTasks")]
    public TaskEventEntrySnapshot?[] VisibleTasks { get; init; } = [];
}
