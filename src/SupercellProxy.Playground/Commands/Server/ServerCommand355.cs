using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Server command 355. Native execution passes its optional value to the shop-event manager.</para>
/// </summary>
public sealed record ServerCommand355 : ServerCommand
{
    /// <summary>
    /// Defines the <c>CommandType</c> value.
    /// </summary>
    public const int CommandType = 355;

    /// <summary>
    /// Initializes a new <see cref="ServerCommand355"/> instance.
    /// </summary>
    public ServerCommand355(
        ShopEvents? shopEvents,
        int serverCommandId,
        int executeSubTick = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandId, executeSubTick, debugData0, debugData1)
    {
        ShopEvents = shopEvents;
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c>ShopEvents</c> value.
    /// </summary>
    public ShopEvents? ShopEvents { get; }

    internal static ServerCommand355 Decode(MessageStream stream, CommandEnvironment environment)
    {
        var shopEvents = stream.ReadBoolean() ? ShopEvents.Decode(stream) : null;
        var fields = DecodeServerCommand(stream, environment);

        return new ServerCommand355(
            shopEvents,
            fields.ServerCommandId,
            fields.CommandFields.ExecuteSubTick,
            fields.CommandFields.DebugData0,
            fields.CommandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(ShopEvents is not null);
        ShopEvents?.Encode(stream);
        EncodeServerCommand(stream, environment);
    }
}
