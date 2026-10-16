using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>One reward of a Farm Pass level, resolved to its table entry when the season starts.</summary>
public sealed record FarmPassRewardItem
{
    /// <summary>How many of the reward are given (saved as <c>Value</c>).</summary>
    [JsonPropertyName("Value")]
    public int Count { get; init; }

    /// <summary>Global id of the rewarded table entry (saved as <c>ID</c>).</summary>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>Shop group the reward is shown with; -1 when it has none.</summary>
    [JsonPropertyName("shopDisplayGroup")]
    public int ShopDisplayGroup { get; init; } = -1;
}
