using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers.PopPromos;

/// <summary>Updates a pop promo stand offer.</summary>
/// <param name="HomeId">The home id.</param>
/// <param name="PromoId">The promo id.</param>
/// <param name="State">The state.</param>
/// <param name="RewardGlobalId">The reward data id.</param>
/// <param name="Amount">The amount.</param>
/// <param name="StartTime">The start time.</param>
/// <param name="Duration">The duration.</param>
/// <param name="Probability">The probability.</param>
public sealed record PopPromoUpdateServerCommand(LongId HomeId, int PromoId, int State, int RewardGlobalId, int Amount, int StartTime, int Duration, int Probability) : CodecServerCommand<PopPromoUpdateServerCommand>, ICommandCodecProvider<PopPromoUpdateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PopPromoUpdateServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.HomeId,
            Codecs.VarInt,
            static command => command.PromoId,
            Codecs.VarInt,
            static command => command.State,
            Codecs.VarInt,
            static command => command.RewardGlobalId,
            Codecs.VarInt,
            static command => command.Amount,
            Codecs.VarInt,
            static command => command.StartTime,
            Codecs.VarInt,
            static command => command.Duration,
            Codecs.VarInt,
            static command => command.Probability,
            static (homeId, promoId, state, rewardGlobalId, amount, startTime, duration, probability) => new PopPromoUpdateServerCommand(homeId, promoId, state, rewardGlobalId, amount, startTime, duration, probability)
        )
    );
}
