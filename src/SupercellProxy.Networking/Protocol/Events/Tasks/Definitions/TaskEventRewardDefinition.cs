using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks.Definitions;

/// <summary>
/// One reward a task event's document defines, with the fallback used when the reward cannot be received.
/// </summary>
public sealed record TaskEventRewardDefinition
{
    /// <summary>
    /// Gets or sets the name of the fallback reward's table row.
    /// </summary>
    /// <value>The <c>fallbackReward</c> row name of the alternative to <see cref="Reward"/>, or <see langword="null"/> when the reward has no fallback.</value>
    [JsonPropertyName("fallbackReward")]
    public string? FallbackReward { get; init; }

    /// <summary>
    /// Gets or sets how many of the fallback reward the player gets in its place.
    /// </summary>
    /// <value>The <c>fallbackRewardAmount</c> quantity of <see cref="FallbackReward"/>; 0 when the document omits it.</value>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackRewardAmount { get; init; }

    /// <summary>
    /// Gets or sets the event points the reward needs.
    /// </summary>
    /// <value>The <c>points</c> threshold of this reward on the event's points track.</value>
    [JsonPropertyName("points")]
    public int Points { get; init; }

    /// <summary>
    /// Gets or sets the name of the reward's table row.
    /// </summary>
    /// <value>The <c>reward</c> row name of the item granted; an empty string when the document omits it.</value>
    [JsonPropertyName("reward")]
    public string Reward { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets how many of the reward the player gets.
    /// </summary>
    /// <value>The <c>rewardAmount</c> quantity of <see cref="Reward"/> granted on claim; 0 when the document omits it.</value>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }
}
