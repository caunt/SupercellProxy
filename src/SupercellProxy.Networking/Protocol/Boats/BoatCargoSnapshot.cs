using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// One crate of a saved boat order: the goods it asks for and whether it has been filled.
/// </summary>
public sealed record BoatCargoSnapshot
{
    /// <summary>
    /// How many of the goods fill the crate.
    /// </summary>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// Whether the crate has been filled, by the owner or by a helper.
    /// </summary>
    [JsonPropertyName("completed")]
    public bool Completed { get; init; }

    /// <summary>
    /// Global id of the goods the crate asks for.
    /// </summary>
    [JsonPropertyName("data_global_id")]
    public int DataGlobalId { get; init; }
    /// <summary>
    /// Whether a booster filled the crate instead of goods from the barn (saved as <c>boosted</c>).
    /// </summary>
    [JsonPropertyName("boosted")]
    public bool FilledByBooster { get; init; }

    /// <summary>Gets the optional promotion reward attached to this crate.</summary>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }

    /// <summary>
    /// Whether the owner already sent a thank-you gift to the helper who filled the crate (saved as <c>gifted</c>).
    /// </summary>
    [JsonPropertyName("gifted")]
    public bool ThankYouSent { get; init; }
}
