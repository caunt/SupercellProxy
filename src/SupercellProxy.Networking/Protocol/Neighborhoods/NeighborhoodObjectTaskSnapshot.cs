using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>One task in a neighbourhood event task slot and the player's progress on it.</summary>
public sealed record NeighborhoodObjectTaskSnapshot
{
    /// <summary>Gets or sets whether the task has reached its required progress.</summary>
    /// <value><see langword="true"/> if <see cref="Progress"/> has reached the required amount; otherwise, <see langword="false"/>.</value>
    public bool Complete { get; init; }

    /// <summary>Gets or sets the player level when the task was picked.</summary>
    /// <value>The farm's experience level at pick time.</value>
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>Gets or sets the saved task progress.</summary>
    /// <value>How much has been done toward the required amount, 0 for a new task.</value>
    public int Progress { get; init; }

    /// <summary>Gets or sets whether the player has seen the task.</summary>
    /// <value><see langword="true"/> if the player's tasks were marked seen while this one was in its slot; otherwise, <see langword="false"/>.</value>
    public bool Seen { get; init; }

    /// <summary>Gets or sets the global id of the task's table entry.</summary>
    /// <value>A global id in the neighbourhood object task table, which holds the task type, the goods it asks for, the amounts and the points it awards.</value>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataId { get; init; }

    /// <summary>Gets or sets a state number that is 0 for a task that occupies its slot; other values are not identified.</summary>
    /// <value>The saved state number; 0 for every task drawn into a slot.</value>
    public int TaskState { get; init; }
}
