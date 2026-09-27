using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Declines a waiting farm visitor's goods offer.</summary>
public sealed record RejectFarmVisitorOrderCommand(int VisitorGlobalIdentifier, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc/>
    public override int Type => CommandRegistry.RejectFarmVisitorOrderCommandType;

    /// <inheritdoc/>
    public static RejectFarmVisitorOrderCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int visitorGlobalIdentifier = stream.ReadVariableInt();
        (int phase, CommandData? debug0, CommandData? debug1) = DecodeCommand(stream, environment);

        return new RejectFarmVisitorOrderCommand(visitorGlobalIdentifier, phase, debug0, debug1);
    }

    /// <inheritdoc/>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(VisitorGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
