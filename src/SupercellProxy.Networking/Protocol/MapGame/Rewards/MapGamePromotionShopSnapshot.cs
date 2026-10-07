namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains the Valley shop inventories that contribute to the shared promotion limit.</summary>
public sealed record MapGamePromotionShopSnapshot
{
    /// <summary>Gets the retained inventories for individual map instances.</summary>
    public MapGamePromotionInventorySnapshot[] MapGameRewards { get; init; } = [];
}
