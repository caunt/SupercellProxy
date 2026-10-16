using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Reengagement;

/// <summary>
/// Saved state of the welcome-back flow shown to players who return after a long break.
/// </summary>
public sealed record ReengagementFlowSnapshot
{
    /// <summary>
    /// Coins the welcome-back flow gives the returning player.
    /// </summary>
    [JsonPropertyName("coinReward")]
    public int CoinReward { get; init; }

    /// <summary>
    /// Diamonds the welcome-back flow gives the returning player.
    /// </summary>
    [JsonPropertyName("diamondReward")]
    public int DiamondReward { get; init; }
    /// <summary>
    /// Gets the Ui Step value.
    /// </summary>
    [JsonPropertyName("uiStep")]
    public int UserInterfaceStep { get; init; }
}
