using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers.PopPromos;

/// <summary>Claims the reward of a pop promo stand offer.</summary>
/// <param name="PromoId">Pop promo stand offer whose reward to claim.</param>
public sealed record ClaimPopPromoRewardCommand(int PromoId) : CodecCommand<ClaimPopPromoRewardCommand>, ICommandCodecProvider<ClaimPopPromoRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimPopPromoRewardCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.PromoId, static promoId => new ClaimPopPromoRewardCommand(promoId)));
}
