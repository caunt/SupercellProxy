using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Buys a reward from the valley shop.</summary>
/// <param name="MapGameId">The map game id.</param>
/// <param name="RewardIndex">The reward index.</param>
/// <param name="Reward">The reward.</param>
/// <param name="Promotion">The promotion.</param>
public sealed record BuyMapGameShopRewardCommand(LongId MapGameId, int RewardIndex, MapGameShopPurchase? Reward, MapGameShopPromotion? Promotion) : CodecCommand<BuyMapGameShopRewardCommand>, ICommandCodecProvider<BuyMapGameShopRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyMapGameShopRewardCommand> Codec { get; } = Codecs.Command(
        type: 296,
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
