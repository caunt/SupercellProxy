using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Boats;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Contains the rewards retained by a truck delivery.</summary>
public sealed record TruckDeliveryRewardSnapshot : ExtensibleDocument
{
    /// <summary>Gets the AdBonusCash value.</summary>
    public int AdBonusCash { get; init; }

    /// <summary>Gets the AdBonusExperience value.</summary>
    [JsonPropertyName("AdBonusExp")]
    public int AdBonusExperience { get; init; }

    /// <summary>Gets the BonusCash value.</summary>
    public int BonusCash { get; init; }

    /// <summary>Gets the BonusCount value.</summary>
    public int BonusCount { get; init; }

    /// <summary>Gets the BonusExperience value.</summary>
    [JsonPropertyName("BonusExp")]
    public int BonusExperience { get; init; }

    /// <summary>Gets the BonusGlobalId value.</summary>
    [JsonPropertyName("BonusGlobalID")]
    public int BonusGlobalId { get; init; }

    /// <summary>Gets the Cash value.</summary>
    public int Cash { get; init; }
    /// <summary>Gets the Experience value.</summary>
    [JsonPropertyName("Exp")]
    public int Experience { get; init; }

    /// <summary>Gets the HelperAvatarId value.</summary>
    [JsonPropertyName("HelperAvatarID")]
    public string? HelperAvatarId { get; init; }

    /// <summary>Gets the ItemCount value.</summary>
    [JsonPropertyName("VoucherCount")]
    public int ItemCount { get; init; }

    /// <summary>Gets the ItemGlobalId value.</summary>
    [JsonPropertyName("ItemGlobalID")]
    public int ItemGlobalId { get; init; }

    /// <summary>Gets the promotion box carried by the delivery.</summary>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }
}
