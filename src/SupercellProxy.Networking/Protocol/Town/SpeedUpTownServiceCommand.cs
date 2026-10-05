using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Instantly completes a running town service using premium currency.</summary>
public sealed record SpeedUpTownServiceCommand(
    int ServiceIndex,
    int ServiceBuildingGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SpeedUpTownServiceCommandType;

    /// <summary>Decodes the service index, building identifier, and command fields.</summary>
    public static SpeedUpTownServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int service = stream.ReadVariableInt();
        int building = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new(service, building, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(ServiceIndex);
        stream.WriteVariableInt(ServiceBuildingGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
