using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>A neighbourhood event task the player finished, kept so new tasks avoid repeating it.</summary>
public sealed record CompletedNeighborhoodTaskSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets whether the progress reached the task's goal.</summary>
    /// <value><see langword="true"/> if the task had reached its goal when it was turned in; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("Complete")]
    public bool Complete { get; init; }

    /// <summary>Gets or sets the player level when the task was picked.</summary>
    /// <value>The farm's experience level at pick time, saved as <c>PlayerLevelAtTaskStart</c>.</value>
    [JsonPropertyName("PlayerLevelAtTaskStart")]
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>Gets or sets how much of the task's goal was done.</summary>
    /// <value>The progress the task had when it was turned in.</value>
    [JsonPropertyName("Progress")]
    public int Progress { get; init; }

    /// <summary>Gets or sets whether the player has seen the task.</summary>
    /// <value><see langword="true"/> if the task had been marked seen in its slot when it was turned in; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("Seen")]
    public bool Seen { get; init; }

    /// <summary>Gets or sets the global id of the task's table entry.</summary>
    /// <value>A global id in the neighbourhood object task table, saved as <c>TaskDataId</c>.</value>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataId { get; init; }
}
