using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.BonusRounds;

/// <summary>Reports a scratched bonus round slot.</summary>
/// <param name="RoundId">The round id.</param>
/// <param name="Round">The round.</param>
/// <param name="SlotIndex">The slot index.</param>
/// <param name="RewardGlobalId">The reward data id.</param>
/// <param name="RewardAmount">The reward amount.</param>
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
