using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>A neighbourhood event task the player finished, kept so new tasks avoid repeating it.</summary>
public sealed record CompletedNeighborhoodTaskSnapshot : ExtensibleDocument
{
    /// <summary>Whether the progress reached the task's goal.</summary>
    [JsonPropertyName("Complete")]
    public bool Complete { get; init; }

    /// <summary>Player level when the task was picked.</summary>
    [JsonPropertyName("PlayerLevelAtTaskStart")]
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>How much of the task's goal was done.</summary>
    [JsonPropertyName("Progress")]
    public int Progress { get; init; }

    /// <summary>Whether the player has seen the task.</summary>
    [JsonPropertyName("Seen")]
    public bool Seen { get; init; }

    /// <summary>Global id of the task's table entry.</summary>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataId { get; init; }
}
