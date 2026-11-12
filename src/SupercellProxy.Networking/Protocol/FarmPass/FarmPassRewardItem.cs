using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>One reward of a Farm Pass level, resolved to its table entry when the season starts.</summary>
public sealed record FarmPassRewardItem
{
    /// <summary>Gets or sets how many of the reward are given (saved as <c>Value</c>).</summary>
    /// <value>The amount saved for this player, which can differ from the event's <c>rewardAmount</c>.</value>
    [JsonPropertyName("Value")]
    public int Count { get; init; }

    /// <summary>Gets or sets the global id of the rewarded table entry (saved as <c>ID</c>).</summary>
    /// <value>The row the reward grants, such as a good, decoration, Farm Pass perk or decision box; it may name a different row than the event.</value>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets or sets the shop group the reward is shown with; -1 when it has none.</summary>
    /// <value>The group index saved as <c>shopDisplayGroup</c>; defaults to -1 when the save omits it.</value>
    [JsonPropertyName("shopDisplayGroup")]
    public int ShopDisplayGroup { get; init; } = -1;
}
