using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Marks a neighborhood notification stream entry as removed in the game log manager.</summary>
/// <param name="EntryId">The entry id.</param>
public sealed record NeighborhoodNotificationStreamEntryRemovedMessage(LongId EntryId) : CodecMessage<NeighborhoodNotificationStreamEntryRemovedMessage>, ICodecProvider<NeighborhoodNotificationStreamEntryRemovedMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodNotificationStreamEntryRemovedMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.EntryId,
        static (entryId) => new NeighborhoodNotificationStreamEntryRemovedMessage(entryId)
    );
}
