using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Votes for one member of a received decoration-canvas candidate pair.</summary>
public sealed record SubmitDecorationVoteCommand(LongId? CandidateId, int EventId, int EventVariantId) : CodecCommand<SubmitDecorationVoteCommand>, ICommandCodecProvider<SubmitDecorationVoteCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SubmitDecorationVoteCommand> Codec { get; } = Codecs.Command(
        type: 687,
        Codecs.Composite(
            Codecs.OptionalLongId,
            static command => command.CandidateId,
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.EventVariantId,
            static (candidateId, eventId, eventVariantId) => new SubmitDecorationVoteCommand(candidateId, eventId, eventVariantId)
        ),
        CommandLayout.MetadataFirst
    );
}
