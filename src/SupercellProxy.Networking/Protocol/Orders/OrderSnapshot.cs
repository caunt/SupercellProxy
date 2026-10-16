using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Saved state of one truck order on the order board, or of an empty slot waiting for its next order.</summary>
public sealed record OrderSnapshot : ExtensibleDocument
{
    /// <summary>Global ids of the goods the order asks for, matching <see cref="Amounts"/> by position; empty for an empty slot.</summary>
    public int[] Datas { get; init; } = [];
    /// <summary>Gets the optional bonus reward amount.</summary>
    public int? BonusCount { get; init; }

    /// <summary>Gets the event id associated with this order's bonus reward.</summary>
    [JsonPropertyName("BonusEventId")]
    public int BonusEventId { get; init; }

    /// <summary>Gets the optional bonus reward data-table id.</summary>
    public int? BonusReward { get; init; }

    /// <summary>Gets whether the order's bonus reward is enabled.</summary>
    public bool? BonusRewardEnabled { get; init; }

    /// <summary>Coins the order pays when delivered.</summary>
    public int Cash { get; init; }

    /// <summary>
    /// Percentage that shifts the reward between coins and experience: bonus coins grow by it and bonus experience shrinks by it.
    /// </summary>
    public int CashExpMultiplier { get; init; }

    /// <summary>How many of each good the order asks for, matching <see cref="Datas"/> by position.</summary>
    public int[] Amounts { get; init; } = [];

    /// <summary>Experience the order gives when delivered.</summary>
    public int Exp { get; init; }

    /// <summary>Helper completion flag, saved under the key HC.</summary>
    [JsonPropertyName("HC")]
    public ProtocolFlag HelperCompleted { get; init; }


    /// <summary>Helper grant flag, saved under the key HG.</summary>
    [JsonPropertyName("HG")]
    public ProtocolFlag HelperGranted { get; init; }

    /// <summary>Help id of the order's helper, saved under the key HID.</summary>
    [JsonPropertyName("HID")]
    public int? HelperId { get; init; }

    /// <summary>Helper reward data, saved under the key HRD.</summary>
    [JsonPropertyName("HRD")]
    public int? HelperRewardData { get; init; }

    /// <summary>Player level the order was made for; always at least 1.</summary>
    public int Lvl { get; init; }

    /// <summary>Whether the order still shows its "new" badge because the player has not opened the board since it arrived.</summary>
    public bool New { get; init; }
    /// <summary>Gets the native order-source data reference retained with a generated order.</summary>
    [JsonPropertyName("OSI")]
    public int OrderSourceDataId { get; init; }

    /// <summary>Gets the promotion reward attached to this truck order.</summary>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }

    /// <summary>Global id of the customer who placed the order; 0 for an empty slot.</summary>
    public int Receiver { get; init; }

    /// <summary>Gets the optional reviver avatar id.</summary>
    [JsonPropertyName("ReviverAvatarId")]
    public EncodedDocumentValue? ReviverAvatarId { get; init; }

    /// <summary>Gets the seasonal-currency bonus marker.</summary>
    [JsonPropertyName("seasonalCurrencyBonus")]
    public ProtocolFlag SeasonalCurrencyBonus { get; init; }

    /// <summary>Countdown of an empty slot until its next order arrives; null when the slot has no countdown.</summary>
    public TimerSnapshot? Timer { get; init; }

    /// <summary>Global id of the voucher the order gives besides coins and experience; 0 when it gives none.</summary>
    public int Voucher { get; init; }
}
