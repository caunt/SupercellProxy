using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>One daily or weekly Farm Pass task and the player's progress on it.</summary>
public sealed record FarmPassTaskSnapshot
{
    /// <summary>Gets or sets whether the progress reached the task's goal.</summary>
    /// <value><see langword="true"/> if <see cref="Progress"/> reached the task's <c>Amount</c>; otherwise, <see langword="false"/>.</value>
    public bool Complete { get; init; }

    /// <summary>Gets or sets the player level when the task was picked.</summary>
    /// <value>The player's experience level at the moment the task was drawn.</value>
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>Gets or sets how much of the task's goal is done.</summary>
    /// <value>A count in the task's own unit (goods, actions and so on), starting at 0; it may pass the goal.</value>
    public int Progress { get; init; }

    /// <summary>Gets or sets whether the player has seen the task.</summary>
    /// <value><see langword="true"/> if the client has shown the task to the player; otherwise, <see langword="false"/>.</value>
    public bool Seen { get; init; }

    /// <summary>Gets or sets the global id of the task's table entry.</summary>
    /// <value>A Farm Pass task row whose <c>TaskType</c>, <c>Good</c>, <c>Amount</c> and <c>FarmPoints</c> columns define what counts, the goal and the reward.</value>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataId { get; init; }

    /// <summary>Gets or sets the task's state: 0 for an open task; 2 for an extra weekly task still locked behind diamonds.</summary>
    /// <value>The saved state code; other values are not identified.</value>
    public int TaskState { get; init; }
}
