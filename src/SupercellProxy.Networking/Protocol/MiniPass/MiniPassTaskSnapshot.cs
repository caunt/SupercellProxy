using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved identity of an active Mini Pass task.</summary>
public sealed record MiniPassTaskSnapshot
{
    /// <summary>Gets the player level when the task began.</summary>
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>Gets the task data row.</summary>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataGlobalIdentifier { get; init; }
}
