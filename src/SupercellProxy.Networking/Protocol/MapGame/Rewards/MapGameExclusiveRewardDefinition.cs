using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Defines one event-configured exclusive Valley reward alternative.</summary>
public sealed record MapGameExclusiveRewardDefinition
{
    /// <summary>Gets or sets the resources included in the reward.</summary>
    /// <value>Resources named by table row name with amounts. Empty by default.</value>
    [JsonPropertyName("rewards")]
    public MapGameRewardResourceDefinition[] Rewards { get; init; } = [];

    /// <summary>Gets or sets the token prices in native order.</summary>
    /// <value>Each price names a Valley coin row and an amount. Empty by default.</value>
    [JsonPropertyName("rewardPrices")]
    public MapGameRewardPriceDefinition[] Prices { get; init; } = [];

    /// <summary>Gets or sets the optional localized reward title.</summary>
    /// <value>The title text or key; <see langword="null"/> when the definition has none.</value>
    [JsonPropertyName("rewardTitle")]
    public string? Title { get; init; }
}
