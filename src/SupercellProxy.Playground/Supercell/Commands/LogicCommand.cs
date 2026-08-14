using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Base wire representation shared by Hay Day logic commands.
/// </summary>
public abstract record LogicCommand
{
    protected LogicCommand(int executeSubTick, LogicCommandData? debugData0, LogicCommandData? debugData1)
    {
        ExecuteSubTick = executeSubTick;
        DebugData0 = debugData0;
        DebugData1 = debugData1;
    }

    public abstract int Type { get; }
    public int ExecuteSubTick { get; }
    public LogicCommandData? DebugData0 { get; }
    public LogicCommandData? DebugData1 { get; }

    internal abstract void EncodeBody(SupercellStream stream, LogicEnvironment environment);

    protected void EncodeLogicCommand(SupercellStream stream, LogicEnvironment environment)
    {
        stream.WriteVarInt(ExecuteSubTick);

        if (environment is LogicEnvironment.Production)
            return;

        stream.WriteBoolean(DebugData0 is not null);
        DebugData0?.Encode(stream);
        stream.WriteBoolean(DebugData1 is not null);
        DebugData1?.Encode(stream);
    }

    protected static LogicCommandFields DecodeLogicCommand(SupercellStream stream, LogicEnvironment environment)
    {
        var executeSubTick = stream.ReadVarInt();

        if (environment is LogicEnvironment.Production)
            return new LogicCommandFields(executeSubTick, null, null);

        var debugData0 = stream.ReadBoolean() ? LogicCommandData.Decode(stream) : null;
        var debugData1 = stream.ReadBoolean() ? LogicCommandData.Decode(stream) : null;

        return new LogicCommandFields(executeSubTick, debugData0, debugData1);
    }

    protected readonly record struct LogicCommandFields(int ExecuteSubTick, LogicCommandData? DebugData0, LogicCommandData? DebugData1);
}
