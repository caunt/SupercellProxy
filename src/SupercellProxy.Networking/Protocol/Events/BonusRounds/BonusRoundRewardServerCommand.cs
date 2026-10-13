using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.BonusRounds;

/// <summary>Reveals a bonus round reward slot.</summary>
/// <param name="RewardAmount">The reward amount.</param>
/// <param name="Round">The round.</param>
/// <param name="MaximumRewards">The maximum rewards.</param>
/// <param name="SlotIndex">The slot index.</param>
/// <param name="BonusRoundId">The bonus round id.</param>
/// <param name="RewardGlobalId">The reward data id.</param>
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
