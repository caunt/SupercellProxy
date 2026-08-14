using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Base wire representation shared by commands issued by the server.
/// </summary>
public abstract record LogicServerCommand : LogicCommand
{
    protected LogicServerCommand(int serverCommandId, int executeSubTick, LogicCommandData? debugData0, LogicCommandData? debugData1)
        : base(executeSubTick, debugData0, debugData1)
    {
        if (serverCommandId is -1)
            throw new InvalidDataException("Server command ID cannot be -1.");

        ServerCommandId = serverCommandId;
    }

    public int ServerCommandId { get; }

    protected void EncodeLogicServerCommand(SupercellStream stream, LogicEnvironment environment)
    {
        stream.WriteVarInt(ServerCommandId);
        EncodeLogicCommand(stream, environment);
    }

    protected static LogicServerCommandFields DecodeLogicServerCommand(SupercellStream stream, LogicEnvironment environment)
    {
        var serverCommandId = stream.ReadVarInt();
        var logicCommandFields = DecodeLogicCommand(stream, environment);

        if (serverCommandId is -1)
            throw new InvalidDataException("Server command ID cannot be -1.");

        return new LogicServerCommandFields(serverCommandId, logicCommandFields);
    }

    protected readonly record struct LogicServerCommandFields(int ServerCommandId, LogicCommandFields LogicCommandFields);
}
