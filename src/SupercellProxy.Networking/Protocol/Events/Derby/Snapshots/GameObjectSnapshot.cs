using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events.Derby.Rewards;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets or sets the saved bingo theme reward-set identifiers.</summary>
    /// <value>The identifiers of the reward sets the bingo-line rewards are drawn from, in bingo-line order, or <see langword="null"/> when the object saves no bingo theme.</value>
    public int[]? BingoThemeRewardSets { get; init; }
    /// <summary>Gets or sets the amounts for the bingo reward choices.</summary>
    /// <value>The quantity of each entry in <see cref="BingoThresholdRewards"/>, matched by position, or <see langword="null"/> when no bingo choices are saved.</value>
    public int[]? BingoThresholdRewardAmounts { get; init; }
    /// <summary>Gets or sets the high word of the derby owning the bingo reward sets.</summary>
    /// <value>The upper 32 bits of the identifier of the derby that <see cref="BingoThemeRewardSets"/> was generated for, or <see langword="null"/> when no owner is saved.</value>
    public int? BingoThresholdRewardSetsHigh { get; init; }
    /// <summary>Gets or sets the low word of the derby owning the bingo reward sets.</summary>
    /// <value>The lower 32 bits of that same derby identifier, paired with <see cref="BingoThresholdRewardSetsHigh"/>, or <see langword="null"/> when no owner is saved.</value>
    public int? BingoThresholdRewardSetsLow { get; init; }
    /// <summary>Gets or sets the saved bingo reward choices.</summary>
    /// <value>The data identifiers of the rewards offered for the bingo thresholds, in threshold order, or <see langword="null"/> when none were generated.</value>
    public int[]? BingoThresholdRewards { get; init; }
    /// <summary>Gets or sets the high word of the derby owning the bingo choices.</summary>
    /// <value>The upper 32 bits of the identifier of the derby that <see cref="BingoThresholdRewards"/> belongs to, or <see langword="null"/> when no owner is saved.</value>
    public int? BingoThresholdRewardsHigh { get; init; }
    /// <summary>Gets or sets the low word of the derby owning the bingo choices.</summary>
    /// <value>The lower 32 bits of the derby identifier paired with <see cref="BingoThresholdRewardsHigh"/>, or <see langword="null"/> when no owner is saved.</value>
    public int? BingoThresholdRewardsLow { get; init; }
    /// <summary>Gets or sets the extra podium quantity percentage for boosted rewards.</summary>
    /// <value>The saved quantity boost applied to boosted podium rewards; it defaults to 1 when the key is missing.</value>
    public int BoostedTopMultiplier { get; init; } = 1;
    /// <summary>Gets or sets the reward names receiving the derby's saved quantity and weight boosts.</summary>
    /// <value>The asset row names of the boosted reward items, or <see langword="null"/> when the derby boosts none.</value>
    public string[]? BoostedRewards { get; init; }
    /// <summary>Gets or sets the extra selection weight percentage for boosted rewards.</summary>
    /// <value>The saved extra spawn percentage for the <see cref="BoostedRewards"/> items, or <see langword="null"/> when the save has no spawn boost.</value>
    public int? BoostedSpawn { get; init; }
    /// <summary>Gets or sets the extra threshold quantity percentage for boosted rewards.</summary>
    /// <value>The saved quantity boost applied to boosted horseshoe threshold rewards; it defaults to 1 when the key is missing.</value>
    public int BoostedMultiplier { get; init; } = 1;
    /// <summary>Gets or sets the saved bunny theme reward-set identifiers.</summary>
    /// <value>The identifiers of the reward sets the bunny rewards are drawn from, in bunny-reward order, or <see langword="null"/> when the object saves no bunny theme.</value>
    public int[]? BunnyThemeRewardSets { get; init; }
    /// <summary>Gets or sets the amounts for the bunny reward choices.</summary>
    /// <value>The quantity of each entry in <see cref="BunnyThresholdRewards"/>, matched by position, or <see langword="null"/> when no bunny choices are saved.</value>
    public int[]? BunnyThresholdRewardAmounts { get; init; }
    /// <summary>Gets or sets the high word of the derby owning the bunny reward sets.</summary>
    /// <value>The upper 32 bits of the identifier of the derby that <see cref="BunnyThemeRewardSets"/> was generated for, or <see langword="null"/> when no owner is saved.</value>
    public int? BunnyThresholdRewardSetsHigh { get; init; }
    /// <summary>Gets or sets the low word of the derby owning the bunny reward sets.</summary>
    /// <value>The lower 32 bits of the derby identifier paired with <see cref="BunnyThresholdRewardSetsHigh"/>, or <see langword="null"/> when no owner is saved.</value>
    public int? BunnyThresholdRewardSetsLow { get; init; }
    /// <summary>Gets or sets the saved bunny reward choices.</summary>
    /// <value>The data identifiers of the rewards offered for the bunny thresholds, in threshold order, or <see langword="null"/> when none were generated.</value>
    public int[]? BunnyThresholdRewards { get; init; }
    /// <summary>Gets or sets the high word of the derby owning the bunny choices.</summary>
    /// <value>The upper 32 bits of the identifier of the derby that <see cref="BunnyThresholdRewards"/> belongs to, or <see langword="null"/> when no owner is saved.</value>
    public int? BunnyThresholdRewardsHigh { get; init; }
    /// <summary>Gets or sets the low word of the derby owning the bunny choices.</summary>
    /// <value>The lower 32 bits of the derby identifier paired with <see cref="BunnyThresholdRewardsHigh"/>, or <see langword="null"/> when no owner is saved.</value>
    public int? BunnyThresholdRewardsLow { get; init; }
    /// <summary>Gets or sets the saved maximum number of booster choices per threshold.</summary>
    /// <value>How many booster rewards one horseshoe threshold may offer, saved under <c>NumThresholdBoosters</c>; it defaults to 1 when the key is missing.</value>
    [JsonPropertyName("NumThresholdBoosters")]
    public int MaximumThresholdBoosters { get; init; } = 1;
    /// <summary>Gets or sets the cached participation requirement for the current derby.</summary>
    /// <value>The minimum neighborhood points a member must earn in the running derby to qualify, saved under <c>currentMinRequiredPoints</c>; -1 when not cached yet.</value>
    [JsonPropertyName("currentMinRequiredPoints")]
    public int CurrentDerbyMinimumPoints { get; init; } = -1;
    /// <summary>Gets or sets the cached current derby identifier as a signed native long.</summary>
    /// <value>The running derby's identifier as one 64-bit number, saved under <c>currentKnownDerbyId</c>, or <see langword="null"/> when no current derby is known.</value>
    [JsonPropertyName("currentKnownDerbyId")]
    public long? CurrentKnownDerbyId { get; init; }
    /// <summary>Gets or sets the leaderboard entry values already presented.</summary>
    /// <value>One saved state per leaderboard entry with the score, order and bunny score last shown, or <see langword="null"/> when nothing has been presented.</value>
    public DerbyEntryState[]? EntryStates { get; init; }
    /// <summary>Gets or sets the amounts for the legacy podium rewards.</summary>
    /// <value>The quantity of each entry in <see cref="LeagueRewards"/>, matched by position, or <see langword="null"/> when the save uses <see cref="LeagueRewardsByRank"/> instead.</value>
    public int[]? LeagueRewardAmounts { get; init; }
    /// <summary>Gets or sets the legacy three podium reward identifiers.</summary>
    /// <value>The data identifiers of the first-, second- and third-place rewards in older saves, or <see langword="null"/> when they are absent.</value>
    public int[]? LeagueRewards { get; init; }
    /// <summary>Gets or sets generated podium rewards indexed by placement, one through three.</summary>
    /// <value>The reward list for each podium place, keyed by placement 1, 2 and 3, or <see langword="null"/> when no podium rewards were generated.</value>
    public Dictionary<int, DerbyRewardPoolSnapshot>? LeagueRewardsByRank { get; init; }
    /// <summary>Gets or sets the high word of the derby owning the podium rewards.</summary>
    /// <value>The upper 32 bits of the identifier of the derby the podium rewards were generated for, or <see langword="null"/> when no owner is saved.</value>
    public int? LeagueRewardsHigh { get; init; }
    /// <summary>Gets or sets the low word of the derby owning the podium rewards.</summary>
    /// <value>The lower 32 bits of the derby identifier paired with <see cref="LeagueRewardsHigh"/>, or <see langword="null"/> when no owner is saved.</value>
    public int? LeagueRewardsLow { get; init; }
    /// <summary>Gets or sets the cached number of thresholds for the current derby.</summary>
    /// <value>How many horseshoe thresholds the running derby has, saved under <c>currentThresholdCount</c>; -1 when not cached yet.</value>
    [JsonPropertyName("currentThresholdCount")]
    public int CurrentDerbyThresholdCount { get; init; } = -1;
    /// <summary>Gets or sets promotion rewards attached to ordinary derby choices.</summary>
    /// <value>The Pop boxes tied to ordinary horseshoe choices, each naming its flattened choice index, or <see langword="null"/> when no promotion boxes are attached.</value>
    public DerbyPromotionRewardSnapshot[]? PopBoxThresholdRewards { get; init; }
    /// <summary>Gets or sets the cached participation requirement for the previous derby.</summary>
    /// <value>The minimum points a member needed in the last finished derby, saved under <c>lastMinRequiredPoints</c>; -1 when not cached.</value>
    [JsonPropertyName("lastMinRequiredPoints")]
    public int PreviousDerbyMinimumPoints { get; init; } = -1;
    /// <summary>Gets or sets the cached number of thresholds for the previous derby.</summary>
    /// <value>How many horseshoe thresholds the last finished derby had, saved under <c>lastThresholdCount</c>; -1 when not cached.</value>
    [JsonPropertyName("lastThresholdCount")]
    public int PreviousDerbyThresholdCount { get; init; } = -1;
    /// <summary>Gets or sets the league threshold counts retained for the previous derby.</summary>
    /// <value>The threshold count of each league in the last finished derby, in league order, saved under <c>lastThresholdsCountPerLeague</c>, or <see langword="null"/> when not retained.</value>
    [JsonPropertyName("lastThresholdsCountPerLeague")]
    public int[]? PreviousDerbyThresholdCountsPerLeague { get; init; }
    /// <summary>Gets or sets the point thresholds retained for the previous derby.</summary>
    /// <value>The neighborhood points needed for each horseshoe of the last finished derby, in threshold order, saved under <c>lastThresholdPoints</c>, or <see langword="null"/> when not retained.</value>
    [JsonPropertyName("lastThresholdPoints")]
    public int[]? PreviousDerbyThresholdPoints { get; init; }
    /// <summary>Gets or sets the cached previous derby identifier as a signed native long.</summary>
    /// <value>The last finished derby's identifier as one 64-bit number, saved under <c>lastKnownDerbyId</c>, or <see langword="null"/> when none is known.</value>
    [JsonPropertyName("lastKnownDerbyId")]
    public long? PreviousKnownDerbyId { get; init; }
    /// <summary>Gets or sets the saved candidate pools indexed by zero-based threshold.</summary>
    /// <value>The weighted reward pool each horseshoe threshold draws its choices from, keyed by threshold index starting at 0, or <see langword="null"/> when no pools are saved.</value>
    public Dictionary<int, DerbyRewardPoolSnapshot>? RewardsByThresholdIdx { get; init; }
    /// <summary>Gets or sets the number of bingo reward thresholds already shown.</summary>
    /// <value>How many bingo thresholds the player has already been shown in the derby named by <see cref="SeenBingoHigh"/> and <see cref="SeenBingoLow"/>; 0 by default.</value>
    public int SeenBingoCount { get; init; }
    /// <summary>Gets or sets the high part of the derby associated with the seen bingo rewards.</summary>
    /// <value>The upper 32 bits of the derby identifier that <see cref="SeenBingoCount"/> counts for, or <see langword="null"/> when not saved.</value>
    public int? SeenBingoHigh { get; init; }
    /// <summary>Gets or sets the low part of the derby associated with the seen bingo rewards.</summary>
    /// <value>The lower 32 bits of the derby identifier paired with <see cref="SeenBingoHigh"/>, or <see langword="null"/> when not saved.</value>
    public int? SeenBingoLow { get; init; }
    /// <summary>Gets or sets the number of bunny reward thresholds already shown.</summary>
    /// <value>How many bunny thresholds the player has already been shown in the derby named by <see cref="SeenBunnyHigh"/> and <see cref="SeenBunnyLow"/>; 0 by default.</value>
    public int SeenBunnyCount { get; init; }
    /// <summary>Gets or sets the high part of the derby associated with the seen bunny rewards.</summary>
    /// <value>The upper 32 bits of the derby identifier that <see cref="SeenBunnyCount"/> counts for, or <see langword="null"/> when not saved.</value>
    public int? SeenBunnyHigh { get; init; }
    /// <summary>Gets or sets the low part of the derby associated with the seen bunny rewards.</summary>
    /// <value>The lower 32 bits of the derby identifier paired with <see cref="SeenBunnyHigh"/>, or <see langword="null"/> when not saved.</value>
    public int? SeenBunnyLow { get; init; }
    /// <summary>Gets or sets the number of derby bunny appearances already shown to the player.</summary>
    /// <value>How many of the derby's flying-bunny appearances have already played for this player; 0 by default.</value>
    public int SeenFlyingBunnyCount { get; init; }
    /// <summary>Gets or sets the number of ordinary horseshoe thresholds already shown.</summary>
    /// <value>How many ordinary horseshoe thresholds the player has already been shown in the derby named by <see cref="SeenKeyHigh"/> and <see cref="SeenKeyLow"/>; 0 by default.</value>
    public int SeenKeyCount { get; init; }
    /// <summary>Gets or sets the high part of the derby associated with the seen horseshoes.</summary>
    /// <value>The upper 32 bits of the derby identifier that <see cref="SeenKeyCount"/> counts for, or <see langword="null"/> when not saved.</value>
    public int? SeenKeyHigh { get; init; }
    /// <summary>Gets or sets the low part of the derby associated with the seen horseshoes.</summary>
    /// <value>The lower 32 bits of the derby identifier paired with <see cref="SeenKeyHigh"/>, or <see langword="null"/> when not saved.</value>
    public int? SeenKeyLow { get; init; }
    /// <summary>Gets or sets whether the league-change presentation is disabled for the derby.</summary>
    /// <value><see langword="true"/> if the league promotion or demotion screen is not shown for this derby; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    public bool SkipLeaguePromotionDemotion { get; init; }
    /// <summary>Gets or sets whether the derby start presentation was acknowledged.</summary>
    /// <value><see langword="true"/> if the player has already seen the derby start intro; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    public bool StartIntroSeen { get; init; }
    /// <summary>Gets or sets the saved number of ordinary reward thresholds.</summary>
    /// <value>How many horseshoe thresholds the saved reward choices cover; 0 when the key is missing.</value>
    public int ThresholdCount { get; init; }
    /// <summary>Gets or sets the saved threshold count for each of the five leagues.</summary>
    /// <value>Five counts in league order, one horseshoe threshold count per league, or <see langword="null"/> when not saved.</value>
    public int[]? ThresholdCountPerLeague { get; init; }
    /// <summary>Gets or sets the neighborhood points required for each saved horseshoe.</summary>
    /// <value>The point total that unlocks each horseshoe, in threshold order, or <see langword="null"/> when not saved.</value>
    public int[]? ThresholdPoints { get; init; }
    /// <summary>Gets or sets the amounts for the ordinary horseshoe choices.</summary>
    /// <value>The quantity of each entry in <see cref="ThresholdRewards"/>, matched by position, or <see langword="null"/> when no choices are saved.</value>
    public int[]? ThresholdRewardAmounts { get; init; }
    /// <summary>Gets or sets the saved ordinary horseshoe choices in threshold and slot order.</summary>
    /// <value>The data identifiers of every choice, listed threshold by threshold with each threshold's slots together; its positions are the flattened indices that <see cref="DerbyPromotionRewardSnapshot.PopBoxThresholdIndex"/> refers to. <see langword="null"/> when none were generated.</value>
    public int[]? ThresholdRewards { get; init; }
    /// <summary>Gets or sets the high word of the derby owning the horseshoe choices.</summary>
    /// <value>The upper 32 bits of the identifier of the derby that <see cref="ThresholdRewards"/> belongs to, or <see langword="null"/> when no owner is saved.</value>
    public int? ThresholdRewardsHigh { get; init; }
    /// <summary>Gets or sets the low word of the derby owning the horseshoe choices.</summary>
    /// <value>The lower 32 bits of the derby identifier paired with <see cref="ThresholdRewardsHigh"/>, or <see langword="null"/> when no owner is saved.</value>
    public int? ThresholdRewardsLow { get; init; }
}
