using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Saved progress of the Valley's themed tasks.</summary>
public sealed record MapGameThemedTasksSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the day index the themed task state belongs to.</summary>
    /// <value>The day number the saved themed task progress was made on; 0 by default when not saved.</value>
    [JsonPropertyName("CurrentDayIndex")]
    public int CurrentDayIndex { get; init; }
}
