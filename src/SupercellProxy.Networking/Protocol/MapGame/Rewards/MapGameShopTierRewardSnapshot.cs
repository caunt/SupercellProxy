using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains the resolved contents and prices of a Valley shop offer.</summary>
public sealed record MapGameShopTierRewardSnapshot
{
    /// <summary>Gets the day on which the offer unlocks, or -1 when unrestricted.</summary>
    [JsonPropertyName("m_unlockDay")]
    public int UnlockDay { get; init; } = -1;

    /// <summary>Gets the ordered reward contents.</summary>
    [JsonPropertyName("rewards")]
    public MapGameRewardAmountSnapshot[] Rewards { get; init; } = [];

    /// <summary>Gets the shop reward tier.</summary>
    [JsonPropertyName("tier")]
    public int Tier { get; init; }

    /// <summary>Gets the optional localized title.</summary>
    [JsonPropertyName("rewardTitle")]
    public string? Title { get; init; }

    /// <summary>Gets the ordered token prices.</summary>
    [JsonPropertyName("prices")]
    public MapGameRewardAmountSnapshot[] Prices { get; init; } = [];
}
