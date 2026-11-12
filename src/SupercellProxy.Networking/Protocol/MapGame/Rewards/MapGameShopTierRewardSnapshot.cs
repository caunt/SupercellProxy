using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains the resolved contents and prices of a Valley shop offer.</summary>
public sealed record MapGameShopTierRewardSnapshot
{
    /// <summary>Gets or sets the day on which the offer unlocks, or -1 when unrestricted.</summary>
    /// <value>The unlock day saved as <c>m_unlockDay</c>; -1 by default.</value>
    [JsonPropertyName("m_unlockDay")]
    public int UnlockDay { get; init; } = -1;

    /// <summary>Gets or sets the ordered reward contents.</summary>
    /// <value>Resource ids and amounts the offer grants, in definition order; empty by default.</value>
    [JsonPropertyName("rewards")]
    public MapGameRewardAmountSnapshot[] Rewards { get; init; } = [];

    /// <summary>Gets or sets the shop reward tier.</summary>
    /// <value>The tier number; 2 for exclusive offers.</value>
    [JsonPropertyName("tier")]
    public int Tier { get; init; }

    /// <summary>Gets or sets the optional localized title.</summary>
    /// <value>The title copied from the reward definition; <see langword="null"/> when it has none.</value>
    [JsonPropertyName("rewardTitle")]
    public string? Title { get; init; }

    /// <summary>Gets or sets the ordered token prices.</summary>
    /// <value>Valley coin ids and amounts the offer costs, in definition order; empty by default.</value>
    [JsonPropertyName("prices")]
    public MapGameRewardAmountSnapshot[] Prices { get; init; } = [];
}
