using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>One neighborhood stream entry; native picks the entry class from the type that precedes it.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
public abstract record NeighborhoodStreamEntry(StreamEntryHeader Header) : ICodecProvider<NeighborhoodStreamEntry>
{
    /// <summary>
    /// Gets the wire codec: the entry type selects the entry record.
    /// </summary>
    public static ICodec<NeighborhoodStreamEntry> Codec { get; } = Codecs.Dispatch(
        Codecs.VarInt,
        static entry => entry.EntryType,
        [
            Case(ChatStreamEntry.Type, ChatStreamEntry.Codec),
            Case(JoinRequestNeighborhoodStreamEntry.Type, JoinRequestNeighborhoodStreamEntry.Codec),
            Case(NeighborhoodEventStreamEntry.Type, NeighborhoodEventStreamEntry.Codec),
            Case(DerbyEventStreamEntry.Type, DerbyEventStreamEntry.Codec),
            Case(ItemRequestStreamEntry.Type, ItemRequestStreamEntry.Codec),
            Case(NeighborhoodNotificationStreamEntry.Type, NeighborhoodNotificationStreamEntry.Codec),
            Case(NeighborhoodGiftStreamEntry.Type, NeighborhoodGiftStreamEntry.Codec),
            Case(EmoteStreamEntry.Type, EmoteStreamEntry.Codec),
            Case(ValuesStreamEntry.Type, ValuesStreamEntry.Codec),
            .. HelpRequestCases(),
        ]
    );

    /// <summary>Gets the native entry type written before the entry.</summary>
    public abstract int EntryType { get; }

    private static KeyValuePair<int, ICodec<NeighborhoodStreamEntry>> Case<TEntry>(int entryType, ICodec<TEntry> codec) where TEntry : NeighborhoodStreamEntry
    {
        return new KeyValuePair<int, ICodec<NeighborhoodStreamEntry>>(entryType, Codecs.Subtype<NeighborhoodStreamEntry, TEntry>(codec));
    }

    private static KeyValuePair<int, ICodec<NeighborhoodStreamEntry>>[] HelpRequestCases()
    {
        ReadOnlySpan<int> types = HelpRequestStreamEntry.Types;
        KeyValuePair<int, ICodec<NeighborhoodStreamEntry>>[] cases = new KeyValuePair<int, ICodec<NeighborhoodStreamEntry>>[types.Length];

        for (int index = 0; index < cases.Length; index++)
            cases[index] = Case(types[index], HelpRequestStreamEntry.CodecFor(types[index]));

        return cases;
    }
}
