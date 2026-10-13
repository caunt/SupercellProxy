using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>The map game shop purchase.</summary>
/// <param name="RewardGlobalId">The reward data id.</param>
/// <param name="Price">The price.</param>
/// <param name="Amount">The amount.</param>
/// <param name="Costs">The costs.</param>
/// <param name="Rewards">The rewards.</param>
/// <param name="RewardSetName">The reward set name.</param>
public sealed record MapGameShopPurchase(
    int RewardGlobalId,
    int Price,
    int Amount,
    ReadOnlyMemory<MapGameShopGood> Costs,
    ReadOnlyMemory<MapGameShopGood> Rewards,
    string? RewardSetName
) : ICodecProvider<MapGameShopPurchase>
{
    /// <inheritdoc />
    public static ICodec<MapGameShopPurchase> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.RewardGlobalId,
        Codecs.VarInt,
        static value => value.Price,
        Codecs.VarInt,
        static value => value.Amount,
        Codecs.Memory(MapGameShopGood.Codec),
        static value => value.Costs,
        Codecs.Memory(MapGameShopGood.Codec),
        static value => value.Rewards,
        Codecs.OptionalString,
        static value => value.RewardSetName,
        static (rewardGlobalId, price, amount, costs, rewards, rewardSetName) => new MapGameShopPurchase(rewardGlobalId, price, amount, costs, rewards, rewardSetName)
    );
}
