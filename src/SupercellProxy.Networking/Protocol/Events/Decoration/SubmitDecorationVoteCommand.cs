using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Votes for one member of a received decoration-canvas candidate pair.</summary>
/// <param name="CandidateId">The avatar id of the chosen candidate, one of the two in the current pair, written as an optional long id.</param>
/// <param name="EventId">The id of the decoration event being voted in, written as a VarInt after the candidate id.</param>
/// <param name="EventVariantId">The variant id of that decoration event, written as a VarInt after the event id.</param>
public sealed record SubmitDecorationVoteCommand(LongId? CandidateId, int EventId, int EventVariantId) : CodecCommand<SubmitDecorationVoteCommand>, ICommandCodecProvider<SubmitDecorationVoteCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SubmitDecorationVoteCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.OptionalLongId,
            static command => command.CandidateId,
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.EventVariantId,
            static (candidateId, eventId, eventVariantId) => new SubmitDecorationVoteCommand(candidateId, eventId, eventVariantId)),
        CommandLayout.MetadataFirst);
}
