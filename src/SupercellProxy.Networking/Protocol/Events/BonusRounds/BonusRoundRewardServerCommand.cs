using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.BonusRounds;

/// <summary>Reveals a bonus round reward slot.</summary>
/// <param name="RewardAmount">How many of the reward the slot holds.</param>
/// <param name="Round">Which round of the bonus game the slot is in.</param>
/// <param name="MaximumRewards">Upper limit on rewards in this round, as the server sets it.</param>
/// <param name="SlotIndex">Position of the revealed slot.</param>
/// <param name="BonusRoundId">The bonus game the slot belongs to.</param>
/// <param name="RewardGlobalId">Global id of what the slot gives.</param>
public sealed record BonusRoundRewardServerCommand(int RewardAmount, int Round, int MaximumRewards, int SlotIndex, int BonusRoundId, int RewardGlobalId) : CodecServerCommand<BonusRoundRewardServerCommand>, ICommandCodecProvider<BonusRoundRewardServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BonusRoundRewardServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.RewardAmount,
            Codecs.VarInt,
            static command => command.Round,
            Codecs.VarInt,
            static command => command.MaximumRewards,
            Codecs.VarInt,
            static command => command.SlotIndex,
            Codecs.VarInt,
            static command => command.BonusRoundId,
            Codecs.VarInt,
            static command => command.RewardGlobalId,
            static (rewardAmount, round, maximumRewards, slotIndex, bonusRoundId, rewardGlobalId) => new BonusRoundRewardServerCommand(rewardAmount, round, maximumRewards, slotIndex, bonusRoundId, rewardGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
