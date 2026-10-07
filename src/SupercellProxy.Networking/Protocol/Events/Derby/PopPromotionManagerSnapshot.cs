using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Retains the promotion inventory and reward generation counters shared with the derby.</summary>
public sealed record PopPromotionManagerSnapshot
{
    /// <summary>Gets the saved promotion event identifier.</summary>
    public int Event { get; init; }
    /// <summary>Gets the promotion item quantities, indexed by their data-table rows.</summary>
    public int[] Goods { get; init; } = [];
    /// <summary>Gets the pending Hay Day reward boxes.</summary>
    public BoatPromotionRewardSnapshot[] HayDayBoxes { get; init; } = [];
    /// <summary>Gets whether promotion rewards are enabled for the avatar.</summary>
    public bool PopPromoEnabled { get; init; }
    /// <summary>Gets the promotion event timer.</summary>
    public TimerSnapshot? Timer { get; init; }
    /// <summary>Gets the number of generated boat promotion boxes.</summary>
    public int WeeklyBoatOrderPopBoxCount { get; init; }
    /// <summary>Gets the retained number of generated derby promotion boxes.</summary>
    public int WeeklyDerbyPopBoxCount { get; init; }
    /// <summary>Gets the number of Hay Day boxes collected in the current reset period.</summary>
    [JsonPropertyName("WeeklyHDBoxCount")]
    public int WeeklyHayDayBoxCount { get; init; }
    /// <summary>Gets the retained number of generated Valley promotion boxes.</summary>
    public int WeeklyMapGamePopBoxCount { get; init; }
    /// <summary>Gets the number of generated mystery-box promotion boxes.</summary>
    public int WeeklyMysteryBoxPopBoxCount { get; init; }
    /// <summary>Gets the number of Pop boxes collected in the current reset period.</summary>
    public int WeeklyPopBoxCount { get; init; }
    /// <summary>Gets the weekly reward counter reset timer.</summary>
    public TimerSnapshot? WeeklyResetTimer { get; init; }
    /// <summary>Gets the number of generated truck promotion boxes.</summary>
    public int WeeklyTruckOrderPopBoxCount { get; init; }
}
