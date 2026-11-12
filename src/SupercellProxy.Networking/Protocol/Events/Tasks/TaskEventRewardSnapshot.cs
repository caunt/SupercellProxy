using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// One saved reward of a task event's points track, with the fallback the event offers when the reward cannot be received.
/// </summary>
public sealed record TaskEventRewardSnapshot
{
    /// <summary>
    /// Gets or sets a value indicating whether the reward was collected.
    /// </summary>
    /// <value><see langword="true"/> if the player has already claimed this step of the track; otherwise, <see langword="false"/>.</value>
    /// <remarks>An unclaimed reward keeps the default <see langword="false"/>.</remarks>
    [JsonPropertyName("claimed")]
    public bool Claimed { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the fallback took the reward's place.
    /// </summary>
    /// <value><see langword="true"/> if the claim granted the fallback instead of the reward; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("claimedFallback")]
    public bool ClaimedFallback { get; init; }

    /// <summary>
    /// Gets or sets how many of the fallback reward the player gets in its place.
    /// </summary>
    /// <value>The quantity of <see cref="FallbackRewardId"/> granted when the fallback applies; 0 when there is no fallback.</value>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackRewardAmount { get; init; }

    /// <summary>
    /// Gets or sets the global id of the fallback reward.
    /// </summary>
    /// <value>The global data identifier of the item granted instead of <see cref="RewardId"/>; 0 when the event defines no fallback.</value>
    [JsonPropertyName("fallbackRewardId")]
    public int FallbackRewardId { get; init; }

    /// <summary>
    /// Gets or sets the event points the reward needs.
    /// </summary>
    /// <value>The points threshold of this track step, on the same scale as <see cref="TaskEventSnapshot.Points"/>.</value>
    [JsonPropertyName("points")]
    public int Points { get; init; }

    /// <summary>
    /// Gets or sets how many of the reward the player gets.
    /// </summary>
    /// <value>The quantity of <see cref="RewardId"/> the claim grants when the fallback does not apply.</value>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }

    /// <summary>
    /// Gets or sets the global id of the reward.
    /// </summary>
    /// <value>The global data identifier of the item this track step grants on a normal claim.</value>
    [JsonPropertyName("rewardId")]
    public int RewardId { get; init; }
}
