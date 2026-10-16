using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>One reward of an order-track milestone.</summary>
public sealed record OrderTrackReward
{
    /// <summary>How many of the reward are granted (saved as <c>Value</c>).</summary>
    [JsonPropertyName("Value")]
    public int Count { get; init; }

    /// <summary>Global id of the rewarded table entry (saved as <c>ID</c>).</summary>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }
}
