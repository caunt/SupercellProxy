using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Claims a reward of an event task list.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="RewardIndex">The reward index.</param>
/// <param name="Premium">The premium.</param>
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
