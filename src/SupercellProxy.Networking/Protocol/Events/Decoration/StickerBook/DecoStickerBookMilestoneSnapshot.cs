using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>A saved sticker-book collection milestone: the reward earned once enough decorations are collected.</summary>
public sealed record DecoStickerBookMilestoneSnapshot
{
    /// <summary>Gets or sets whether the milestone's reward was claimed.</summary>
    /// <value><see langword="true"/> if the milestone has been claimed; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("claimed")]
    public bool Claimed { get; init; }

    /// <summary>Gets or sets how many of the claimed reward the player got, saved once the milestone is claimed.</summary>
    /// <value>The quantity of the main or fallback reward actually granted, or <see langword="null"/> (and left out of the save) while the milestone is unclaimed.</value>
    [JsonPropertyName("claimedRewardAmount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ClaimedRewardAmount { get; init; }

    /// <summary>Gets or sets the data row of the reward the player got, saved once the milestone is claimed.</summary>
    /// <value>The data-table global id of whichever reward, main or fallback, was granted, or <see langword="null"/> (and left out of the save) while the milestone is unclaimed.</value>
    [JsonPropertyName("claimedRewardData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ClaimedRewardDataGlobalId { get; init; }

    /// <summary>Gets or sets the number of collected decorations the milestone requires.</summary>
    /// <value>The minimum count of collected slots, among the decorations the collection lists, saved as <c>decoCountRequired</c>.</value>
    [JsonPropertyName("decoCountRequired")]
    public int DecorationCountRequired { get; init; }

    /// <summary>Gets or sets how many of the fallback item the milestone gives.</summary>
    /// <value>The quantity of <see cref="FallbackRewardDataGlobalId"/> given in place of the main reward.</value>
    [JsonPropertyName("fallbackRewardAmount")]
    public int FallbackRewardAmount { get; init; }

    /// <summary>Gets or sets the data row of the reward given instead when the main reward is unavailable.</summary>
    /// <value>A data-table global id, saved as <c>fallbackRewardData</c>.</value>
    [JsonPropertyName("fallbackRewardData")]
    public int FallbackRewardDataGlobalId { get; init; }

    /// <summary>Gets or sets whether the fallback reward stays as saved when the collection's event changes.</summary>
    /// <value><see langword="true"/> if the saved fallback reward is frozen against event changes; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("fallbackRewardFrozen")]
    public bool FallbackRewardFrozen { get; init; }

    /// <summary>Gets or sets whether the milestone is hidden.</summary>
    /// <value><see langword="true"/> if the milestone is hidden from the player; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("hidden")]
    public bool Hidden { get; init; }

    /// <summary>Gets or sets how many of the reward item the milestone gives.</summary>
    /// <value>The quantity of <see cref="RewardDataGlobalId"/> the main reward gives.</value>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }

    /// <summary>Gets or sets the data row of the reward.</summary>
    /// <value>The data-table global id of the main reward (saved as <c>rewardData</c>).</value>
    [JsonPropertyName("rewardData")]
    public int RewardDataGlobalId { get; init; }

    /// <summary>Gets or sets whether the reward stays as saved when the collection's event changes.</summary>
    /// <value><see langword="true"/> if the saved main reward is frozen against event changes; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("rewardFrozen")]
    public bool RewardFrozen { get; init; }
}
