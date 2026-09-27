using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Starts a selected service at a town service building.</summary>
public sealed record StartTownServiceCommand(
    int ServiceIndex,
    int ServiceBuildingGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.StartTownServiceCommandType;

    /// <summary>Decodes the service index and building before the base command fields.</summary>
    public static StartTownServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int index = stream.ReadVariableInt();
        int building = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new StartTownServiceCommand(index, building, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(ServiceIndex);
        stream.WriteVariableInt(ServiceBuildingGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
