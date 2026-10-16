using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>One task in a neighbourhood event task slot and the player's progress on it.</summary>
public sealed record NeighborhoodObjectTaskSnapshot
{
    /// <summary>Gets whether the task has reached its required progress.</summary>
    public bool Complete { get; init; }

    /// <summary>Player level when the task was picked.</summary>
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>Gets the saved task progress.</summary>
    public int Progress { get; init; }

    /// <summary>Whether the player has seen the task.</summary>
    public bool Seen { get; init; }

    /// <summary>Global id of the task's table entry.</summary>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataId { get; init; }
    /// <summary>0 for a task that occupies its slot; other values are not identified.</summary>
    public int TaskState { get; init; }
}
