using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events.Derby.Rewards;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets the saved bingo theme reward-set identifiers.</summary>
    public int[]? BingoThemeRewardSets { get; init; }
    /// <summary>Gets the amounts for the bingo reward choices.</summary>
    public int[]? BingoThresholdRewardAmounts { get; init; }
    /// <summary>Gets the high word of the derby owning the bingo reward sets.</summary>
    public int? BingoThresholdRewardSetsHigh { get; init; }
    /// <summary>Gets the low word of the derby owning the bingo reward sets.</summary>
    public int? BingoThresholdRewardSetsLow { get; init; }
    /// <summary>Gets the saved bingo reward choices.</summary>
    public int[]? BingoThresholdRewards { get; init; }
    /// <summary>Gets the high word of the derby owning the bingo choices.</summary>
    public int? BingoThresholdRewardsHigh { get; init; }
    /// <summary>Gets the low word of the derby owning the bingo choices.</summary>
    public int? BingoThresholdRewardsLow { get; init; }
    /// <summary>Gets the extra podium quantity percentage for boosted rewards.</summary>
    public int BoostedTopMultiplier { get; init; } = 1;
    /// <summary>Gets the reward names receiving the derby's saved quantity and weight boosts.</summary>
    public string[]? BoostedRewards { get; init; }
    /// <summary>Gets the extra selection weight percentage for boosted rewards.</summary>
    public int? BoostedSpawn { get; init; }
    /// <summary>Gets the extra threshold quantity percentage for boosted rewards.</summary>
    public int BoostedMultiplier { get; init; } = 1;
    /// <summary>Gets the saved bunny theme reward-set identifiers.</summary>
    public int[]? BunnyThemeRewardSets { get; init; }
    /// <summary>Gets the amounts for the bunny reward choices.</summary>
    public int[]? BunnyThresholdRewardAmounts { get; init; }
    /// <summary>Gets the high word of the derby owning the bunny reward sets.</summary>
    public int? BunnyThresholdRewardSetsHigh { get; init; }
    /// <summary>Gets the low word of the derby owning the bunny reward sets.</summary>
    public int? BunnyThresholdRewardSetsLow { get; init; }
    /// <summary>Gets the saved bunny reward choices.</summary>
    public int[]? BunnyThresholdRewards { get; init; }
    /// <summary>Gets the high word of the derby owning the bunny choices.</summary>
    public int? BunnyThresholdRewardsHigh { get; init; }
    /// <summary>Gets the low word of the derby owning the bunny choices.</summary>
    public int? BunnyThresholdRewardsLow { get; init; }
    /// <summary>Gets the saved maximum number of booster choices per threshold.</summary>
    [JsonPropertyName("NumThresholdBoosters")]
    public int MaximumThresholdBoosters { get; init; } = 1;
    /// <summary>Gets the cached participation requirement for the current derby.</summary>
    [JsonPropertyName("currentMinRequiredPoints")]
    public int CurrentDerbyMinimumPoints { get; init; } = -1;
    /// <summary>Gets the cached current derby identifier as a signed native long.</summary>
    [JsonPropertyName("currentKnownDerbyId")]
    public long? CurrentKnownDerbyId { get; init; }
    /// <summary>Gets the leaderboard entry values already presented.</summary>
    public DerbyEntryState[]? EntryStates { get; init; }
    /// <summary>Gets the amounts for the legacy podium rewards.</summary>
    public int[]? LeagueRewardAmounts { get; init; }
    /// <summary>Gets the legacy three podium reward identifiers.</summary>
    public int[]? LeagueRewards { get; init; }
    /// <summary>Gets generated podium rewards indexed by placement, one through three.</summary>
    public Dictionary<int, DerbyRewardPoolSnapshot>? LeagueRewardsByRank { get; init; }
    /// <summary>Gets the high word of the derby owning the podium rewards.</summary>
    public int? LeagueRewardsHigh { get; init; }
    /// <summary>Gets the low word of the derby owning the podium rewards.</summary>
    public int? LeagueRewardsLow { get; init; }
    /// <summary>Gets the cached number of thresholds for the current derby.</summary>
    [JsonPropertyName("currentThresholdCount")]
    public int CurrentDerbyThresholdCount { get; init; } = -1;
    /// <summary>Gets promotion rewards attached to ordinary derby choices.</summary>
    public DerbyPromotionRewardSnapshot[]? PopBoxThresholdRewards { get; init; }
    /// <summary>Gets the cached participation requirement for the previous derby.</summary>
    [JsonPropertyName("lastMinRequiredPoints")]
    public int PreviousDerbyMinimumPoints { get; init; } = -1;
    /// <summary>Gets the cached number of thresholds for the previous derby.</summary>
    [JsonPropertyName("lastThresholdCount")]
    public int PreviousDerbyThresholdCount { get; init; } = -1;
    /// <summary>Gets the league threshold counts retained for the previous derby.</summary>
    [JsonPropertyName("lastThresholdsCountPerLeague")]
    public int[]? PreviousDerbyThresholdCountsPerLeague { get; init; }
    /// <summary>Gets the point thresholds retained for the previous derby.</summary>
    [JsonPropertyName("lastThresholdPoints")]
    public int[]? PreviousDerbyThresholdPoints { get; init; }
    /// <summary>Gets the cached previous derby identifier as a signed native long.</summary>
    [JsonPropertyName("lastKnownDerbyId")]
    public long? PreviousKnownDerbyId { get; init; }
    /// <summary>Gets the saved candidate pools indexed by zero-based threshold.</summary>
    public Dictionary<int, DerbyRewardPoolSnapshot>? RewardsByThresholdIdx { get; init; }
    /// <summary>Gets the number of bingo reward thresholds already shown.</summary>
    public int SeenBingoCount { get; init; }
    /// <summary>Gets the high part of the derby associated with the seen bingo rewards.</summary>
    public int? SeenBingoHigh { get; init; }
    /// <summary>Gets the low part of the derby associated with the seen bingo rewards.</summary>
    public int? SeenBingoLow { get; init; }
    /// <summary>Gets the number of bunny reward thresholds already shown.</summary>
    public int SeenBunnyCount { get; init; }
    /// <summary>Gets the high part of the derby associated with the seen bunny rewards.</summary>
    public int? SeenBunnyHigh { get; init; }
    /// <summary>Gets the low part of the derby associated with the seen bunny rewards.</summary>
    public int? SeenBunnyLow { get; init; }
    /// <summary>Gets the number of derby bunny appearances already shown to the player.</summary>
    public int SeenFlyingBunnyCount { get; init; }
    /// <summary>Gets the number of ordinary horseshoe thresholds already shown.</summary>
    public int SeenKeyCount { get; init; }
    /// <summary>Gets the high part of the derby associated with the seen horseshoes.</summary>
    public int? SeenKeyHigh { get; init; }
    /// <summary>Gets the low part of the derby associated with the seen horseshoes.</summary>
    public int? SeenKeyLow { get; init; }
    /// <summary>Gets whether the league-change presentation is disabled for the derby.</summary>
    public bool SkipLeaguePromotionDemotion { get; init; }
    /// <summary>Gets whether the derby start presentation was acknowledged.</summary>
    public bool StartIntroSeen { get; init; }
    /// <summary>Gets the saved number of ordinary reward thresholds.</summary>
    public int ThresholdCount { get; init; }
    /// <summary>Gets the saved threshold count for each of the five leagues.</summary>
    public int[]? ThresholdCountPerLeague { get; init; }
    /// <summary>Gets the neighborhood points required for each saved horseshoe.</summary>
    public int[]? ThresholdPoints { get; init; }
    /// <summary>Gets the amounts for the ordinary horseshoe choices.</summary>
    public int[]? ThresholdRewardAmounts { get; init; }
    /// <summary>Gets the saved ordinary horseshoe choices in threshold and slot order.</summary>
    public int[]? ThresholdRewards { get; init; }
    /// <summary>Gets the high word of the derby owning the horseshoe choices.</summary>
    public int? ThresholdRewardsHigh { get; init; }
    /// <summary>Gets the low word of the derby owning the horseshoe choices.</summary>
    public int? ThresholdRewardsLow { get; init; }
}
