using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Defines the Order Track Reward contract.
/// </summary>
public sealed record OrderTrackReward
{

    /// <summary>
    /// Gets the Count value.
    /// </summary>
    [JsonPropertyName("Value")]
    public int Count { get; init; }
    /// <summary>
    /// Gets the Data Global Id value.
    /// </summary>
    [JsonPropertyName("ID")]
    public int DataGlobalIdentifier { get; init; }
}
