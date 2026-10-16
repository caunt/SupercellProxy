using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Claims a reward of an event task list.</summary>
/// <param name="EventId">Task event the reward belongs to.</param>
/// <param name="RewardIndex">Position of the reward on the event's reward track.</param>
/// <param name="Premium">Selects the premium reward at that position instead of the regular one.</param>
public sealed record ClaimEventTaskRewardCommand(int EventId, int RewardIndex, bool Premium) : CodecCommand<ClaimEventTaskRewardCommand>, ICommandCodecProvider<ClaimEventTaskRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimEventTaskRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.RewardIndex,
            Codecs.Boolean,
            static command => command.Premium,
            static (eventId, rewardIndex, premium) => new ClaimEventTaskRewardCommand(eventId, rewardIndex, premium)
        )
    );
}
