using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>A saved sticker-book collection milestone: the reward earned once enough decorations are collected.</summary>
public sealed record DecoStickerBookMilestoneSnapshot
{
    /// <summary>Gets whether the milestone's reward was claimed.</summary>
    [JsonPropertyName("claimed")]
    public bool Claimed { get; init; }

    /// <summary>Gets how many of the claimed reward the player got, saved once the milestone is claimed.</summary>
    [JsonPropertyName("claimedRewardAmount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ClaimedRewardAmount { get; init; }

    /// <summary>Gets the data row of the reward the player got, saved once the milestone is claimed.</summary>
    [JsonPropertyName("claimedRewardData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ClaimedRewardDataGlobalId { get; init; }

    /// <summary>Gets the number of collected decorations the milestone requires.</summary>
    [JsonPropertyName("decoCountRequired")]
    public int DecorationCountRequired { get; init; }

    /// <summary>Gets how many of the fallback item the milestone gives.</summary>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackRewardAmount { get; init; }

    /// <summary>Gets the data row of the reward given instead when the main reward is unavailable.</summary>
    [JsonPropertyName("fallbackRewardData")]
    public int FallbackRewardDataGlobalId { get; init; }

    /// <summary>Gets whether the fallback reward stays as saved when the collection's event changes.</summary>
    [JsonPropertyName("fallbackRewardFrozen")]
    public bool FallbackRewardFrozen { get; init; }

    /// <summary>Gets whether the milestone is hidden; a hidden milestone cannot be claimed.</summary>
    [JsonPropertyName("hidden")]
    public bool Hidden { get; init; }

    /// <summary>Gets how many of the reward item the milestone gives.</summary>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }

    /// <summary>Gets the data row of the reward.</summary>
    [JsonPropertyName("rewardData")]
    public int RewardDataGlobalId { get; init; }

    /// <summary>Gets whether the reward stays as saved when the collection's event changes.</summary>
    [JsonPropertyName("rewardFrozen")]
    public bool RewardFrozen { get; init; }
}
