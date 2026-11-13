using System.Collections.Immutable;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains the Valley shop inventories that contribute to the shared promotion limit.</summary>
public sealed record MapGamePromotionShopSnapshot
{
    /// <summary>Gets or sets the retained inventories for individual map instances.</summary>
    /// <value>One inventory per Valley game instance, each identified by its <see cref="MapGamePromotionInventorySnapshot.MapInstanceId"/>. Empty by default.</value>
    public ImmutableArray<MapGamePromotionInventorySnapshot> MapGameRewards { get; init; } = [];
}
