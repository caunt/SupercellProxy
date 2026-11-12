using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents a native <c language="csharp">LogicLiveEvents</c>: the scheduled live events.
/// </summary>
/// <param name="Unknown0">The VarInt written before the event list; its meaning is not yet identified, and it is round-tripped unchanged.</param>
/// <param name="Entries">The scheduled live events in wire order, each with its JSON definition and its start, end and visibility times; empty when no event is scheduled.</param>
public sealed record LiveEvents(int Unknown0, LiveEvent[] Entries) : ICodecProvider<LiveEvents>
{
    /// <inheritdoc />
    public static ICodec<LiveEvents> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static events => events.Unknown0,
        Codecs.Array(LiveEvent.Codec, CollectionCount.Bounded),
        static events => events.Entries,
        static (unknown0, entries) => new LiveEvents(unknown0, entries));
}
