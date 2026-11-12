using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Saved state of the season the player joined: its levels, the points earned, and the passes bought.</summary>
public sealed record FarmPassSeasonSnapshot
{
    /// <summary>Gets or sets the global id of the season's baby pet table entry.</summary>
    /// <value>A row of the baby pets table, the one the event's <c>farmPassBabyPet</c> name refers to.</value>
    [JsonPropertyName("babyPetData")]
    public int BabyPetData { get; init; }

    /// <summary>Gets or sets the id of the live event the season belongs to.</summary>
    /// <value>The Farm Pass live event id, saved as <c>chronosEventId</c>.</value>
    [JsonPropertyName("chronosEventId")]
    public int ChronosEventId { get; init; }

    /// <summary>Gets or sets the season point marks copied from the event, which split the season into stages.</summary>
    /// <value>Rising season-point totals, one per stage boundary; empty by default.</value>
    [JsonPropertyName("farmPassThresholds")]
    public int[] FarmPassThresholds { get; init; } = [];

    /// <summary>Gets or sets the season points the player earned so far.</summary>
    /// <value>A running season-point total, saved as <c>farmPoints</c>.</value>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>Gets or sets the season's levels in order.</summary>
    /// <value>One saved level per reward tier of the event, first level at index 0; level claim commands address levels by this index.</value>
    [JsonPropertyName("levels")]
    public FarmPassLevelSnapshot[] Levels { get; init; } = [];

    /// <summary>Gets or sets which passes the player bought, as bits; bit 0 is the premium pass.</summary>
    /// <value>A bit mask that is 0 for a player who bought no pass.</value>
    [JsonPropertyName("purchasedTiers")]
    public int PurchasedTiers { get; init; }

    /// <summary>Gets or sets the global id of the season's Farm Pass road table entry.</summary>
    /// <value>A row of the Farm Pass road table, the one the event's <c>farmPassRoad</c> name refers to.</value>
    [JsonPropertyName("roadData")]
    public int RoadData { get; init; }

    /// <summary>Gets or sets the season points at the last time the player looked at the pass.</summary>
    /// <value>A copy of <see cref="FarmPoints"/> taken when the points are marked seen; the difference to the current total is what the player has not seen yet.</value>
    [JsonPropertyName("seenPoints")]
    public int SeenPoints { get; init; }

    /// <summary>Gets or sets whether the tier containing the last seen points was purchased.</summary>
    /// <value><see langword="true"/> if the stage holding <see cref="SeenPoints"/> was bought when the points were last marked seen; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("seenPurchased")]
    public bool SeenPurchased { get; init; }
}
