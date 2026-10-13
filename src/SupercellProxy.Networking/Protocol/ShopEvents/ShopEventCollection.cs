using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>Shop-event collection carried by server command 355.</para>
/// </summary>
public sealed record ShopEventCollection : ICodecProvider<ShopEventCollection>
{
    /// <summary>
    /// Defines the <c language="csharp">MaxEventCount</c> value.
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
    /// Gets or sets the <c language="csharp">Events</c> value.
    /// </summary>
    public ReadOnlyMemory<ShopEvent> Events { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; } = -1;
}
