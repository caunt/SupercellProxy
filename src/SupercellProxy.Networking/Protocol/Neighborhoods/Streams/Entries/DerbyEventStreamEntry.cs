using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 6: a derby event such as the derby ending.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="EventType">The derby event type.</param>
/// <param name="AvatarId">The avatar the event is about.</param>
/// <param name="Value0">The first value.</param>
/// <param name="Value1">The second value.</param>
public sealed record DerbyEventStreamEntry(StreamEntryHeader Header, int EventType, LongId AvatarId, int Value0, int Value1) : NeighborhoodStreamEntry(Header), ICodecProvider<DerbyEventStreamEntry>
{
    internal const int Type = 6;

    /// <inheritdoc />
    public static new ICodec<DerbyEventStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.VarInt,
        static entry => entry.EventType,
        Codecs.LongId,
        static entry => entry.AvatarId,
        Codecs.VarInt,
        static entry => entry.Value0,
        Codecs.VarInt,
        static entry => entry.Value1,
        static (header, eventType, avatarId, value0, value1) => new DerbyEventStreamEntry(header, eventType, avatarId, value0, value1)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
