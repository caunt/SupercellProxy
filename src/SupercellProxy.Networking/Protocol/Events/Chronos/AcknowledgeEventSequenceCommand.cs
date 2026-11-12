using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Acknowledges the presentation sequence following an event reward.</summary>
/// <param name="StepIndex">The zero-based chain-offer step whose claimed reward sequence was seen, or -1 to mark an event-board event (type 51) itself as seen.</param>
/// <param name="EventId">The id of the live event the sequence belongs to.</param>
public sealed record AcknowledgeEventSequenceCommand(int StepIndex, int EventId) : CodecCommand<AcknowledgeEventSequenceCommand>, ICommandCodecProvider<AcknowledgeEventSequenceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AcknowledgeEventSequenceCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.StepIndex,
            Codecs.VarInt,
            static command => command.EventId,
            static (stepIndex, eventId) => new AcknowledgeEventSequenceCommand(stepIndex, eventId)),
        CommandLayout.MetadataFirst);
}
