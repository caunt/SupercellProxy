using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Base wire representation shared by Hay Day logic commands.</para>
/// </summary>
internal abstract record Command
{
    /// <summary>
    /// Initializes a new <see cref="Command"/> instance.
    /// </summary>
    protected Command(int executionPhaseCounter, CommandData? debugData0, CommandData? debugData1)
    {
        ExecutionPhaseCounter = executionPhaseCounter;
        DebugData0 = debugData0;
        DebugData1 = debugData1;
    }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public abstract int Type { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExecutionPhaseCounter</c> value.
    /// </summary>
    public int ExecutionPhaseCounter { get; private set; }

    internal void SetExecutionPhaseCounter(int executionPhaseCounter)
    {
        ExecutionPhaseCounter = executionPhaseCounter;
    }

    /// <summary>
    /// Gets the <c language="csharp">DebugData0</c> value.
    /// </summary>
    public CommandData? DebugData0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">DebugData1</c> value.
    /// </summary>
    public CommandData? DebugData1 { get; }

    internal abstract void EncodeBody(MessageStream stream, CommandEnvironment environment);

    /// <summary>
    /// Executes the <c language="csharp">EncodeCommand</c> operation.
    /// </summary>
    protected void EncodeCommand(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ExecutionPhaseCounter);

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
        int ExecutionPhaseCounter,
        CommandData? DebugData0,
        CommandData? DebugData1
    ) DecodeCommand(MessageStream stream, CommandEnvironment environment)
    {
        var executionPhaseCounter = stream.ReadVarInt();

        if (environment is CommandEnvironment.Production)
            return (executionPhaseCounter, null, null);

        var debugData0 = stream.ReadBoolean() ? CommandData.Decode(stream) : null;
        var debugData1 = stream.ReadBoolean() ? CommandData.Decode(stream) : null;

        return (executionPhaseCounter, debugData0, debugData1);
    }
}
