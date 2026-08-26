using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Base wire representation shared by Hay Day logic commands.</para>
/// </summary>
public abstract record Command
{
    /// <summary>
    /// Initializes a new <see cref="Command"/> instance.
    /// </summary>
    protected Command(int executeSubTick, CommandData? debugData0, CommandData? debugData1)
    {
        ExecuteSubTick = executeSubTick;
        DebugData0 = debugData0;
        DebugData1 = debugData1;
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public abstract int Type { get; }

    /// <summary>
    /// Gets the <c>ExecuteSubTick</c> value.
    /// </summary>
    public int ExecuteSubTick { get; }

    /// <summary>
    /// Gets the <c>DebugData0</c> value.
    /// </summary>
    public CommandData? DebugData0 { get; }

    /// <summary>
    /// Gets the <c>DebugData1</c> value.
    /// </summary>
    public CommandData? DebugData1 { get; }

    internal abstract void EncodeBody(MessageStream stream, CommandEnvironment environment);

    /// <summary>
    /// Executes the <c>EncodeCommand</c> operation.
    /// </summary>
    protected void EncodeCommand(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ExecuteSubTick);

        if (environment is CommandEnvironment.Production)
            return;

        stream.WriteBoolean(DebugData0 is not null);
        DebugData0?.Encode(stream);
        stream.WriteBoolean(DebugData1 is not null);
        DebugData1?.Encode(stream);
    }

    /// <summary>
    /// <para>Decodes the common logic-command header.</para>
    /// </summary>
    protected static (
        int ExecuteSubTick,
        CommandData? DebugData0,
        CommandData? DebugData1
    ) DecodeCommand(MessageStream stream, CommandEnvironment environment)
    {
        var executeSubTick = stream.ReadVarInt();

        if (environment is CommandEnvironment.Production)
            return (executeSubTick, null, null);

        var debugData0 = stream.ReadBoolean() ? CommandData.Decode(stream) : null;
        var debugData1 = stream.ReadBoolean() ? CommandData.Decode(stream) : null;

        return (executeSubTick, debugData0, debugData1);
    }
}
