using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>
/// A live event that overrides one movie ticket placement's settings while it runs: daily video limit, play-limit windows and
/// reward sets. Players who recently spent money can get separate limits and rewards.
/// </summary>
public sealed record MovieTicketEventDefinition
{
    /// <summary>The shop-event type carrying movie-ticket placement overrides.</summary>
    public const int EventType = 47;

    /// <summary>Gets or sets the name of the movie ticket placement the event overrides, matching the placement's table entry name.</summary>
    /// <value>A movie ticket table row name read from <c>AdPlacement</c>, or <see langword="null"/> when absent.</value>
    [JsonPropertyName("AdPlacement")]
    public string? AdPlacement { get; init; }

    /// <summary>Gets or sets how many videos a player may watch per day at this placement.</summary>
    /// <value>The daily video limit, read from <c>DailyAds</c>; 0 or less means none.</value>
    [JsonPropertyName("DailyAds")]
    public int DailyAds { get; init; }

    /// <summary>Gets or sets the rewards used as the first reward (when the game configuration enables that) for players who are not recent spenders.</summary>
    /// <value>Saved as <c>RewardSetForcedNonSpender</c>; empty by default.</value>
    [JsonPropertyName("RewardSetForcedNonSpender")]
    public ImmutableArray<MovieTicketRewardDefinition> ForcedNonSpenderRewards { get; init; } = [];

    /// <summary>Gets or sets the same first-reward set for recent spenders.</summary>
    /// <value>Saved as <c>RewardSetForcedSpender</c>; used for spenders unless <see cref="UseSameRewardSetForSpenders"/> is set; empty by default.</value>
    [JsonPropertyName("RewardSetForcedSpender")]
    public ImmutableArray<MovieTicketRewardDefinition> ForcedSpenderRewards { get; init; } = [];

    /// <summary>Gets or sets the play-limit windows: how many videos fit in each window and how long each window lasts.</summary>
    /// <value>One entry per window, in the order the placement saves its window counters and timers.</value>
    [JsonPropertyName("MoviesCycle")]
    public ImmutableArray<MovieTicketCycleDefinition> MoviesCycle { get; init; } = [];

    /// <summary>Gets or sets the legacy spender-cycle array.</summary>
    /// <value>Saved as <c>MoviesCycleSpender</c>; carried through unchanged so the event round-trips.</value>
    [JsonPropertyName("MoviesCycleSpender")]
    public ImmutableArray<MovieTicketCycleDefinition> SpenderMoviesCycle { get; init; } = [];

    /// <summary>Gets or sets the weighted reward pool the following rewards are drawn from, for players who are not recent spenders.</summary>
    /// <value>Saved as <c>RewardRandomSetNonSpender</c>; each entry carries its weight. Empty by default.</value>
    [JsonPropertyName("RewardRandomSetNonSpender")]
    public ImmutableArray<MovieTicketRewardDefinition> RandomNonSpenderRewards { get; init; } = [];

    /// <summary>Gets or sets the placement level override used by movie-ticket availability.</summary>
    /// <value>The placement's required experience level while the event runs, in place of the table's required level.</value>
    [JsonPropertyName("RequiredLevel")]
    public int RequiredLevel { get; init; }

    /// <summary>Gets or sets the level limits a player must meet for the event to apply.</summary>
    /// <value>Saved as <c>requirements</c>; holds the minimum experience level, or is <see langword="null"/> when the event sets none.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets or sets the seasonal currency the event's rewards can pay out in; <see langword="null"/> when it has none.</summary>
    /// <value>Names the seasonal currency table entry that rewards of type 4 refer to.</value>
    [JsonPropertyName("SeasonalCurrency")]
    public EventSeasonalCurrencyDefinition? SeasonalCurrency { get; init; }

    /// <summary>Gets or sets the daily video limit for recent spenders, used unless <see cref="UseSameCycleForSpender"/> is set.</summary>
    /// <value>Saved as <c>DailyAdsSpender</c>; replaces <see cref="DailyAds"/> for spenders, and 0 or less means none.</value>
    [JsonPropertyName("DailyAdsSpender")]
    public int SpenderDailyAds { get; init; }

    /// <summary>Gets or sets the weighted reward pool for recent spenders, used unless <see cref="UseSameRewardSetForSpenders"/> is set.</summary>
    /// <value>Saved as <c>RewardRandomSetSpender</c>; each entry carries its weight. Empty by default.</value>
    [JsonPropertyName("RewardRandomSetSpender")]
    public ImmutableArray<MovieTicketRewardDefinition> RandomSpenderRewards { get; init; } = [];

    /// <summary>Gets or sets whether recent spenders get the same daily video limit as everyone else.</summary>
    /// <value><see langword="true"/> if <see cref="DailyAds"/> applies to spenders too; otherwise, <see langword="false"/>.</value>
    /// <remarks>When the flag is <see langword="false"/>, spenders use <see cref="SpenderDailyAds"/>.</remarks>
    [JsonPropertyName("useSameCycleForSpender")]
    public bool UseSameCycleForSpender { get; init; }

    /// <summary>Gets or sets whether recent spenders get the same reward sets as everyone else.</summary>
    /// <value><see langword="true"/> if spenders draw from the non-spender forced and random sets; otherwise, <see langword="false"/>.</value>
    /// <remarks>When the flag is <see langword="false"/>, spenders use the spender sets.</remarks>
    [JsonPropertyName("useSameRewardSetForSpenders")]
    public bool UseSameRewardSetForSpenders { get; init; }
}
