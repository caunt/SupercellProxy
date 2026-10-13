using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.BonusRounds;

/// <summary>Pays to start the next round of the current bonus round.</summary>
/// <param name="RoundId">The bonus round id.</param>
/// <param name="Round">The round being bought.</param>
/// <param name="Price">The expected price.</param>
/// <param name="CurrencyGlobalId">The currency data id.</param>
public sealed record BuyNextBonusRoundCommand(int RoundId, int Round, int Price, int CurrencyGlobalId) : CodecCommand<BuyNextBonusRoundCommand>, ICommandCodecProvider<BuyNextBonusRoundCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyNextBonusRoundCommand> Codec { get; } = Codecs.Command(
        type: 319,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.RoundId,
            Codecs.VarInt,
            static command => command.Round,
            Codecs.VarInt,
            static command => command.Price,
            Codecs.VarInt,
            static command => command.CurrencyGlobalId,
            static (roundId, round, price, currencyGlobalId) => new BuyNextBonusRoundCommand(roundId, round, price, currencyGlobalId)
        )
    );
}
