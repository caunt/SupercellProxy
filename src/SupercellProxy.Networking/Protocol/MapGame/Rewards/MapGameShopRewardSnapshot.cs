using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains one exclusive shop offer and its native wrapper state.</summary>
public sealed record MapGameShopRewardSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the resolved reward and its token prices.</summary>
    /// <value>The offer saved under <c>tierReward</c>; <see langword="null"/> when the slot holds no offer.</value>
    [JsonPropertyName("tierReward")]
    public MapGameShopTierRewardSnapshot? Reward { get; init; }
}
