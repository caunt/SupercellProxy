using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

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

    /// <summary>Gets the BonusGlobalIdentifier value.</summary>
    [JsonPropertyName("BonusGlobalID")]
    public int BonusGlobalIdentifier { get; init; }
    /// <summary>Gets the Cash value.</summary>
    public int Cash { get; init; }

    /// <summary>Gets the Experience value.</summary>
    [JsonPropertyName("Exp")]
    public int Experience { get; init; }

    /// <summary>Gets the HelperAvatarIdentifier value.</summary>
    [JsonPropertyName("HelperAvatarID")]
    public string? HelperAvatarIdentifier { get; init; }

    /// <summary>Gets the ItemCount value.</summary>
    [JsonPropertyName("VoucherCount")]
    public int ItemCount { get; init; }

    /// <summary>Gets the ItemGlobalIdentifier value.</summary>
    [JsonPropertyName("ItemGlobalID")]
    public int ItemGlobalIdentifier { get; init; }

}
