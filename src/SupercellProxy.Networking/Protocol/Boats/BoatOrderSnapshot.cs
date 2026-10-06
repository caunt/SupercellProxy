using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Boat Order Snapshot contract.
/// </summary>
public sealed record BoatOrderSnapshot
{
    /// <summary>
    /// Gets the Cargo Entries value.
    /// </summary>
    [JsonPropertyName("crate_array")]
    public BoatCargoSnapshot[] CargoEntries { get; init; } = [];

    /// <summary>Gets the daily reward's data id.</summary>
    [JsonPropertyName("DailyRewardID")]
    public int DailyRewardGlobalId { get; init; }

    /// <summary>Gets the daily reward amount.</summary>
    [JsonPropertyName("DailyRewardQty")]
    public int DailyRewardQuantity { get; init; }

    /// <summary>
    /// Gets the Experience Level value.
    /// </summary>
    [JsonPropertyName("expLevel")]
    public int ExperienceLevel { get; init; }

    /// <summary>Gets the boat's monotonically increasing order number.</summary>
    public int OrderAnalyticsId { get; init; }

    /// <summary>Gets the difficulty modifier applied to this order.</summary>
    public int OrderModifier { get; init; }

    /// <summary>Gets the retained local generator state for this order slot.</summary>
    public int RandomSeed { get; init; }

    /// <summary>Gets the extra completion reward amount.</summary>
    public int RewardCount { get; init; }

    /// <summary>Gets the extra completion reward's data id.</summary>
    [JsonPropertyName("RewardID")]
    public int RewardGlobalId { get; init; }

    /// <summary>Gets whether the boat awards a sanctuary puzzle piece.</summary>
    public bool RewardPuzzlePiece { get; init; }

    /// <summary>Gets the chosen destination definition.</summary>
    public int SelectedDestination { get; init; }

    /// <summary>Gets the chosen difficulty definition.</summary>
    public int SelectedDifficulty { get; init; }

    /// <summary>Gets whether the daily reward is shown during arrival.</summary>
    public bool ShowBonusRewardWhenBoatIsArriving { get; init; }

    /// <summary>Gets whether the daily reward is shown at the dock.</summary>
    public bool ShowBonusRewardWhenBoatIsDocked { get; init; }

    /// <summary>Gets the order's value before per-crate rounding.</summary>
    [JsonPropertyName("total_value")]
    public int TotalValue { get; init; }
    /// <summary>Gets the completion points credited to the boat reward track.</summary>
    [JsonPropertyName("trackProgressValue")]
    public int TrackProgressValue { get; init; }
    /// <summary>Gets the separately selected voucher reward.</summary>
    [JsonPropertyName("VoucherRewardGlobalID")]
    public int VoucherRewardGlobalId { get; init; }
}
