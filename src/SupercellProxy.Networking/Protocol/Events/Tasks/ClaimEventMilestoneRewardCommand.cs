using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Claims an event milestone reward.</summary>
/// <param name="EventId">Event whose points track the milestone is on.</param>
/// <param name="MilestoneIndex">Position of the reached milestone on that track, starting at 0.</param>
public sealed record ClaimEventMilestoneRewardCommand(int EventId, int MilestoneIndex) : CodecCommand<ClaimEventMilestoneRewardCommand>, ICommandCodecProvider<ClaimEventMilestoneRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimEventMilestoneRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.MilestoneIndex,
            static (eventId, milestoneIndex) => new ClaimEventMilestoneRewardCommand(eventId, milestoneIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
