using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.BonusRounds;

/// <summary>Finishes the current bonus round, optionally moving its rewards to the piggy bank.</summary>
/// <param name="RoundId">The bonus round id.</param>
/// <param name="ToPiggyBank">Whether the rewards go to the piggy bank.</param>
/// <param name="Round">The finished round number.</param>
/// <param name="EventId">The event that owns the bonus round.</param>
public sealed record FinishBonusRoundCommand(int RoundId, bool ToPiggyBank, int Round, int EventId) : CodecCommand<FinishBonusRoundCommand>, ICommandCodecProvider<FinishBonusRoundCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FinishBonusRoundCommand> Codec { get; } = Codecs.Command(
        type: 316,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.RoundId,
            Codecs.Boolean,
            static command => command.ToPiggyBank,
            Codecs.VarInt,
            static command => command.Round,
            Codecs.VarInt,
            static command => command.EventId,
            static (roundId, toPiggyBank, round, eventId) => new FinishBonusRoundCommand(roundId, toPiggyBank, round, eventId)
        )
    );
}
