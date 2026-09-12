using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// <para>Base wire representation shared by commands issued by the server.</para>
/// </summary>
public abstract record ServerCommand : Command
{
    /// <summary>
    /// Initializes a new <see cref="ServerCommand"/> instance.
    /// </summary>
    protected ServerCommand(int serverCommandIdentifier, int executionPhaseCounter, CommandData? debugData0, CommandData? debugData1)
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        if (serverCommandIdentifier is -1)
            throw new InvalidDataException(message: "Server command ID cannot be -1.");

        ServerCommandIdentifier = serverCommandIdentifier;
    }

    /// <summary>
    /// Gets the <c language="csharp">ServerCommandId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ServerCommandId")]
    public int ServerCommandIdentifier { get; }

    /// <summary>
    /// <para>Decodes the common server-command header.</para>
    /// </summary>
    protected static (
        int ServerCommandId,
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) CommandFields
    ) DecodeServerCommand(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int serverCommandIdentifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) logicCommandFields = DecodeCommand(stream, environment);

        return serverCommandIdentifier is -1
            ? throw new InvalidDataException(message: "Server command ID cannot be -1.")
            : ((int ServerCommandId, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) CommandFields))(serverCommandIdentifier, logicCommandFields);
    }

    /// <summary>
    /// Executes the <c language="csharp">EncodeServerCommand</c> operation.
    /// </summary>
    protected void EncodeServerCommand(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(ServerCommandIdentifier);
        EncodeCommand(stream, environment);
    }
}
