using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Saved animal identity and the compact per-piece acquisition state.</summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record SanctuaryPuzzleSnapshot
{
    /// <summary>Gets the Sanctuary animal data identifier.</summary>
    [JsonPropertyName("gid")]
    public int AnimalGlobalIdentifier { get; init; }

    /// <summary>Gets whether every piece has been placed in the completed puzzle.</summary>
    [JsonPropertyName("Done")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Done { get; init; }

    /// <summary>Gets the digit-encoded states in puzzle-piece index order.</summary>
    [JsonPropertyName("PiecesV2")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PieceStates { get; init; }
}
