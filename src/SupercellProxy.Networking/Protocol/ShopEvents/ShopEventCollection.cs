using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>Shop-event collection carried by server command 355.</para>
/// </summary>
public sealed record ShopEventCollection : ICodecProvider<ShopEventCollection>
{
    /// <summary>
    /// Largest number of events a collection may carry; the decoder rejects more as corrupt data.
    /// </summary>
    public const int MaximumEventCount = 1024;

    /// <inheritdoc />
    public static ICodec<ShopEventCollection> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static collection => collection.Unknown0,
        Codecs.Memory(ShopEvent.Codec, CollectionCount.AtMost(MaximumEventCount)),
        static collection => collection.Events,
        static (unknown0, events) => new ShopEventCollection
        {
            Unknown0 = unknown0,
            Events = events,
        }
    );

    /// <summary>
    /// Live event definitions the server delivers: shop offers, seasonal events and similar, at most <see cref="MaximumEventCount"/>.
    /// </summary>
    public ReadOnlyMemory<ShopEvent> Events { get; init; }

    /// <summary>
    /// Number sent before the events; its meaning is not identified (-1 when the collection is built locally).
    /// </summary>
    public int Unknown0 { get; init; } = -1;
}
