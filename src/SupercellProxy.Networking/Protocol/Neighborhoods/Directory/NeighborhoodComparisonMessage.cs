using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>Lists neighborhood comparison entries.</summary>
/// <param name="Value">Number sent before the entries; its meaning is not identified.</param>
/// <param name="Entries">The compared players with their numbers.</param>
public sealed record NeighborhoodComparisonMessage(int Value, ReadOnlyMemory<NeighborhoodComparisonEntry> Entries) : CodecMessage<NeighborhoodComparisonMessage>, ICodecProvider<NeighborhoodComparisonMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodComparisonMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Value,
        Codecs.Memory(NeighborhoodComparisonEntry.Codec),
        static message => message.Entries,
        static (@value, entries) => new NeighborhoodComparisonMessage(value, entries)
    );
}
