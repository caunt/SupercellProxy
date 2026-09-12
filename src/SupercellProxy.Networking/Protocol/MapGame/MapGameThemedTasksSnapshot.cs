using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents decoded MapGameThemedTasksSnapshot state.</summary>
public sealed record MapGameThemedTasksSnapshot : ExtensibleDocument
{
    /// <summary>Gets the CurrentDayIndex value.</summary>
    [JsonPropertyName("CurrentDayIndex")]
    public int CurrentDayIndex { get; init; }

}
