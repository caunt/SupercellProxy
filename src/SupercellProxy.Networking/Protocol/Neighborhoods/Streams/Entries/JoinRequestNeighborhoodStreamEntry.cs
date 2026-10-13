using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 3: a player asked to join the neighborhood.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="Message">The request message.</param>
/// <param name="ResponderName">The member who answered the request.</param>
/// <param name="State">The request state.</param>
/// <param name="SourceId">The source id.</param>
public sealed record JoinRequestNeighborhoodStreamEntry(StreamEntryHeader Header, string? Message, string? ResponderName, int State, string? SourceId) : NeighborhoodStreamEntry(Header), ICodecProvider<JoinRequestNeighborhoodStreamEntry>
{
    internal const int Type = 3;

    /// <inheritdoc />
    public static new ICodec<JoinRequestNeighborhoodStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.OptionalString,
        static entry => entry.Message,
        Codecs.OptionalString,
        static entry => entry.ResponderName,
        Codecs.VarInt,
        static entry => entry.State,
        Codecs.OptionalString,
        static entry => entry.SourceId,
        static (header, message, responderName, state, sourceId) => new JoinRequestNeighborhoodStreamEntry(header, message, responderName, state, sourceId)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
