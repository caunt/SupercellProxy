using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Boats;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Rewards of one delivered truck order, kept on the delivery truck until the player collects them; an order another player filled
/// brings no rewards here.
/// </summary>
public sealed record TruckDeliveryRewardSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the extra coins from watching a video for the delivery: the order's coins and bonus coins once more.</summary>
    /// <value>The sum of <see cref="Cash"/> and <see cref="BonusCash"/> when the double-reward video was used; otherwise, 0.</value>
    public int AdBonusCash { get; init; }

    /// <summary>Gets or sets the extra experience from watching a video for the delivery: the order's experience and bonus experience once more.</summary>
    /// <value>Saved as <c>AdBonusExp</c>; the sum of <see cref="Experience"/> and <see cref="BonusExperience"/> when the video was used; otherwise, 0.</value>
    [JsonPropertyName("AdBonusExp")]
    public int AdBonusExperience { get; init; }

    /// <summary>Gets or sets the extra coins from active delivery bonuses.</summary>
    /// <value>Coins added on top of <see cref="Cash"/> by bonuses running at delivery time; 0 without bonuses or for a helped order.</value>
    public int BonusCash { get; init; }

    /// <summary>Gets or sets how many of the event bonus reward the delivery gives.</summary>
    /// <value>The order's bonus count when its bonus was enabled; otherwise, 0.</value>
    public int BonusCount { get; init; }

    /// <summary>Gets or sets the extra experience from active delivery bonuses.</summary>
    /// <value>Saved as <c>BonusExp</c>; experience added on top of <see cref="Experience"/> by bonuses running at delivery time, 0 for a helped order.</value>
    [JsonPropertyName("BonusExp")]
    public int BonusExperience { get; init; }

    /// <summary>Gets or sets the global id of the event bonus reward; 0 when there is none.</summary>
    /// <value>Saved as <c>BonusGlobalID</c>; copied from the order's bonus reward, such as an event's seasonal currency, and paid <see cref="BonusCount"/> times on collection.</value>
    [JsonPropertyName("BonusGlobalID")]
    public int BonusGlobalId { get; init; }

    /// <summary>Gets or sets the coins the order pays; 0 when another player filled it.</summary>
    /// <value>The order's coin reward, granted when the player collects from the truck.</value>
    public int Cash { get; init; }
    /// <summary>Gets or sets the experience the order gives; 0 when another player filled it.</summary>
    /// <value>Saved as <c>Exp</c>; the order's experience reward, granted when the player collects from the truck.</value>
    [JsonPropertyName("Exp")]
    public int Experience { get; init; }

    /// <summary>Gets or sets the home id of the player who filled the order, as text; <see langword="null"/> when the owner filled it.</summary>
    /// <value>Saved as <c>HelperAvatarID</c>; the helper's id in its text form.</value>
    [JsonPropertyName("HelperAvatarID")]
    public string? HelperAvatarId { get; init; }

    /// <summary>Gets or sets the saved helper account's high word.</summary>
    /// <value>Saved as <c>HelperAvatarID_hi</c>; the high 32 bits of the helper's account id, or <see langword="null"/> (left out of the JSON) when nobody helped. Used only together with <see cref="HelperAvatarIdLow"/>.</value>
    [JsonPropertyName("HelperAvatarID_hi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperAvatarIdHigh { get; init; }

    /// <summary>Gets or sets the saved helper account's low word.</summary>
    /// <value>Saved as <c>HelperAvatarID_lo</c>; the low 32 bits of the helper's account id, or <see langword="null"/> (left out of the JSON) when nobody helped.</value>
    [JsonPropertyName("HelperAvatarID_lo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperAvatarIdLow { get; init; }

    /// <summary>Gets or sets how many vouchers the delivery gives (saved as <c>VoucherCount</c>); 0 when another player filled the order.</summary>
    /// <value>1 for an order the owner filled, whether or not it carries a voucher; 0 for a helped order.</value>
    [JsonPropertyName("VoucherCount")]
    public int ItemCount { get; init; }

    /// <summary>Gets or sets the voucher the delivery gives; 0 when there is none.</summary>
    /// <value>Saved as <c>ItemGlobalID</c>, but holding the voucher's type number from the voucher table's <c>VoucherType</c> column, copied from the order.</value>
    [JsonPropertyName("ItemGlobalID")]
    public int ItemGlobalId { get; init; }

    /// <summary>Gets or sets the promotion box carried by the delivery.</summary>
    /// <value>Saved as <c>PopPromoBox</c>; the order's promotion box, or <see langword="null"/> (left out of the JSON) when the order had none or another player filled it.</value>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }
}
