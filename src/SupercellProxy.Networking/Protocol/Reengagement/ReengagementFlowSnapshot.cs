using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Reengagement;

/// <summary>
/// Saved state of the welcome-back flow shown to players who return after a long break.
/// </summary>
public sealed record ReengagementFlowSnapshot
{
    /// <summary>
    /// Gets or sets the coins the welcome-back flow gives the returning player.
    /// </summary>
    /// <value>The coin amount saved under <c>coinReward</c>; 0 when the save omits the key.</value>
    [JsonPropertyName("coinReward")]
    public int CoinReward { get; init; }

    /// <summary>
    /// Gets or sets the diamonds the welcome-back flow gives the returning player.
    /// </summary>
    /// <value>The diamond amount saved under <c>diamondReward</c>; 0 when the save omits the key.</value>
    [JsonPropertyName("diamondReward")]
    public int DiamondReward { get; init; }
    /// <summary>
    /// Gets or sets how far the player has got through the welcome-back screens.
    /// </summary>
    /// <value>The step number saved under <c>uiStep</c>, 0 when the key is missing; which number stands for which screen is not yet identified.</value>
    [JsonPropertyName("uiStep")]
    public int UserInterfaceStep { get; init; }
}
