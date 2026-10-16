using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Delivers the neighborhood notification stream entries to the game log manager.</summary>
/// <param name="Entries">The entries, or <see langword="null"/> when native writes a negative count.</param>
public sealed record NeighborhoodNotificationStreamMessage(ReadOnlyMemory<NeighborhoodStreamEntry>? Entries) : CodecMessage<NeighborhoodNotificationStreamMessage>, ICodecProvider<NeighborhoodNotificationStreamMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodNotificationStreamMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(NeighborhoodStreamEntry.Codec),
        static message => message.Entries,
        static entries => new NeighborhoodNotificationStreamMessage(entries)
    );
}
