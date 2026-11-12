using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Saved state of one truck order on the order board, or of an empty slot waiting for its next order.</summary>
public sealed record OrderSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the global ids of the goods the order asks for, matching <see cref="Amounts"/> by position; empty for an empty slot.</summary>
    /// <value>Global ids of crop, product or other goods rows, one per entry of <see cref="Amounts"/>.</value>
    public int[] Datas { get; init; } = [];
    /// <summary>Gets or sets the optional bonus reward amount.</summary>
    /// <value>How many of <see cref="BonusReward"/> the delivery adds, such as an event's seasonal currency; <see langword="null"/> or 0 when the order has no bonus.</value>
    public int? BonusCount { get; init; }

    /// <summary>Gets or sets the event id associated with this order's bonus reward.</summary>
    /// <value>The id of the truck order live event that attached the bonus, or 0 when no event did.</value>
    [JsonPropertyName("BonusEventId")]
    public int BonusEventId { get; init; }

    /// <summary>Gets or sets the optional bonus reward data-table id.</summary>
    /// <value>The global id of the bonus item, such as a seasonal currency or an event reward; <see langword="null"/> or 0 when the order has none.</value>
    public int? BonusReward { get; init; }

    /// <summary>Gets or sets whether the order's bonus reward is enabled.</summary>
    /// <value>
    /// <see langword="true"/> if the bonus is paid on delivery; otherwise, <see langword="false"/>. When <see langword="null"/>, the bonus counts
    /// as enabled whenever <see cref="BonusReward"/> or <see cref="BonusCount"/> is set.
    /// </value>
    public bool? BonusRewardEnabled { get; init; }

    /// <summary>Gets or sets the coins the order pays when delivered.</summary>
    /// <value>The coin reward fixed when the order was generated; 0 for an empty slot.</value>
    public int Cash { get; init; }

    /// <summary>
    /// Gets or sets the percentage that shifts the order's reward between coins and experience.
    /// </summary>
    /// <value>A signed percentage; 0 leaves the split even.</value>
    public int CashExpMultiplier { get; init; }

    /// <summary>Gets or sets how many of each good the order asks for, matching <see cref="Datas"/> by position.</summary>
    /// <value>Positive counts, one per entry of <see cref="Datas"/>; empty for an empty slot.</value>
    public int[] Amounts { get; init; } = [];

    /// <summary>Gets or sets the experience the order gives when delivered.</summary>
    /// <value>The experience reward fixed when the order was generated; 0 for an empty slot.</value>
    public int Exp { get; init; }

    /// <summary>Gets or sets the helper completion flag, saved under the key HC.</summary>
    /// <value>A flag of the order's help request whose exact meaning is not identified.</value>
    [JsonPropertyName("HC")]
    public ProtocolFlag HelperCompleted { get; init; }


    /// <summary>Gets or sets the helper grant flag, saved under the key HG.</summary>
    /// <value>A second flag of the order's help request whose exact meaning is not identified.</value>
    [JsonPropertyName("HG")]
    public ProtocolFlag HelperGranted { get; init; }

    /// <summary>Gets or sets the help id of the order's helper, saved under the key HID.</summary>
    /// <value>The id of the help request the owner posted for this order; <see langword="null"/> or 0 when the order does not ask for help.</value>
    [JsonPropertyName("HID")]
    public int? HelperId { get; init; }

    /// <summary>Gets or sets the helper reward data, saved under the key HRD.</summary>
    /// <value>The reward data sent with the order's help request; <see langword="null"/> or 0 when the order does not ask for help.</value>
    [JsonPropertyName("HRD")]
    public int? HelperRewardData { get; init; }

    /// <summary>Gets or sets the player level the order was made for; always at least 1.</summary>
    /// <value>The farm's experience level when the order was generated.</value>
    public int Lvl { get; init; }

    /// <summary>Gets or sets whether the order still shows its "new" badge because the player has not opened the board since it arrived.</summary>
    /// <value><see langword="true"/> if the order arrived after the board was last opened; otherwise, <see langword="false"/>.</value>
    public bool New { get; init; }
    /// <summary>Gets or sets the native order-source data reference retained with a generated order.</summary>
    /// <value>Saved as <c>OSI</c>; a global id kept with the order, or 0 when there is none. What the row names is not identified.</value>
    [JsonPropertyName("OSI")]
    public int OrderSourceDataId { get; init; }

    /// <summary>Gets or sets the promotion reward attached to this truck order.</summary>
    /// <value>Saved as <c>PopPromoBox</c>; the promotion box attached to the order, or <see langword="null"/> (left out of the JSON) when there is none.</value>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }

    /// <summary>Gets or sets the global id of the customer who placed the order; 0 for an empty slot.</summary>
    /// <value>A global id in the truck order customer table.</value>
    public int Receiver { get; init; }

    /// <summary>Gets or sets the optional reviver avatar id.</summary>
    /// <value>The account id of the neighbour who filled this order for the owner, or <see langword="null"/> while nobody has.</value>
    [JsonPropertyName("ReviverAvatarId")]
    public EncodedDocumentValue? ReviverAvatarId { get; init; }

    /// <summary>Gets or sets the seasonal-currency bonus marker.</summary>
    /// <value>Saved as <c>seasonalCurrencyBonus</c>; set when the bonus reward is an event's seasonal currency, clear for other bonuses or none.</value>
    [JsonPropertyName("seasonalCurrencyBonus")]
    public ProtocolFlag SeasonalCurrencyBonus { get; init; }

    /// <summary>Gets or sets the countdown of an empty slot until its next order arrives; <see langword="null"/> when the slot has no countdown.</summary>
    /// <value>The remaining wait of the empty slot, saved as a timer.</value>
    public TimerSnapshot? Timer { get; init; }

    /// <summary>Gets or sets the voucher the order gives besides coins and experience; 0 when it gives none.</summary>
    /// <value>
    /// The voucher's type number from the voucher table's <c>VoucherType</c> column, not a row id.
    /// </value>
    public int Voucher { get; init; }
}
