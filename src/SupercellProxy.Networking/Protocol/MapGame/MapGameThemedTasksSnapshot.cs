using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Saved progress of the Valley's themed tasks.</summary>
public sealed record MapGameThemedTasksSnapshot : ExtensibleDocument
{
    /// <summary>Day index the themed task state belongs to.</summary>
    [JsonPropertyName("CurrentDayIndex")]
    public int CurrentDayIndex { get; init; }
}
