using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Saved state of the season the player joined: its levels, the points earned, and the passes bought.</summary>
public sealed record FarmPassSeasonSnapshot
{
    /// <summary>Global id of the season's baby pet table entry.</summary>
    [JsonPropertyName("babyPetData")]
    public int BabyPetData { get; init; }

    /// <summary>Id of the live event the season belongs to.</summary>
    [JsonPropertyName("chronosEventId")]
    public int ChronosEventId { get; init; }

    /// <summary>Season point marks copied from the event, which split the season into stages.</summary>
    [JsonPropertyName("farmPassThresholds")]
    public int[] FarmPassThresholds { get; init; } = [];

    /// <summary>Season points the player earned so far.</summary>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>The season's levels in order.</summary>
    [JsonPropertyName("levels")]
    public FarmPassLevelSnapshot[] Levels { get; init; } = [];

    /// <summary>Which passes the player bought, as bits; bit 0 unlocks the premium rewards.</summary>
    [JsonPropertyName("purchasedTiers")]
    public int PurchasedTiers { get; init; }

    /// <summary>Global id of the season's Farm Pass road table entry.</summary>
    [JsonPropertyName("roadData")]
    public int RoadData { get; init; }

    /// <summary>Season points at the last time the player looked at the pass.</summary>
    [JsonPropertyName("seenPoints")]
    public int SeenPoints { get; init; }

    /// <summary>Gets whether the tier containing the last seen points was purchased.</summary>
    [JsonPropertyName("seenPurchased")]
    public bool SeenPurchased { get; init; }
}
