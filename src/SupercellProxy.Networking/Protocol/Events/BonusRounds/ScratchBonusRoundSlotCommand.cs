using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.BonusRounds;

/// <summary>Scratches one reward slot of the current bonus round.</summary>
/// <param name="RoundId">The bonus round id.</param>
/// <param name="Round">The round number.</param>
/// <param name="SlotIndex">The scratched slot.</param>
/// <param name="RewardGlobalId">The reward expected in the slot.</param>
public sealed record ScratchBonusRoundSlotCommand(int RoundId, int Round, int SlotIndex, int RewardGlobalId) : CodecCommand<ScratchBonusRoundSlotCommand>, ICommandCodecProvider<ScratchBonusRoundSlotCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ScratchBonusRoundSlotCommand> Codec { get; } = Codecs.Command(
        type: 317,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.RoundId,
            Codecs.VarInt,
            static command => command.Round,
            Codecs.VarInt,
            static command => command.SlotIndex,
            Codecs.VarInt,
            static command => command.RewardGlobalId,
            static (roundId, round, slotIndex, rewardGlobalId) => new ScratchBonusRoundSlotCommand(roundId, round, slotIndex, rewardGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
