using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers;

/// <summary>Delivers the goods of a purchased offer.</summary>
/// <param name="Goods">Goods the offer grants.</param>
/// <param name="OfferId">Offer that was bought.</param>
/// <param name="RewardMode">Delivery mode number native uses to decide where the goods go.</param>
/// <param name="Count">Purchase count sent with the delivery.</param>
/// <param name="ProductId">Store product the payment went through.</param>
public sealed record OfferDeliveredServerCommand(ReadOnlyMemory<OfferGood> Goods, int OfferId, int RewardMode, int Count, string? ProductId) : CodecServerCommand<OfferDeliveredServerCommand>, ICommandCodecProvider<OfferDeliveredServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OfferDeliveredServerCommand> Codec { get; } = Codecs.Command(
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
