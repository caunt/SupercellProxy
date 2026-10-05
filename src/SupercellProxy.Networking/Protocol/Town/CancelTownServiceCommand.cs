using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Cancels an unstarted town service and sends its passenger toward the train.</summary>
public sealed record CancelTownServiceCommand(
    int ServiceIndex,
    int ServiceBuildingGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CancelTownServiceCommandType;

    /// <summary>Decodes the running service index, building identifier and command metadata.</summary>
    public static CancelTownServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
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
