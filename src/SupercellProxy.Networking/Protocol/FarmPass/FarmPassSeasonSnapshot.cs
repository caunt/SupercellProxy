using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Defines the Farm Pass Season Snapshot contract.
/// </summary>
public sealed record FarmPassSeasonSnapshot
{

    /// <summary>
    /// Gets the Baby Pet Data value.
    /// </summary>
    [JsonPropertyName("babyPetData")]
    public int BabyPetData { get; init; }
    /// <summary>
    /// Gets the Chronos Event Id value.
    /// </summary>
    [JsonPropertyName("chronosEventId")]
    public int ChronosEventIdentifier { get; init; }

    /// <summary>
    /// Gets the Farm Pass Thresholds value.
    /// </summary>
    [JsonPropertyName("farmPassThresholds")]
    public int[] FarmPassThresholds { get; init; } = [];

    /// <summary>
    /// Gets the Farm Points value.
    /// </summary>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>
    /// Gets the Levels value.
    /// </summary>
    [JsonPropertyName("levels")]
    public FarmPassLevelSnapshot[] Levels { get; init; } = [];

    /// <summary>
    /// Gets the Purchased Tiers value.
    /// </summary>
    [JsonPropertyName("purchasedTiers")]
    public int PurchasedTiers { get; init; }

    /// <summary>
    /// Gets the Road Data value.
    /// </summary>
    [JsonPropertyName("roadData")]
    public int RoadData { get; init; }

    /// <summary>
    /// Gets the Seen Points value.
    /// </summary>
    [JsonPropertyName("seenPoints")]
    public int SeenPoints { get; init; }
}
