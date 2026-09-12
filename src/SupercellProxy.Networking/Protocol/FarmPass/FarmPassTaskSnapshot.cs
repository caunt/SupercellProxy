namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Defines the Farm Pass Task Snapshot contract.
/// </summary>
public sealed record FarmPassTaskSnapshot
{

    /// <summary>
    /// Gets the Complete value.
    /// </summary>
    public bool Complete { get; init; }

    /// <summary>
    /// Gets the Player Level At Task Start value.
    /// </summary>
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>
    /// Gets the Progress value.
    /// </summary>
    public int Progress { get; init; }

    /// <summary>
    /// Gets the Seen value.
    /// </summary>
    public bool Seen { get; init; }
    /// <summary>
    /// Gets the Task Data Id value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TaskDataId")]
    public int TaskDataIdentifier { get; init; }

    /// <summary>
    /// Gets the Task State value.
    /// </summary>
    public int TaskState { get; init; }
}
