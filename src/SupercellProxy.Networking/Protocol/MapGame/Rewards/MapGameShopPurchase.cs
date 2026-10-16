using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>A reward bought in the Valley shop, with what it cost and what it gives.</summary>
/// <param name="RewardGlobalId">Global id of the shop reward's table entry.</param>
/// <param name="Price">Price of the reward in the shop.</param>
/// <param name="Amount">How many of the reward were bought.</param>
/// <param name="Costs">Resources the purchase spends.</param>
/// <param name="Rewards">Resources the purchase gives.</param>
/// <param name="RewardSetName">Name of the reward set the contents were drawn from; null when none.</param>
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
