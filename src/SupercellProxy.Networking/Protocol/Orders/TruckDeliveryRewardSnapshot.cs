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
    /// <summary>Extra coins from watching a video for the delivery: the order's coins and bonus coins once more.</summary>
    public int AdBonusCash { get; init; }

    /// <summary>Extra experience from watching a video for the delivery: the order's experience and bonus experience once more.</summary>
    [JsonPropertyName("AdBonusExp")]
    public int AdBonusExperience { get; init; }

    /// <summary>Extra coins from active delivery bonuses.</summary>
    public int BonusCash { get; init; }

    /// <summary>How many of the event bonus reward the delivery gives.</summary>
    public int BonusCount { get; init; }

    /// <summary>Extra experience from active delivery bonuses.</summary>
    [JsonPropertyName("BonusExp")]
    public int BonusExperience { get; init; }

    /// <summary>Global id of the event bonus reward; 0 when there is none.</summary>
    [JsonPropertyName("BonusGlobalID")]
    public int BonusGlobalId { get; init; }

    /// <summary>Coins the order pays; 0 when another player filled it.</summary>
    public int Cash { get; init; }
    /// <summary>Experience the order gives; 0 when another player filled it.</summary>
    [JsonPropertyName("Exp")]
    public int Experience { get; init; }

    /// <summary>Home id of the player who filled the order, as text; null when the owner filled it.</summary>
    [JsonPropertyName("HelperAvatarID")]
    public string? HelperAvatarId { get; init; }

    /// <summary>Gets the saved helper account's high word.</summary>
    [JsonPropertyName("HelperAvatarID_hi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperAvatarIdHigh { get; init; }

    /// <summary>Gets the saved helper account's low word.</summary>
    [JsonPropertyName("HelperAvatarID_lo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HelperAvatarIdLow { get; init; }

    /// <summary>How many vouchers the delivery gives (saved as <c>VoucherCount</c>); 0 when another player filled the order.</summary>
    [JsonPropertyName("VoucherCount")]
    public int ItemCount { get; init; }

    /// <summary>Global id of the voucher the delivery gives; 0 when there is none.</summary>
    [JsonPropertyName("ItemGlobalID")]
    public int ItemGlobalId { get; init; }

    /// <summary>Gets the promotion box carried by the delivery.</summary>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }
}
