using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Valley state saved with a home: the cars on the board and the themed task progress.</summary>
public sealed record MapGameHomeManagerSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the saved cars of the board's players; <see langword="null"/> when the home has none.</summary>
    /// <value>One entry per player car on the Valley board, saved under <c>Pawns</c> and left out of the JSON when <see langword="null"/>.</value>
    [JsonPropertyName("Pawns")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImmutableArray<MapGameHomePawnSnapshot>? Pawns { get; init; }

    /// <summary>Gets or sets the themed task progress (saved as <c>ThemedTasksManager</c>); <see langword="null"/> when not saved.</summary>
    /// <value>The themed tasks manager's saved state, left out of the JSON when <see langword="null"/>.</value>
    [JsonPropertyName("ThemedTasksManager")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameThemedTasksSnapshot? ThemedTasks { get; init; }
}
