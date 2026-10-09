using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Names a Valley token and its price.</summary>
public sealed record MapGameRewardPriceDefinition
{
    /// <summary>Gets the token amount.</summary>
    [JsonPropertyName("tokenAmount")]
    public int Amount { get; init; }

    /// <summary>Gets the token resource name.</summary>
    [JsonPropertyName("token")]
    public string Name { get; init; } = string.Empty;
}
