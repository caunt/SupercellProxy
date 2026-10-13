using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Claims an event milestone reward.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="MilestoneIndex">The milestone index.</param>
public sealed record ClaimEventMilestoneRewardCommand(int EventId, int MilestoneIndex) : CodecCommand<ClaimEventMilestoneRewardCommand>, ICommandCodecProvider<ClaimEventMilestoneRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimEventMilestoneRewardCommand> Codec { get; } = Codecs.Command(
        type: 367,
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
