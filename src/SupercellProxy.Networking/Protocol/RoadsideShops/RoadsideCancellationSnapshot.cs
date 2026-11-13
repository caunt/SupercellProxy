using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Retains inventory-owned cancellation history and mussel opening progress.</summary>
public sealed record RoadsideCancellationSnapshot
{
    /// <summary>Gets or sets the countdown before the next cancellation returns to the first price tier.</summary>
    /// <value>The timer saved under <c>lastCancelRssSellCostCooldown</c>; <see langword="null"/> when the save has no such timer.</value>
    [JsonPropertyName("lastCancelRssSellCostCooldown")]
    public TimerSnapshot? CancellationCooldown { get; init; }

    /// <summary>Gets or sets the listings cancelled during the current cooldown window.</summary>
    /// <value>The count saved under <c>cancelRssSellCount</c>; 0 when the key is missing.</value>
    [JsonPropertyName("cancelRssSellCount")]
    public int CancellationCount { get; init; }

    /// <summary>Gets or sets the inventory's mussel opening count.</summary>
    /// <value>The number of mussels opened so far, at most 2 in saves; 0 for a farm that has never opened one or a save without the field.</value>
    public int OpenedMusselCount { get; init; }
}
