using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 19: two values the chat does not show.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="Value0">The first value.</param>
/// <param name="Value1">The second value.</param>
public sealed record ValuesStreamEntry(StreamEntryHeader Header, int Value0, int Value1) : NeighborhoodStreamEntry(Header), ICodecProvider<ValuesStreamEntry>
{
    internal const int Type = 19;

    /// <inheritdoc />
    public static new ICodec<ValuesStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.VarInt,
        static entry => entry.Value0,
        Codecs.VarInt,
        static entry => entry.Value1,
        static (header, value0, value1) => new ValuesStreamEntry(header, value0, value1)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
