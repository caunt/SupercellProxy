using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Server command 355. Native execution passes its optional value to the shop-event manager.</para>
/// </summary>
internal sealed record ServerCommand355 : ServerCommand
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 355;

    /// <summary>
    /// Initializes a new <see cref="ServerCommand355"/> instance.
    /// </summary>
    public ServerCommand355(
        ShopEventCollection? shopEvents,
        int serverCommandId,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandId, executionPhaseCounter, debugData0, debugData1)
    {
        ShopEventCollection = shopEvents;
    }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">ShopEventCollection</c> value.
    /// </summary>
    public ShopEventCollection? ShopEventCollection { get; }

    internal static ServerCommand355 Decode(MessageStream stream, CommandEnvironment environment)
    {
        var shopEvents = stream.ReadBoolean() ? ShopEventCollection.Decode(stream) : null;
        var fields = DecodeServerCommand(stream, environment);

        return new ServerCommand355(
            shopEvents,
            fields.ServerCommandId,
            fields.CommandFields.ExecutionPhaseCounter,
            fields.CommandFields.DebugData0,
            fields.CommandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(ShopEventCollection is not null);
        ShopEventCollection?.Encode(stream);
        EncodeServerCommand(stream, environment);
    }
}
