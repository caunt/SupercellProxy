using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Inventory-owned roadside cancellation price history.</summary>
public sealed record RoadsideCancellationSnapshot
{

    /// <summary>Countdown before the next cancellation returns to the first price tier.</summary>
    [JsonPropertyName("lastCancelRssSellCostCooldown")]
    public TimerSnapshot? CancellationCooldown { get; init; }
    /// <summary>Listings cancelled during the current cooldown window.</summary>
    [JsonPropertyName("cancelRssSellCount")]
    public int CancellationCount { get; init; }
}
