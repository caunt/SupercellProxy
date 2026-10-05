using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Boat Cargo Snapshot contract.
/// </summary>
public sealed record BoatCargoSnapshot
{
    /// <summary>
    /// Gets the Amount value.
    /// </summary>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// Gets the Completed value.
    /// </summary>
    [JsonPropertyName("completed")]
    public bool Completed { get; init; }

    /// <summary>
    /// Gets the Data Global Id value.
    /// </summary>
    [JsonPropertyName("data_global_id")]
    public int DataGlobalId { get; init; }
    /// <summary>
    /// Gets the FilledByBooster value.
    /// </summary>
    [JsonPropertyName("boosted")]
    public bool FilledByBooster { get; init; }

    /// <summary>Gets the optional promotion reward attached to this crate.</summary>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }

    /// <summary>
    /// Gets the ThankYouSent value.
    /// </summary>
    [JsonPropertyName("gifted")]
    public bool ThankYouSent { get; init; }
}
