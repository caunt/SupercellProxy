using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Buys a reward from the valley shop.</summary>
/// <param name="MapGameId">Id of the Valley board whose shop is used.</param>
/// <param name="RewardIndex">Position of the bought reward in the shop.</param>
/// <param name="Reward">The bought reward with its price and contents; null when not sent.</param>
/// <param name="Promotion">Promotion box sent with the purchase; null when there is none.</param>
public sealed record BuyMapGameShopRewardCommand(LongId MapGameId, int RewardIndex, MapGameShopPurchase? Reward, MapGameShopPromotion? Promotion) : CodecCommand<BuyMapGameShopRewardCommand>, ICommandCodecProvider<BuyMapGameShopRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyMapGameShopRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.MapGameId,
            Codecs.VarInt,
            static command => command.RewardIndex,
            Codecs.Optional(MapGameShopPurchase.Codec),
            static command => command.Reward,
            Codecs.Optional(MapGameShopPromotion.Codec),
            static command => command.Promotion,
            static (mapGameId, rewardIndex, reward, promotion) => new BuyMapGameShopRewardCommand(mapGameId, rewardIndex, reward, promotion)
        ),
        CommandLayout.MetadataFirst
    );
}
