using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>Server command 355. Native execution passes its optional value to the shop-event manager.</para>
/// </summary>
public sealed record ShopEventsServerCommand : CodecServerCommand<ShopEventsServerCommand>, ICommandCodecProvider<ShopEventsServerCommand>
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 355;

    /// <summary>
    /// Initializes a new <see cref="ShopEventsServerCommand"/> instance.
    /// </summary>
    public ShopEventsServerCommand(ShopEventCollection? shopEvents)
    {
        ShopEventCollection = shopEvents;
    }

    /// <inheritdoc />
    public static CommandCodec<ShopEventsServerCommand> Codec { get; } = Codecs.Command(
        type: 355,
        Codecs.Composite(
            Codecs.Optional(ShopEventCollection.Codec),
            static command => command.ShopEventCollection,
            static (shopEvents) => new ShopEventsServerCommand(shopEvents)
        )
    );

    /// <summary>
    /// Gets the <c language="csharp">ShopEventCollection</c> value.
    /// </summary>
    public ShopEventCollection? ShopEventCollection { get; }
}
