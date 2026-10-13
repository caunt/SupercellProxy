using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 17: a neighborhood gift members can claim.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="Value0">The first value.</param>
/// <param name="Value1">The second value.</param>
/// <param name="Value2">The third value.</param>
/// <param name="Value3">The fourth value.</param>
/// <param name="ClaimedAvatarIds">The avatars that claimed the gift; native writes their count as a 32-bit integer.</param>
public sealed record NeighborhoodGiftStreamEntry(StreamEntryHeader Header, int Value0, int Value1, int Value2, int Value3, ReadOnlyMemory<LongId>? ClaimedAvatarIds) : NeighborhoodStreamEntry(Header), ICodecProvider<NeighborhoodGiftStreamEntry>
{
    internal const int Type = 17;

    /// <inheritdoc />
    public static new ICodec<NeighborhoodGiftStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.VarInt,
        static entry => entry.Value0,
        Codecs.VarInt,
        static entry => entry.Value1,
        Codecs.VarInt,
        static entry => entry.Value2,
        Codecs.VarInt,
        static entry => entry.Value3,
        Codecs.NullableMemory(Codecs.LongId, CollectionCount.Int32),
        static entry => entry.ClaimedAvatarIds,
        static (header, value0, value1, value2, value3, claimedAvatarIds) => new NeighborhoodGiftStreamEntry(header, value0, value1, value2, value3, claimedAvatarIds)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
