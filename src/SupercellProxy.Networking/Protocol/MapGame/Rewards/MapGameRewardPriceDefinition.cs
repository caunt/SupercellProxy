using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Names a Valley token and its price.</summary>
public sealed record MapGameRewardPriceDefinition
{
    /// <summary>Gets or sets the token amount.</summary>
    /// <value>How many of the token the offer costs, from <c>tokenAmount</c>.</value>
    [JsonPropertyName("tokenAmount")]
    public int Amount { get; init; }

    /// <summary>Gets or sets the token resource name.</summary>
    /// <value>The row name of a Valley coin, from <c>token</c>. Empty by default.</value>
    [JsonPropertyName("token")]
    public string Name { get; init; } = string.Empty;
}
