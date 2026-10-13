using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 18: a chat emote.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="Emote">The emote.</param>
public sealed record EmoteStreamEntry(StreamEntryHeader Header, int Emote) : NeighborhoodStreamEntry(Header), ICodecProvider<EmoteStreamEntry>
{
    internal const int Type = 18;

    /// <inheritdoc />
    public static new ICodec<EmoteStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.Int32,
        static entry => entry.Emote,
        static (header, emote) => new EmoteStreamEntry(header, emote)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
