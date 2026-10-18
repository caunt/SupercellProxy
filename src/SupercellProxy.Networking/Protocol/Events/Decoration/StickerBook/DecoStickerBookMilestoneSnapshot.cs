using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>A saved sticker-book collection milestone: the reward earned once enough decorations are collected.</summary>
public sealed record DecoStickerBookMilestoneSnapshot
{
    /// <summary>Gets the number of collected decorations the milestone requires.</summary>
    [JsonPropertyName("decoCountRequired")]
    public int DecorationCountRequired { get; init; }

    /// <summary>Gets how many of the fallback item the milestone gives.</summary>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackRewardAmount { get; init; }

    /// <summary>Gets the data row of the reward given instead when the main reward is unavailable.</summary>
    [JsonPropertyName("fallbackRewardData")]
    public int FallbackRewardDataGlobalId { get; init; }

    /// <summary>Gets how many of the reward item the milestone gives.</summary>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }

    /// <summary>Gets the data row of the reward.</summary>
    [JsonPropertyName("rewardData")]
    public int RewardDataGlobalId { get; init; }
}
