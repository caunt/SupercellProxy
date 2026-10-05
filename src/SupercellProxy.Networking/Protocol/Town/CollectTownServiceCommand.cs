using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Collects a completed town service and releases its passenger.</summary>
public sealed record CollectTownServiceCommand(
    int ServiceBuildingGlobalIdentifier,
    int FinishedServiceIndex,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectTownServiceCommandType;

    /// <summary>Decodes the building identifier, finished-service index, and command fields.</summary>
    public static CollectTownServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int building = stream.ReadVariableInt();
        int service = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new(building, service, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(ServiceBuildingGlobalIdentifier);
        stream.WriteVariableInt(FinishedServiceIndex);
        EncodeCommand(stream, environment);
    }
}
