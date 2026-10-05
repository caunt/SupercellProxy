using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Validates that a passenger is present in the player's town without changing its state.</summary>
public sealed record ValidateTownPassengerCommand(int PassengerGlobalIdentifier, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ValidateTownPassengerCommandType;

    /// <summary>Decodes the passenger identifier and command fields.</summary>
    public static ValidateTownPassengerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int passenger = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new(passenger, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(PassengerGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
