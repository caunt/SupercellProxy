using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Represents one booster held in the player's booster storage.</summary>
public sealed record BoosterSlotSnapshot
{
    /// <summary>Gets or sets the stored booster's data global id.</summary>
    /// <value>The global id of the booster's row in the boosters table, saved as <c>BoosterId</c>.</value>
    [JsonPropertyName("BoosterId")]
    public int BoosterDataGlobalId { get; init; }

    /// <summary>Gets or sets a value indicating whether this booster is exempt from the normal storage-slot limit.</summary>
    /// <value><see langword="true"/> if the booster does not take up one of the limited storage slots; otherwise, <see langword="false"/>.</value>
    public bool IsFree { get; init; }

    /// <summary>Gets or sets the retained native booster level.</summary>
    /// <value>The level number native saves with the slot, carried through unchanged; what the level changes is not yet identified.</value>
    public int Level { get; init; }

    /// <summary>Gets or sets the retained native storage tag.</summary>
    /// <value>The tag number native saves with the slot, carried through unchanged; its meaning is not yet identified.</value>
    public int Tag { get; init; }
}
