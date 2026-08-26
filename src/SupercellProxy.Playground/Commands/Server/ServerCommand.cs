using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Base wire representation shared by commands issued by the server.</para>
/// </summary>
public abstract record ServerCommand : Command
{
    /// <summary>
    /// Initializes a new <see cref="ServerCommand"/> instance.
    /// </summary>
    protected ServerCommand(
        int serverCommandId,
        int executeSubTick,
        CommandData? debugData0,
        CommandData? debugData1
    )
        : base(executeSubTick, debugData0, debugData1)
    {
        if (serverCommandId is -1)
            throw new InvalidDataException("Server command ID cannot be -1.");

        ServerCommandId = serverCommandId;
    }

    /// <summary>
    /// Gets the <c>ServerCommandId</c> value.
    /// </summary>
    public int ServerCommandId { get; }

    /// <summary>
    /// Executes the <c>EncodeServerCommand</c> operation.
    /// </summary>
    protected void EncodeServerCommand(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ServerCommandId);
        EncodeCommand(stream, environment);
    }

    /// <summary>
    /// <para>Decodes the common server-command header.</para>
    /// </summary>
    protected static (
        int ServerCommandId,
        (int ExecuteSubTick, CommandData? DebugData0, CommandData? DebugData1) CommandFields
    ) DecodeServerCommand(MessageStream stream, CommandEnvironment environment)
    {
        var serverCommandId = stream.ReadVarInt();
        var logicCommandFields = DecodeCommand(stream, environment);

        if (serverCommandId is -1)
            throw new InvalidDataException("Server command ID cannot be -1.");

        return (serverCommandId, logicCommandFields);
    }
}
