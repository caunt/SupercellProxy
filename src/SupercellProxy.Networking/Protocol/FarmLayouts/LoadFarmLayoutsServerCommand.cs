using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>
/// Defines the Load Farm Layouts Server Command contract.
/// </summary>
/// <summary>
/// Defines the Game Mode contract.
/// </summary>
/// <summary>
/// Defines the Compressed Layouts contract.
/// </summary>
public sealed record LoadFarmLayoutsServerCommand(
    int GameMode,
    ReadOnlyMemory<byte>? CompressedLayouts,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.LoadFarmLayoutsCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static LoadFarmLayoutsServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);
        int mode = stream.ReadVariableInt();
        ReadOnlyMemory<byte>? layouts = stream.ReadBoolean() ? stream.ReadByteArray() : null;

        return new LoadFarmLayoutsServerCommand(mode, layouts, serverCommandIdentifier, commandFields.ExecutionPhaseCounter, commandFields.DebugData0, commandFields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeServerCommand(stream, environment);
        stream.WriteVariableInt(GameMode);
        stream.WriteBoolean(CompressedLayouts is not null);

        if (CompressedLayouts is { } layouts)
            stream.WriteByteArray(layouts.Span);
    }
}
