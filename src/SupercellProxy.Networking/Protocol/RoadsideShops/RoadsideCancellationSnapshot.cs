using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Retains inventory-owned cancellation history and mussel opening progress.</summary>
public sealed record RoadsideCancellationSnapshot
{
    /// <summary>Countdown before the next cancellation returns to the first price tier.</summary>
    [JsonPropertyName("lastCancelRssSellCostCooldown")]
    public TimerSnapshot? CancellationCooldown { get; init; }
    /// <summary>Listings cancelled during the current cooldown window.</summary>
    [JsonPropertyName("cancelRssSellCount")]
    public int CancellationCount { get; init; }
    /// <summary>Gets the inventory's mussel opening count, capped after the two introductory openings.</summary>
    public int OpenedMusselCount { get; init; }
}
