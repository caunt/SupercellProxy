using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Valley state saved with a home: the cars on the board and the themed task progress.</summary>
public sealed record MapGameHomeManagerSnapshot : ExtensibleDocument
{
    /// <summary>Saved cars of the board's players; null when the home has none.</summary>
    [JsonPropertyName("Pawns")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameHomePawnSnapshot[]? Pawns { get; init; }

    /// <summary>Themed task progress (saved as <c>ThemedTasksManager</c>); null when not saved.</summary>
    [JsonPropertyName("ThemedTasksManager")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameThemedTasksSnapshot? ThemedTasks { get; init; }
}
