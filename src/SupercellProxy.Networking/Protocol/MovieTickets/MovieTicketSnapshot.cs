using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>
/// Defines the Movie Ticket Snapshot contract.
/// </summary>
public sealed record MovieTicketSnapshot
{

    /// <summary>
    /// Gets the Last Reward Amount value.
    /// </summary>
    public int LastRewardAmount { get; init; }

    /// <summary>
    /// Gets the Last Reward Id value.
    /// </summary>
    [JsonPropertyName("LastRewardId")]
    public int LastRewardIdentifier { get; init; }

    /// <summary>
    /// Gets the Next Expire Hour Index value.
    /// </summary>
    public int NextExpireHourIndex { get; init; }

    /// <summary>
    /// Gets the Random Seed value.
    /// </summary>
    public int RandomSeed { get; init; }

    /// <summary>
    /// Gets the Reward Amount value.
    /// </summary>
    public int RewardAmount { get; init; }

    /// <summary>
    /// Gets the Reward Id value.
    /// </summary>
    [JsonPropertyName("RewardId")]
    public int RewardIdentifier { get; init; }

    /// <summary>
    /// Gets the Reward Randomized Timestamp value.
    /// </summary>
    [JsonPropertyName("rewardRandomizedTimestamp")]
    public int RewardRandomizedTimestamp { get; init; }

    /// <summary>
    /// Gets the Shop Notification Visible value.
    /// </summary>
    [JsonPropertyName("shopNotificationVisible")]
    public bool? ShopNotificationVisible { get; init; }
    /// <summary>
    /// Gets the State value.
    /// </summary>
    public int State { get; init; }

    /// <summary>
    /// Gets the Videos Played value.
    /// </summary>
    public int[] VideosPlayed { get; init; } = [];

    /// <summary>
    /// Gets the Timers value.
    /// </summary>
    public TimerSnapshot[] Timers { get; init; } = [];
}
