using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Represents one booster held in the player's booster storage.</summary>
public sealed record BoosterSlotSnapshot
{
    /// <summary>Gets or sets the stored booster's data global id.</summary>
    [JsonPropertyName("BoosterId")]
    public int BoosterDataGlobalId { get; init; }

    /// <summary>Gets whether this booster is exempt from the normal storage-slot limit.</summary>
    public bool IsFree { get; init; }

    /// <summary>Gets or sets the retained native booster level.</summary>
    public int Level { get; init; }

    /// <summary>Gets or sets the retained native storage tag.</summary>
    public int Tag { get; init; }
}
