using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved identity of an active Mini Pass task.</summary>
public sealed record MiniPassTaskSnapshot
{
    /// <summary>Gets or sets the player level when the task began.</summary>
    /// <value>The farm's experience level at the moment the task was assigned.</value>
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>Gets or sets the task data row.</summary>
    /// <value>Saved as <c>TaskDataId</c>; the global id of the task row, whose <c>Amount</c> is the progress needed and <c>FarmPoints</c> the pass points awarded on completion.</value>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataGlobalId { get; init; }
}
