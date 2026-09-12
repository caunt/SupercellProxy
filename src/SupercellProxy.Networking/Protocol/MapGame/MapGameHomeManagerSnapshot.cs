using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents decoded MapGameHomeManagerSnapshot state.</summary>
public sealed record MapGameHomeManagerSnapshot : ExtensibleDocument
{
    /// <summary>Gets the Pawns value.</summary>
    [JsonPropertyName("Pawns")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameHomePawnSnapshot[]? Pawns { get; init; }

    /// <summary>Gets the ThemedTasks value.</summary>
    [JsonPropertyName("ThemedTasksManager")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameThemedTasksSnapshot? ThemedTasks { get; init; }

}
