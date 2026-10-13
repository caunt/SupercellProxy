using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry types 5, 9 and 10: a request for help, a visitor help request and a truck order help request.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="EntryType">The native entry type: 5, 9, 10.</param>
/// <param name="Value">The value native reads first.</param>
/// <param name="HelperIds">The helpers.</param>
/// <param name="Completed">Whether the request was fulfilled.</param>
public sealed record HelpRequestStreamEntry(StreamEntryHeader Header, int EntryType, int Value, ReadOnlyMemory<LongId>? HelperIds, bool Completed) : NeighborhoodStreamEntry(Header)
{
    /// <inheritdoc/>
    public override int EntryType { get; } = EntryType;

    /// <summary>
    /// Gets the native entry types that carry a help request.
    /// </summary>
    internal static ReadOnlySpan<int> Types => [5, 9, 10];

    /// <summary>
    /// Gets the codec for the help-request entry type <paramref name="entryType"/>.
    /// </summary>
    internal static ICodec<HelpRequestStreamEntry> CodecFor(int entryType)
    {
        return Codecs.Composite(
            StreamEntryHeader.Codec,
            static entry => entry.Header,
            Codecs.VarInt,
            static entry => entry.Value,
            Codecs.NullableMemory(Codecs.LongId),
            static entry => entry.HelperIds,
            Codecs.Boolean,
            static entry => entry.Completed,
            (header, value, helperIds, completed) => new HelpRequestStreamEntry(header, entryType, value, helperIds, completed)
        );
    }
}
