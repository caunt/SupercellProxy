using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// One crate of a saved boat order: the goods it asks for and whether it has been filled.
/// </summary>
public sealed record BoatCargoSnapshot
{
    /// <summary>
    /// Gets or sets how many of the goods fill the crate.
    /// </summary>
    /// <value>The number of <see cref="DataGlobalId"/> goods the crate needs, saved as <c>amount</c>; 0 when the save omits it.</value>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the crate has been filled, by the owner or by a helper (saved as <c>completed</c>).
    /// </summary>
    /// <value><see langword="true"/> if the crate is filled; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("completed")]
    public bool Completed { get; init; }

    /// <summary>
    /// Gets or sets the global id of the goods the crate asks for.
    /// </summary>
    /// <value>The data global id of the requested product, saved as <c>data_global_id</c>.</value>
    [JsonPropertyName("data_global_id")]
    public int DataGlobalId { get; init; }
    /// <summary>
    /// Gets or sets a value indicating whether a booster filled the crate instead of goods from the barn (saved as <c>boosted</c>).
    /// </summary>
    /// <value><see langword="true"/> if a booster completed the crate; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("boosted")]
    public bool FilledByBooster { get; init; }

    /// <summary>Gets or sets the optional promotion reward attached to this crate.</summary>
    /// <value>The promotion box saved as <c>PopPromoBox</c>, or <see langword="null"/> when the crate carries none; a <see langword="null"/> value is left out when the snapshot is written back.</value>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the owner already sent a thank-you gift to the helper who filled the crate (saved as <c>gifted</c>).
    /// </summary>
    /// <value><see langword="true"/> if the thank-you gift was already sent; otherwise, <see langword="false"/>.</value>
    /// <remarks>A crate nobody helped with reads as <see langword="false"/>.</remarks>
    [JsonPropertyName("gifted")]
    public bool ThankYouSent { get; init; }
}
