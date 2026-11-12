using SupercellProxy.Networking.Protocol.MapGame.Wallets;
using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains the ordinary shop reward groups for one Valley instance.</summary>
public sealed record MapGamePromotionInventorySnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the owning map instance.</summary>
    /// <value>The id of the Valley game this inventory belongs to; <see langword="null"/> when not saved.</value>
    public MapGameInstanceIdSnapshot? MapInstanceId { get; init; }
    /// <summary>Gets or sets the reward groups with their promotion boxes.</summary>
    /// <value>Daily groups of ordinary shop choices. Empty by default.</value>
    public MapGamePromotionRewardGroupSnapshot[] RewardsTier1 { get; init; } = [];

    /// <summary>Gets or sets the three exclusive shop offers resolved from the season configuration.</summary>
    /// <value>One offer per exclusive slot, in slot order. Empty by default.</value>
    public MapGameShopRewardSnapshot[] ShopRewardsTier2 { get; init; } = [];
}
