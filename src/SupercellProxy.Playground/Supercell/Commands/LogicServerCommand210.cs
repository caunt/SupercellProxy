using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Server command 210. The native class and field names are not present in the stripped client.
/// </summary>
public sealed record LogicServerCommand210 : LogicServerCommand
{
    public const int CommandType = 210;

    public LogicServerCommand210(int unknown0, LogicLong unknown1, int serverCommandId, int executeSubTick = -1, LogicCommandData? debugData0 = null, LogicCommandData? debugData1 = null)
        : base(serverCommandId, executeSubTick, debugData0, debugData1)
    {
        Unknown0 = unknown0;
        Unknown1 = unknown1;
    }

    public override int Type => CommandType;
    public int Unknown0 { get; }
    public LogicLong Unknown1 { get; }

    internal static LogicServerCommand210 Decode(SupercellStream stream, LogicEnvironment environment)
    {
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadLogicLong();
        var fields = DecodeLogicServerCommand(stream, environment);

        return new LogicServerCommand210(
            unknown0,
            unknown1,
            fields.ServerCommandId,
            fields.LogicCommandFields.ExecuteSubTick,
            fields.LogicCommandFields.DebugData0,
            fields.LogicCommandFields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteLogicLong(Unknown1);
        EncodeLogicServerCommand(stream, environment);
    }
}
