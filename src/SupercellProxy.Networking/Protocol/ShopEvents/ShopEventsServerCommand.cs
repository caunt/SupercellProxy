using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>Server command 355. Native execution passes its optional value to the shop-event manager.</para>
/// </summary>
public sealed record ShopEventsServerCommand : ServerCommand
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

    /// <summary>
    /// Gets the <c language="csharp">ShopEventCollection</c> value.
    /// </summary>
    public ShopEventCollection? ShopEventCollection { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ShopEventsServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ShopEventCollection? shopEvents = stream.ReadBoolean() ? ShopEventCollection.Decode(stream) : null;


        return new ShopEventsServerCommand(shopEvents);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(ShopEventCollection is not null);
        ShopEventCollection?.Encode(stream);
    }
}
