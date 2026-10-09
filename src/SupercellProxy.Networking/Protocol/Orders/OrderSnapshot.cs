using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Represents decoded <c language="csharp">OrderSnapshot</c> home data.
/// </summary>
public sealed record OrderSnapshot : ExtensibleDocument
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Datas</c> value.
    /// </summary>
    public int[] Datas { get; init; } = [];
    /// Gets the optional bonus reward amount.
    public int? BonusCount { get; init; }

    /// Gets the event id associated with this order's bonus reward.
    [JsonPropertyName("BonusEventId")]
    public int BonusEventId { get; init; }

    /// Gets the optional bonus reward data-table id.
    public int? BonusReward { get; init; }

    /// Gets whether the order's bonus reward is enabled.
    public bool? BonusRewardEnabled { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Cash</c> value.
    /// </summary>
    public int Cash { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CashExpMultiplier</c> value.
    /// </summary>
    public int CashExpMultiplier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Amounts</c> value.
    /// </summary>
    public int[] Amounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Exp</c> value.
    /// </summary>
    public int Exp { get; init; }

    /// Gets the helper completion checksum flag.
    [JsonPropertyName("HC")]
    public ProtocolFlag HelperCompleted { get; init; }


    /// Gets the helper grant checksum flag.
    [JsonPropertyName("HG")]
    public ProtocolFlag HelperGranted { get; init; }

    /// Gets the helper id checksum value.
    [JsonPropertyName("HID")]
    public int? HelperId { get; init; }

    /// Gets the helper reward data checksum value.
    [JsonPropertyName("HRD")]
    public int? HelperRewardData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Lvl</c> value.
    /// </summary>
    public int Lvl { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">New</c> value.
    /// </summary>
    public bool New { get; init; }
    /// <summary>Gets the native order-source data reference retained with a generated order.</summary>
    [JsonPropertyName("OSI")]
    public int OrderSourceDataId { get; init; }

    /// <summary>Gets the promotion reward attached to this truck order.</summary>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Receiver</c> value.
    /// </summary>
    public int Receiver { get; init; }

    /// Gets the optional reviver avatar id.
    [JsonPropertyName("ReviverAvatarId")]
    public EncodedDocumentValue? ReviverAvatarId { get; init; }

    /// Gets the seasonal-currency bonus marker.
    [JsonPropertyName("seasonalCurrencyBonus")]
    public ProtocolFlag SeasonalCurrencyBonus { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Timer</c> value.
    /// </summary>
    public TimerSnapshot? Timer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Voucher</c> value.
    /// </summary>
    public int Voucher { get; init; }
}
