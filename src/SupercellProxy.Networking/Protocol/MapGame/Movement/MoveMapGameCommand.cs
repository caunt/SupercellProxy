using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Movement;

/// <summary>Requests movement of the local Valley pawn from its current node toward another map node.</summary>
public sealed record MoveMapGameCommand(
    int SourceNodeIdentifier,
    int TargetNodeIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MoveMapGameCommandType;

    /// <summary>Decodes a command from its native wire representation.</summary>
    public static MoveMapGameCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);

        return new MoveMapGameCommand(
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(SourceNodeIdentifier);
        stream.WriteVariableInt(TargetNodeIdentifier);
    }
}
