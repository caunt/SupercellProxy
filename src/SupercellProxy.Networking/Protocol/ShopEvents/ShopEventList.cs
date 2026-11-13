using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>Shop-event list carried by server command 355.</para>
/// </summary>
public sealed record ShopEventList : ICodecProvider<ShopEventList>
{
    /// <summary>
    /// Largest number of events a list may carry; the decoder rejects more as corrupt data.
    /// </summary>
    public const int MaximumEventCount = 1024;

    /// <inheritdoc />
    public static ICodec<ShopEventList> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static list => list.Unknown0,
        Codecs.Memory(ShopEvent.Codec, CollectionCount.AtMost(MaximumEventCount)),
        static list => list.Events,
        static (unknown0, events) => new ShopEventList
        {
            Unknown0 = unknown0,
            Events = events,
        });

    /// <summary>
    /// Gets or sets the live event definitions the server delivers: shop offers, seasonal events and similar, at most <see cref="MaximumEventCount"/>.
    /// </summary>
    /// <value>The events in wire order, sent after <see cref="Unknown0"/> behind a var-int count; empty by default.</value>
    public ReadOnlyMemory<ShopEvent> Events { get; init; }

    /// <summary>
    /// Gets or sets the number sent before the events; its meaning is not identified (-1 when the list is built locally).
    /// </summary>
    /// <value>The leading var-int of the list, read and written unchanged; nothing in the code interprets it.</value>
    public int Unknown0 { get; init; } = -1;
}
