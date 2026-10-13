using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 2: a chat message.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="Message">The message.</param>
public sealed record ChatStreamEntry(StreamEntryHeader Header, string? Message) : NeighborhoodStreamEntry(Header), ICodecProvider<ChatStreamEntry>
{
    internal const int Type = 2;

    /// <inheritdoc />
    public static new ICodec<ChatStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.OptionalString,
        static entry => entry.Message,
        static (header, message) => new ChatStreamEntry(header, message)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
