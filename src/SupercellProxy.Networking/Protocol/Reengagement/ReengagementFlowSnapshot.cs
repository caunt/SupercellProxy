using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Reengagement;

/// <summary>
/// Defines the Reengagement Flow Snapshot contract.
/// </summary>
public sealed record ReengagementFlowSnapshot
{

    /// <summary>
    /// Gets the Coin Reward value.
    /// </summary>
    [JsonPropertyName("coinReward")]
    public int CoinReward { get; init; }

    /// <summary>
    /// Gets the Diamond Reward value.
    /// </summary>
    [JsonPropertyName("diamondReward")]
    public int DiamondReward { get; init; }
    /// <summary>
    /// Gets the Ui Step value.
    /// </summary>
    [JsonPropertyName("uiStep")]
    public int UserInterfaceStep { get; init; }
}
