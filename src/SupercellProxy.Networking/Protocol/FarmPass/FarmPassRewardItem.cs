using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Defines the Farm Pass Reward Item contract.
/// </summary>
public sealed record FarmPassRewardItem
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

    /// <summary>
    /// Gets the Shop Display Group value.
    /// </summary>
    [JsonPropertyName("shopDisplayGroup")]
    public int ShopDisplayGroup { get; init; } = -1;
}
