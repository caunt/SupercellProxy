using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Saved animal identity and the compact per-piece acquisition state.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record SanctuaryPuzzleSnapshot
{
    /// <summary>Gets or sets the Sanctuary animal data id.</summary>
    /// <value>The global data id of the animal the puzzle belongs to, saved under <c>gid</c>; 0 when the key is missing.</value>
    [JsonPropertyName("gid")]
    public int AnimalGlobalId { get; init; }

    /// <summary>Gets or sets a value indicating whether every piece has been placed in the completed puzzle.</summary>
    /// <value><see langword="true"/> if the puzzle is complete and saved without <see cref="PieceStates"/>; otherwise, <see langword="false"/>.</value>
    /// <remarks>An incomplete puzzle is saved without the <c>Done</c> key, and a save that omits it reads as incomplete.</remarks>
    [JsonPropertyName("Done")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Done { get; init; }

    /// <summary>Gets or sets the digit-encoded states in puzzle-piece index order.</summary>
    /// <value>The <c>PiecesV2</c> text with one digit per piece: 0 for missing, 1 for received but not yet placed, and 2 for placed; <see langword="null"/>, and left out on write, for a completed puzzle.</value>
    [JsonPropertyName("PiecesV2")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PieceStates { get; init; }
}
