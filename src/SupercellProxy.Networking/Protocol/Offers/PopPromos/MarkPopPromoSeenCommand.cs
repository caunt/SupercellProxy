using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers.PopPromos;

/// <summary>Records that a pop promo stand offer was shown.</summary>
/// <param name="PromoId">The promo id.</param>
public sealed record MarkPopPromoSeenCommand(int PromoId) : CodecCommand<MarkPopPromoSeenCommand>, ICommandCodecProvider<MarkPopPromoSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkPopPromoSeenCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.PromoId, static (promoId) => new MarkPopPromoSeenCommand(promoId)));
}
