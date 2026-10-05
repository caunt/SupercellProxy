using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>The shared scheduling and diagnostic fields surrounding a command's own fields.</summary>
internal sealed record CommandMetadata(int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1, int? ServerCommandId)
{
    internal static CommandMetadata Decode(MessageStream stream, CommandEnvironment environment, bool serverCommand)
    {
        int? serverId = serverCommand ? stream.ReadVarInt() : null;

        int phase = stream.ReadVarInt();
        CommandData? first = null;
        CommandData? second = null;

        if (environment is not CommandEnvironment.Production)
        {
            first = stream.ReadBoolean() ? CommandData.Decode(stream) : null;
            second = stream.ReadBoolean() ? CommandData.Decode(stream) : null;
        }

        return serverId is -1
            ? throw new InvalidDataException(message: "Server command ID cannot be -1.")
            : new CommandMetadata(phase, first, second, serverId);
    }

    internal static void Encode(MessageStream stream, Command command, CommandEnvironment environment)
    {
        if (command is ServerCommand server)
        {
            if (server.ServerCommandId is -1)
                throw new InvalidDataException(message: "Server command ID cannot be -1.");

            stream.WriteVarInt(server.ServerCommandId);
        }

        stream.WriteVarInt(command.ExecutionPhaseCounter);

        if (environment is CommandEnvironment.Production)
            return;

        stream.WriteBoolean(command.DebugData0 is not null);
        command.DebugData0?.Encode(stream);
        stream.WriteBoolean(command.DebugData1 is not null);
        command.DebugData1?.Encode(stream);
    }

    internal Command Apply(Command command)
    {
        Command result = command with
        {
            ExecutionPhaseCounter = ExecutionPhaseCounter,
            DebugData0 = DebugData0,
            DebugData1 = DebugData1,
        };

        return result is ServerCommand server
            ? server with
            {
                ServerCommandId = ServerCommandId
                ?? throw new InvalidDataException(message: "Server command metadata is missing its id.")
            }
            : result;
    }
}
