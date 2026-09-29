using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Balloons;

/// <summary>Pops an unpopped balloon identified by its Balloon data row.</summary>
public sealed record PopBalloonCommand(int BalloonDataGlobalIdentifier, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.PopBalloonCommandType;

    /// <summary>Decodes the Balloon data identifier followed by the base command fields.</summary>
    public static PopBalloonCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int identifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new PopBalloonCommand(identifier, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(BalloonDataGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
