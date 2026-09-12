using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
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

    /// Gets the event identifier associated with this order's bonus reward.
    [JsonPropertyName("BonusEventId")]
    public int BonusEventIdentifier { get; init; }

    /// Gets the optional bonus reward data-table identifier.
    public int? BonusReward { get; init; }

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

    /// Gets the helper identifier checksum value.
    [JsonPropertyName("HID")]
    public int? HelperIdentifier { get; init; }

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

    /// <summary>
    /// Gets or sets the <c language="csharp">Receiver</c> value.
    /// </summary>
    public int Receiver { get; init; }

    /// Gets the optional reviver avatar identifier.
    [JsonPropertyName("ReviverAvatarId")]
    public string? ReviverAvatarIdentifier { get; init; }

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
