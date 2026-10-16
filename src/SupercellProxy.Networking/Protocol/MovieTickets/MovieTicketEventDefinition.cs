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
    /// <summary>Name of the movie ticket placement the event overrides, matching the placement's table entry name.</summary>
    [JsonPropertyName("AdPlacement")]
    public string? AdPlacement { get; init; }

    /// <summary>How many videos a player may watch per day at this placement.</summary>
    [JsonPropertyName("DailyAds")]
    public int DailyAds { get; init; }

    /// <summary>Rewards used as the first reward (when the game configuration enables that) for players who are not recent spenders.</summary>
    [JsonPropertyName("RewardSetForcedNonSpender")]
    public MovieTicketRewardDefinition[] ForcedNonSpenderRewards { get; init; } = [];

    /// <summary>The same first-reward set for recent spenders.</summary>
    [JsonPropertyName("RewardSetForcedSpender")]
    public MovieTicketRewardDefinition[] ForcedSpenderRewards { get; init; } = [];

    /// <summary>Play-limit windows: how many videos fit in each window and how long each window lasts.</summary>
    [JsonPropertyName("MoviesCycle")]
    public MovieTicketCycleDefinition[] MoviesCycle { get; init; } = [];

    /// <summary>Gets the legacy spender-cycle array; current native cycle limits use MoviesCycle for both account categories.</summary>
    [JsonPropertyName("MoviesCycleSpender")]
    public MovieTicketCycleDefinition[] SpenderMoviesCycle { get; init; } = [];

    /// <summary>Weighted reward pool the following rewards are drawn from, for players who are not recent spenders.</summary>
    [JsonPropertyName("RewardRandomSetNonSpender")]
    public MovieTicketRewardDefinition[] RandomNonSpenderRewards { get; init; } = [];

    /// <summary>Gets the placement level override used by movie-ticket availability.</summary>
    [JsonPropertyName("RequiredLevel")]
    public int RequiredLevel { get; init; }

    /// <summary>Level limits a player must meet for the event to apply.</summary>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Seasonal currency the event's rewards can pay out in; null when it has none.</summary>
    [JsonPropertyName("SeasonalCurrency")]
    public EventSeasonalCurrencyDefinition? SeasonalCurrency { get; init; }

    /// <summary>Daily video limit for recent spenders, used unless <see cref="UseSameCycleForSpender"/> is set.</summary>
    [JsonPropertyName("DailyAdsSpender")]
    public int SpenderDailyAds { get; init; }

    /// <summary>Weighted reward pool for recent spenders, used unless <see cref="UseSameRewardSetForSpenders"/> is set.</summary>
    [JsonPropertyName("RewardRandomSetSpender")]
    public MovieTicketRewardDefinition[] RandomSpenderRewards { get; init; } = [];

    /// <summary>When set, recent spenders get the same daily video limit as everyone else.</summary>
    [JsonPropertyName("useSameCycleForSpender")]
    public bool UseSameCycleForSpender { get; init; }

    /// <summary>When set, recent spenders get the same reward sets as everyone else.</summary>
    [JsonPropertyName("useSameRewardSetForSpenders")]
    public bool UseSameRewardSetForSpenders { get; init; }
}
