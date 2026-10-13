using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers;

/// <summary>Delivers the goods of a purchased offer.</summary>
/// <param name="Goods">The goods.</param>
/// <param name="OfferId">The offer id.</param>
/// <param name="RewardMode">The reward mode.</param>
/// <param name="Count">The count.</param>
/// <param name="ProductId">The product id.</param>
public sealed record OfferDeliveredServerCommand(ReadOnlyMemory<OfferGood> Goods, int OfferId, int RewardMode, int Count, string? ProductId) : CodecServerCommand<OfferDeliveredServerCommand>, ICommandCodecProvider<OfferDeliveredServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OfferDeliveredServerCommand> Codec { get; } = Codecs.Command(
        type: 372,
        Codecs.Composite(
            Codecs.Memory(OfferGood.Codec),
            static command => command.Goods,
            Codecs.VarInt,
            static command => command.OfferId,
            Codecs.VarInt,
            static command => command.RewardMode,
            Codecs.VarInt,
            static command => command.Count,
            Codecs.OptionalString,
            static command => command.ProductId,
            static (goods, offerId, rewardMode, count, productId) => new OfferDeliveredServerCommand(goods, offerId, rewardMode, count, productId)
        )
    );
}
