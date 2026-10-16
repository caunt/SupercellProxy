using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Marks neighborhood stream entries as seen.</summary>
/// <param name="EntryIds">Stream entries the player has now seen; null when none are listed.</param>
public sealed record MarkNeighborhoodStreamEntriesSeenMessage(ReadOnlyMemory<StreamEntryId>? EntryIds) : CodecMessage<MarkNeighborhoodStreamEntriesSeenMessage>, ICodecProvider<MarkNeighborhoodStreamEntriesSeenMessage>
{
    /// <inheritdoc />
    public static ICodec<MarkNeighborhoodStreamEntriesSeenMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(StreamEntryId.Codec),
        static message => message.EntryIds,
        static entryIds => new MarkNeighborhoodStreamEntriesSeenMessage(entryIds)
    );
}
