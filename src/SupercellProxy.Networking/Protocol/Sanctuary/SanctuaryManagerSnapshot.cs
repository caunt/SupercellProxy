using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Saved Sanctuary animal puzzles shared by the player's home modes.</summary>
public sealed record SanctuaryManagerSnapshot
{
    /// <summary>Gets the saved nonempty and completed animal puzzles.</summary>
    [JsonPropertyName("SanctuaryAnimalPuzzles")]
    public SanctuaryPuzzleSnapshot[] Puzzles { get; init; } = [];
}
