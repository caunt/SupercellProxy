using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// A saved boat order: the crates the docked boat asks to be filled and the rewards it pays.
/// </summary>
public sealed record BoatOrderSnapshot
{
    /// <summary>
    /// Gets or sets the boat's crates in slot order; crate indexes in boat commands refer to positions in this list.
    /// </summary>
    /// <value>The crates saved as <c>crate_array</c>; empty by default.</value>
    [JsonPropertyName("crate_array")]
    public ImmutableArray<BoatCargoSnapshot> CargoEntries { get; init; } = [];

    /// <summary>Gets or sets the daily reward's data id.</summary>
    /// <value>The global id of the daily bonus reward, saved as <c>DailyRewardID</c>; 0 or less when the order pays no daily reward.</value>
    [JsonPropertyName("DailyRewardID")]
    public int DailyRewardGlobalId { get; init; }

    /// <summary>Gets or sets the daily reward amount.</summary>
    /// <value>How many of <see cref="DailyRewardGlobalId"/> the daily reward holds, saved as <c>DailyRewardQty</c>.</value>
    [JsonPropertyName("DailyRewardQty")]
    public int DailyRewardQuantity { get; init; }

    /// <summary>
    /// Gets or sets the player's level when the order was generated.
    /// </summary>
    /// <value>The experience level saved as <c>expLevel</c>.</value>
    [JsonPropertyName("expLevel")]
    public int ExperienceLevel { get; init; }

    /// <summary>Gets or sets the boat's monotonically increasing order number.</summary>
    /// <value>The order's sequence number.</value>
    public int OrderAnalyticsId { get; init; }

    /// <summary>Gets or sets the difficulty modifier applied to this order.</summary>
    /// <value>The global id of the order modifier row.</value>
    public int OrderModifier { get; init; }

    /// <summary>Gets or sets the retained local generator state for this order slot.</summary>
    /// <value>The saved seed of the order's own random generator.</value>
    public int RandomSeed { get; init; }

    /// <summary>Gets or sets the extra completion reward amount.</summary>
    /// <value>How many of <see cref="RewardGlobalId"/> the extra completion reward holds.</value>
    public int RewardCount { get; init; }

    /// <summary>Gets or sets the extra completion reward's data id.</summary>
    /// <value>The global id of the boat event reward, saved as <c>RewardID</c>; 0 or less when no boat reward event applies.</value>
    [JsonPropertyName("RewardID")]
    public int RewardGlobalId { get; init; }

    /// <summary>Gets or sets a value indicating whether the boat awards a sanctuary puzzle piece.</summary>
    /// <value><see langword="true"/> if completing the order grants one puzzle piece; otherwise, <see langword="false"/>.</value>
    public bool RewardPuzzlePiece { get; init; }

    /// <summary>Gets or sets the chosen destination definition.</summary>
    /// <value>The global id of the boat destination row the order sails to; it also names the order in the boat screen.</value>
    public int SelectedDestination { get; init; }

    /// <summary>Gets or sets the chosen difficulty definition.</summary>
    /// <value>The global id of the boat difficulty row; orders are listed by it, and 0 or less means no difficulty was selected.</value>
    public int SelectedDifficulty { get; init; }

    /// <summary>Gets or sets a value indicating whether the daily reward is shown during arrival.</summary>
    /// <value><see langword="true"/> if the boat displays its daily reward while sailing in; otherwise, <see langword="false"/>.</value>
    public bool ShowBonusRewardWhenBoatIsArriving { get; init; }

    /// <summary>Gets or sets a value indicating whether the daily reward is shown at the dock.</summary>
    /// <value><see langword="true"/> if the docked boat displays its daily reward; otherwise, <see langword="false"/>.</value>
    public bool ShowBonusRewardWhenBoatIsDocked { get; init; }

    /// <summary>Gets or sets the order's value before per-crate rounding.</summary>
    /// <value>The summed value saved as <c>total_value</c>.</value>
    [JsonPropertyName("total_value")]
    public int TotalValue { get; init; }

    /// <summary>Gets or sets the completion points credited to the boat reward track.</summary>
    /// <value>The nautical miles the order adds to the boat track when sent, saved as <c>trackProgressValue</c>.</value>
    [JsonPropertyName("trackProgressValue")]
    public int TrackProgressValue { get; init; }

    /// <summary>Gets or sets the separately selected voucher reward.</summary>
    /// <value>The global id of one voucher granted on completion, saved as <c>VoucherRewardGlobalID</c>; 0 when the order pays no voucher.</value>
    [JsonPropertyName("VoucherRewardGlobalID")]
    public int VoucherRewardGlobalId { get; init; }
}
