using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers;

/// <summary>Acknowledges the introduction for an available chapter of a chain offer.</summary>
/// <param name="ChapterIndex">The zero-based chapter index within the offer's chapter list, written as a VarInt first.</param>
/// <param name="EventId">The live event id of the chain offer, written as a VarInt after the chapter index.</param>
public sealed record AcknowledgeChainOfferChapterIntroCommand(int ChapterIndex, int EventId) : CodecCommand<AcknowledgeChainOfferChapterIntroCommand>, ICommandCodecProvider<AcknowledgeChainOfferChapterIntroCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AcknowledgeChainOfferChapterIntroCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.ChapterIndex,
            Codecs.VarInt,
            static command => command.EventId,
            static (chapterIndex, eventId) => new AcknowledgeChainOfferChapterIntroCommand(chapterIndex, eventId)),
        CommandLayout.MetadataFirst);
}
