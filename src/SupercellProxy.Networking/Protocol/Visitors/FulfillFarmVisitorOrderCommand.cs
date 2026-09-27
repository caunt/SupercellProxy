using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Accepts a waiting farm visitor's offer for its requested goods.</summary>
public sealed record FulfillFarmVisitorOrderCommand(int VisitorGlobalIdentifier, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc/>
    public override int Type => CommandRegistry.FulfillFarmVisitorOrderCommandType;

    /// <inheritdoc/>
    public static FulfillFarmVisitorOrderCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int phase, CommandData? debug0, CommandData? debug1) = DecodeCommand(stream, environment);

        return new FulfillFarmVisitorOrderCommand(stream.ReadVariableInt(), phase, debug0, debug1);
    }

    /// <inheritdoc/>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(VisitorGlobalIdentifier);
    }
}
