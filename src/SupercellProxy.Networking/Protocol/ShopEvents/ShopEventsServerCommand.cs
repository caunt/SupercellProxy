using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>Server command 355. Native execution passes its optional value to the shop-event manager.</para>
/// </summary>
public sealed record ShopEventsServerCommand : CodecServerCommand<ShopEventsServerCommand>, ICommandCodecProvider<ShopEventsServerCommand>
{
    /// <summary>
    /// Wraps a delivery of live event definitions for the shop-event manager.
    /// </summary>
    /// <param name="shopEvents">The definitions; null when the server sent none.</param>
    public ShopEventsServerCommand(ShopEventCollection? shopEvents)
    {
        ShopEventCollection = shopEvents;
    }

    /// <inheritdoc />
    public static CommandCodec<ShopEventsServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Optional(ShopEventCollection.Codec),
            static command => command.ShopEventCollection,
            static shopEvents => new ShopEventsServerCommand(shopEvents)
        )
    );

    /// <summary>
    /// The live event definitions handed to the shop-event manager (offers, seasonal events and similar); null when none were sent.
    /// The server sends a large copy on every home load and visit.
    /// </summary>
    public ShopEventCollection? ShopEventCollection { get; }
}
