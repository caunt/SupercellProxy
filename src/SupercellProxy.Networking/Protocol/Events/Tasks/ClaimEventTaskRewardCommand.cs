using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Claims a reward of a task event.</summary>
/// <param name="EventId">Task event the reward belongs to.</param>
/// <param name="RewardIndex">Position of the reward in its list.</param>
/// <param name="EventCompletedReward">Selects the rewards granted for completing the points track instead of the points track itself.</param>
public sealed record ClaimEventTaskRewardCommand(int EventId, int RewardIndex, bool EventCompletedReward) : CodecCommand<ClaimEventTaskRewardCommand>, ICommandCodecProvider<ClaimEventTaskRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimEventTaskRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.RewardIndex,
            Codecs.Boolean,
            static command => command.EventCompletedReward,
            static (eventId, rewardIndex, eventCompletedReward) => new ClaimEventTaskRewardCommand(eventId, rewardIndex, eventCompletedReward)
        )
    );
}
