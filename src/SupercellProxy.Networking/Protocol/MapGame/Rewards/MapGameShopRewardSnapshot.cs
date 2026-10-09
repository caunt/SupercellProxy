using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains one exclusive shop offer and its native wrapper state.</summary>
public sealed record MapGameShopRewardSnapshot : ExtensibleDocument
{
    /// <summary>Gets the resolved reward and its token prices.</summary>
    [JsonPropertyName("tierReward")]
    public MapGameShopTierRewardSnapshot? Reward { get; init; }
}
