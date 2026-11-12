using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Retains the promotion inventory and reward generation counters shared with the derby.</summary>
public sealed record PopPromotionManagerSnapshot
{
    /// <summary>Gets or sets the saved promotion event identifier.</summary>
    /// <value>The identifier of the promotion event the saved counters belong to; 0 when the save omits it.</value>
    public int Event { get; init; }
    /// <summary>Gets or sets the promotion item quantities, indexed by their data-table rows.</summary>
    /// <value>One quantity per promotion item row, where position <c>i</c> holds the amount of row <c>i</c>; an empty array when the save lists none.</value>
    public int[] Goods { get; init; } = [];
    /// <summary>Gets or sets the pending Hay Day reward boxes.</summary>
    /// <value>The Hay Day boxes generated for the promotion, each with its reward and collected marker; an empty array when none are waiting.</value>
    public BoatPromotionRewardSnapshot[] HayDayBoxes { get; init; } = [];
    /// <summary>Gets or sets whether promotion rewards are enabled for the avatar.</summary>
    /// <value><see langword="true"/> if the avatar takes part in the promotion; otherwise, <see langword="false"/>.</value>
    /// <remarks>A save that omits the key reads as <see langword="false"/>.</remarks>
    public bool PopPromoEnabled { get; init; }
    /// <summary>Gets or sets the promotion event timer.</summary>
    /// <value>The start time and remaining ticks of the running promotion, or <see langword="null"/> when no promotion timer is saved.</value>
    public TimerSnapshot? Timer { get; init; }
    /// <summary>Gets or sets the number of generated boat promotion boxes.</summary>
    /// <value>How many promotion boxes boat orders have produced in the current weekly period.</value>
    public int WeeklyBoatOrderPopBoxCount { get; init; }
    /// <summary>Gets or sets the retained number of generated derby promotion boxes.</summary>
    /// <value>How many promotion boxes derby tasks have produced in the current weekly period.</value>
    public int WeeklyDerbyPopBoxCount { get; init; }
    /// <summary>Gets or sets the number of Hay Day boxes collected in the current reset period.</summary>
    /// <value>The count saved under the <c>WeeklyHDBoxCount</c> key.</value>
    [JsonPropertyName("WeeklyHDBoxCount")]
    public int WeeklyHayDayBoxCount { get; init; }
    /// <summary>Gets or sets the retained number of generated Valley promotion boxes.</summary>
    /// <value>How many promotion boxes the Valley has produced in the current weekly period.</value>
    public int WeeklyMapGamePopBoxCount { get; init; }
    /// <summary>Gets or sets the number of generated mystery-box promotion boxes.</summary>
    /// <value>How many promotion boxes mystery boxes have produced in the current weekly period.</value>
    public int WeeklyMysteryBoxPopBoxCount { get; init; }
    /// <summary>Gets or sets the number of Pop boxes collected in the current reset period.</summary>
    /// <value>How many Pop boxes the avatar has collected since the last weekly reset; the per-source counters such as <see cref="WeeklyDerbyPopBoxCount"/> count generated boxes instead.</value>
    public int WeeklyPopBoxCount { get; init; }
    /// <summary>Gets or sets the weekly reward counter reset timer.</summary>
    /// <value>The countdown to the next weekly reset, or <see langword="null"/> when the save has no reset timer.</value>
    public TimerSnapshot? WeeklyResetTimer { get; init; }
    /// <summary>Gets or sets the number of generated truck promotion boxes.</summary>
    /// <value>How many promotion boxes truck orders have produced in the current weekly period.</value>
    public int WeeklyTruckOrderPopBoxCount { get; init; }
}
