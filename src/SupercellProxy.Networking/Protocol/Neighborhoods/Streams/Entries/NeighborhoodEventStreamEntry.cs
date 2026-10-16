using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 4: a membership event; event types 1 to 9 are kicked, accepted, joined, left, promoted, demoted, reported, a ninth kind and public message reported.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="EventType">What happened, numbered 1 to 9 as listed in the summary.</param>
/// <param name="TargetAvatarId">The avatar the event is about.</param>
/// <param name="TargetName">The name of the avatar the event is about.</param>
/// <param name="Value">The value native reads after the name.</param>
public sealed record NeighborhoodEventStreamEntry(StreamEntryHeader Header, int EventType, LongId TargetAvatarId, string? TargetName, long Value) : NeighborhoodStreamEntry(Header), ICodecProvider<NeighborhoodEventStreamEntry>
{
    internal const int Type = 4;

    /// <inheritdoc />
    public static new ICodec<NeighborhoodEventStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.VarInt,
        static entry => entry.EventType,
        Codecs.LongId,
        static entry => entry.TargetAvatarId,
        Codecs.OptionalString,
        static entry => entry.TargetName,
        Codecs.VarLong,
        static entry => entry.Value,
        static (header, eventType, targetAvatarId, targetName, value) => new NeighborhoodEventStreamEntry(header, eventType, targetAvatarId, targetName, value)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
