using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>One daily or weekly Farm Pass task and the player's progress on it.</summary>
public sealed record FarmPassTaskSnapshot
{
    /// <summary>Whether the progress reached the task's goal.</summary>
    public bool Complete { get; init; }

    /// <summary>Player level when the task was picked.</summary>
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>How much of the task's goal is done.</summary>
    public int Progress { get; init; }

    /// <summary>Whether the player has seen the task.</summary>
    public bool Seen { get; init; }

    /// <summary>Global id of the task's table entry.</summary>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataId { get; init; }

    /// <summary>0 for an open task; 2 for an extra weekly task still locked behind diamonds. Only open tasks make progress.</summary>
    public int TaskState { get; init; }
}
