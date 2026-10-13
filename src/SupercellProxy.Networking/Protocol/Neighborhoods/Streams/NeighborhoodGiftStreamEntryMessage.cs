using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Carries one neighborhood gift entry without its entry type; the client only resets its pending gift state.</summary>
/// <param name="NeighborhoodId">The id native reads before the entry.</param>
/// <param name="Entry">The gift entry.</param>
public sealed record NeighborhoodGiftStreamEntryMessage(LongId NeighborhoodId, NeighborhoodGiftStreamEntry Entry) : CodecMessage<NeighborhoodGiftStreamEntryMessage>, ICodecProvider<NeighborhoodGiftStreamEntryMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodGiftStreamEntryMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.NeighborhoodId,
        NeighborhoodGiftStreamEntry.Codec,
        static message => message.Entry,
        static (neighborhoodId, entry) => new NeighborhoodGiftStreamEntryMessage(neighborhoodId, entry)
    );
}
