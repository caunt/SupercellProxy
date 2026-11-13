using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Saved Sanctuary animal puzzles shared by the player's home modes.</summary>
public sealed record SanctuaryManagerSnapshot
{
    /// <summary>Gets or sets the saved nonempty and completed animal puzzles.</summary>
    /// <value>One entry per animal saved under <c>SanctuaryAnimalPuzzles</c>; puzzles without any received piece are not saved, and the array is empty by default when the key is missing.</value>
    [JsonPropertyName("SanctuaryAnimalPuzzles")]
    public ImmutableArray<SanctuaryPuzzleSnapshot> Puzzles { get; init; } = [];
}
