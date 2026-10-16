using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers.PopPromos;

/// <summary>Updates a pop promo stand offer.</summary>
/// <param name="HomeId">Farm whose pop promo stand changes.</param>
/// <param name="PromoId">Offer on the stand.</param>
/// <param name="State">New state of the offer, as native numbers it.</param>
/// <param name="RewardGlobalId">Global id of what the offer gives.</param>
/// <param name="Amount">How many of that reward.</param>
/// <param name="StartTime">Unix time stamp when the offer appears.</param>
/// <param name="Duration">How long the offer stays after it appears.</param>
/// <param name="Probability">Chance value the server sends with the offer.</param>
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
