using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Delivers the neighborhood chat stream entries.</summary>
/// <param name="Entries">The entries, or <see langword="null"/> when native writes a negative count.</param>
public sealed record NeighborhoodStreamMessage(ReadOnlyMemory<NeighborhoodStreamEntry>? Entries) : CodecMessage<NeighborhoodStreamMessage>, ICodecProvider<NeighborhoodStreamMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodStreamMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(NeighborhoodStreamEntry.Codec),
        static message => message.Entries,
        static entries => new NeighborhoodStreamMessage(entries)
    );
}
