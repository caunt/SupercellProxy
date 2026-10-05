using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Books a passenger into a town service slot.</summary>
public sealed record BookTownPassengerServiceCommand(
    int PassengerGlobalIdentifier,
    int ServiceIndex,
    int TutorialDataGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.BookTownPassengerServiceCommandType;

    /// <summary>Decodes the passenger, service and optional tutorial before the base command fields.</summary>
    public static BookTownPassengerServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int passenger = stream.ReadVariableInt();
        int service = stream.ReadVariableInt();
        int tutorial = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new BookTownPassengerServiceCommand(passenger, service, tutorial, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(PassengerGlobalIdentifier);
        stream.WriteVariableInt(ServiceIndex);
        stream.WriteVariableInt(TutorialDataGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
