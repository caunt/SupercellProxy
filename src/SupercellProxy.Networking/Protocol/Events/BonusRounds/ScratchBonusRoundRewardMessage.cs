using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.BonusRounds;

/// <summary>Reports a scratched bonus round slot.</summary>
/// <param name="RoundId">The bonus game the slot belongs to.</param>
/// <param name="Round">Which round of the bonus game the slot is in.</param>
/// <param name="SlotIndex">Position of the scratched slot.</param>
/// <param name="RewardGlobalId">Global id of what the slot showed.</param>
/// <param name="RewardAmount">How many of that reward the slot showed.</param>
public sealed record ScratchBonusRoundRewardMessage(int RoundId, int Round, int SlotIndex, int RewardGlobalId, int RewardAmount) : CodecMessage<ScratchBonusRoundRewardMessage>, ICodecProvider<ScratchBonusRoundRewardMessage>
{
    /// <inheritdoc />
    public static ICodec<ScratchBonusRoundRewardMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.RoundId,
        Codecs.VarInt,
        static message => message.Round,
        Codecs.VarInt,
        static message => message.SlotIndex,
        Codecs.VarInt,
        static message => message.RewardGlobalId,
        Codecs.VarInt,
        static message => message.RewardAmount,
        static (roundId, round, slotIndex, rewardGlobalId, rewardAmount) => new ScratchBonusRoundRewardMessage(roundId, round, slotIndex, rewardGlobalId, rewardAmount)
    );
}
