using System.Runtime.InteropServices;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>The shared scheduling and diagnostic fields surrounding a command's own fields.</summary>
[StructLayout(LayoutKind.Auto)]
internal readonly record struct CommandMetadata(int? ServerCommandId, int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1)
{
    private const string MissingServerCommandIdMessage = "Server command metadata is missing its id.";

    /// <summary>
    /// Gets the metadata codec of serverbound commands: the execution phase counter and, outside production, two optional debug data blocks.
    /// </summary>
    internal static ICodec<CommandMetadata> ClientCodec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static metadata => metadata.ExecutionPhaseCounter,
        DebugDataCodec(),
        static metadata => metadata.DebugData0,
        DebugDataCodec(),
        static metadata => metadata.DebugData1,
        static (executionPhaseCounter, debugData0, debugData1) => new CommandMetadata(ServerCommandId: null, executionPhaseCounter, debugData0, debugData1)
    );

    /// <summary>
    /// Gets the metadata codec of clientbound commands, which lead with the server command id.
    /// </summary>
    internal static ICodec<CommandMetadata> ServerCodec { get; } = Codecs.Composite(
        Codecs.Map(Codecs.VarInt, static id => ValidateServerCommandId(id), static id => ValidateServerCommandId(id)),
        static metadata => metadata.ServerCommandId ?? throw new InvalidDataException(MissingServerCommandIdMessage),
        Codecs.VarInt,
        static metadata => metadata.ExecutionPhaseCounter,
        DebugDataCodec(),
        static metadata => metadata.DebugData0,
        DebugDataCodec(),
        static metadata => metadata.DebugData1,
        static (serverCommandId, executionPhaseCounter, debugData0, debugData1) => new CommandMetadata(serverCommandId, executionPhaseCounter, debugData0, debugData1)
    );

    /// <summary>
    /// Gets the metadata a command carries.
    /// </summary>
    internal static CommandMetadata Of(Command command)
    {
        return new CommandMetadata((command as ServerCommand)?.ServerCommandId, command.ExecutionPhaseCounter, command.DebugData0, command.DebugData1);
    }

    /// <summary>
    /// Applies the metadata to a decoded command.
    /// </summary>
    internal void Apply(Command command)
    {
        command.ApplyMetadata(ExecutionPhaseCounter, DebugData0, DebugData1);

        if (command is ServerCommand server)
            server.ApplyServerCommandId(ServerCommandId ?? throw new InvalidDataException(MissingServerCommandIdMessage));
    }

    private static ICodec<CommandData?> DebugDataCodec()
    {
        return Codecs.OutsideProduction(Codecs.Optional(CommandData.Codec), absent: null);
    }

    private static int ValidateServerCommandId(int serverCommandId)
    {
        return serverCommandId is -1 ? throw new InvalidDataException(message: "Server command ID cannot be -1.") : serverCommandId;
    }
}
