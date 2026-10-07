using SupercellProxy.Networking.Protocol.MapGame.Wallets;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains the ordinary shop reward groups for one Valley instance.</summary>
public sealed record MapGamePromotionInventorySnapshot
{
    /// <summary>Gets the owning map instance.</summary>
    public MapGameInstanceIdSnapshot? MapInstanceId { get; init; }
    /// <summary>Gets the reward groups whose promotion boxes count toward the shared limit.</summary>
    public MapGamePromotionRewardGroupSnapshot[] RewardsTier1 { get; init; } = [];
}
