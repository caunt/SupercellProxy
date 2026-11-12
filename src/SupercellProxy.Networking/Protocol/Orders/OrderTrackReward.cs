using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>One reward of an order-track milestone.</summary>
public sealed record OrderTrackReward
{
    /// <summary>Gets or sets how many of the reward are granted (saved as <c>Value</c>).</summary>
    /// <value>The quantity of <see cref="DataGlobalId"/> in the reward.</value>
    [JsonPropertyName("Value")]
    public int Count { get; init; }

    /// <summary>Gets or sets the global id of the rewarded table entry (saved as <c>ID</c>).</summary>
    /// <value>A global id of any grantable item, such as goods, tools, decorations or coins.</value>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }
}
