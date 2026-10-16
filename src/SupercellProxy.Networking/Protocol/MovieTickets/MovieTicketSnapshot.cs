using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Saved state of one movie ticket placement: play limits, timers and the drawn reward.</summary>
public sealed record MovieTicketSnapshot
{
    /// <summary>Amount of the reward given before the current one.</summary>
    public int LastRewardAmount { get; init; }

    /// <summary>Global id of the table entry given as the reward before the current one.</summary>
    [JsonPropertyName("LastRewardId")]
    public int LastRewardId { get; init; }

    /// <summary>Unix hour at which the placement's daily limits reset.</summary>
    public int NextExpireHourIndex { get; init; }

    /// <summary>Seed the next reward is drawn with; it advances on every draw.</summary>
    public int RandomSeed { get; init; }

    /// <summary>Amount of the reward the next video gives.</summary>
    public int RewardAmount { get; init; }

    /// <summary>Global id of the table entry the next video gives as its reward.</summary>
    [JsonPropertyName("RewardId")]
    public int RewardId { get; init; }

    /// <summary>Unix time in seconds when the current reward was drawn.</summary>
    [JsonPropertyName("rewardRandomizedTimestamp")]
    public int RewardRandomizedTimestamp { get; init; }

    /// <summary>Whether the shop shows the placement's notification badge; null when the save does not carry it.</summary>
    [JsonPropertyName("shopNotificationVisible")]
    public bool? ShopNotificationVisible { get; init; }

    /// <summary>Where the placement is: 0 or 1 a video can be watched, 2 a video is being watched, 3 waiting for a play limit to reset.</summary>
    public int State { get; init; }

    /// <summary>Videos watched so far in each play-limit window, one count per window.</summary>
    public int[] VideosPlayed { get; init; } = [];

    /// <summary>Countdown of each play-limit window, one timer per window.</summary>
    public TimerSnapshot[] Timers { get; init; } = [];
}
