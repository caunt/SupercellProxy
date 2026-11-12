using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Saved state of one movie ticket placement: play limits, timers and the drawn reward.</summary>
public sealed record MovieTicketSnapshot
{
    /// <summary>Gets or sets the amount of the reward given before the current one.</summary>
    /// <value>The previous value of <see cref="RewardAmount"/>; 0 before the first reward.</value>
    public int LastRewardAmount { get; init; }

    /// <summary>Gets or sets the global id of the table entry given as the reward before the current one.</summary>
    /// <value>The previous value of <see cref="RewardId"/>; 0 before the first reward.</value>
    [JsonPropertyName("LastRewardId")]
    public int LastRewardId { get; init; }

    /// <summary>Gets or sets the Unix hour at which the placement's daily limits reset.</summary>
    /// <value>Hours since 1970-01-01 UTC; 0 or less means no daily reset is scheduled.</value>
    public int NextExpireHourIndex { get; init; }

    /// <summary>Gets or sets the placement's saved random seed.</summary>
    /// <value>The random generator state as saved, carried through unchanged.</value>
    public int RandomSeed { get; init; }

    /// <summary>Gets or sets the amount of the reward the next video gives.</summary>
    /// <value>The quantity of <see cref="RewardId"/> granted when the video is collected; 0 or less means there is nothing to collect.</value>
    public int RewardAmount { get; init; }

    /// <summary>Gets or sets the global id of the table entry the next video gives as its reward.</summary>
    /// <value>A global id of a money, goods, tool, voucher, decoration or seasonal currency row, already resolved to the free money variant for coins and diamonds.</value>
    [JsonPropertyName("RewardId")]
    public int RewardId { get; init; }

    /// <summary>Gets or sets the Unix time in seconds when the current reward was drawn.</summary>
    /// <value>Server time, in seconds since 1970-01-01 UTC, of the last reward draw; saved as <c>rewardRandomizedTimestamp</c>.</value>
    [JsonPropertyName("rewardRandomizedTimestamp")]
    public int RewardRandomizedTimestamp { get; init; }

    /// <summary>Gets or sets whether the shop shows the placement's notification badge; <see langword="null"/> when the save does not carry it.</summary>
    /// <value><see langword="true"/> if a video is ready and the badge has not been cleared; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("shopNotificationVisible")]
    public bool? ShopNotificationVisible { get; init; }

    /// <summary>Gets or sets where the placement is: 0 or 1 a video can be watched, 2 a video is being watched, 3 waiting for a play limit to reset.</summary>
    /// <value>A state number from 0 to 3.</value>
    public int State { get; init; }

    /// <summary>Gets or sets the videos watched so far in each play-limit window, one count per window.</summary>
    /// <value>Counts in the same order as <see cref="Timers"/>, the daily window last.</value>
    public int[] VideosPlayed { get; init; } = [];

    /// <summary>Gets or sets the countdown of each play-limit window, one timer per window.</summary>
    /// <value>Timers in the same order as <see cref="VideosPlayed"/>, counted in timer ticks.</value>
    public TimerSnapshot[] Timers { get; init; } = [];
}
