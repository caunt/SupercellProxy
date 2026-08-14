using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Server command 355. Native execution passes its optional value to the shop-event manager.
/// </summary>
public sealed record LogicServerCommand355 : LogicServerCommand
{
    public const int CommandType = 355;

    public LogicServerCommand355(LogicShopEvents? shopEvents, int serverCommandId, int executeSubTick = -1, LogicCommandData? debugData0 = null, LogicCommandData? debugData1 = null)
        : base(serverCommandId, executeSubTick, debugData0, debugData1)
    {
        ShopEvents = shopEvents;
    }

    public override int Type => CommandType;
    public LogicShopEvents? ShopEvents { get; }

    internal static LogicServerCommand355 Decode(SupercellStream stream, LogicEnvironment environment)
    {
        var shopEvents = stream.ReadBoolean() ? LogicShopEvents.Decode(stream) : null;
        var fields = DecodeLogicServerCommand(stream, environment);

        return new LogicServerCommand355(
            shopEvents,
            fields.ServerCommandId,
            fields.LogicCommandFields.ExecuteSubTick,
            fields.LogicCommandFields.DebugData0,
            fields.LogicCommandFields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        stream.WriteBoolean(ShopEvents is not null);
        ShopEvents?.Encode(stream);
        EncodeLogicServerCommand(stream, environment);
    }
}
