using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Applies a passenger action such as booking movement or holding the visitor for interaction.</summary>
public sealed record TownPassengerActionCommand(
    int PassengerGlobalIdentifier,
    int ActionCode,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>Moves the passenger toward a booked service building.</summary>
    public const int GoToServiceBuildingAction = 1;

    /// <summary>Sends the passenger toward the train.</summary>
    public const int GoToTrainAction = 2;

    /// <summary>Holds the passenger in place during an interaction.</summary>
    public const int HoldForInteractionAction = 3;

    /// <inheritdoc />
    public override int Type => CommandRegistry.TownPassengerActionCommandType;

    /// <summary>Decodes the passenger and action before the base command fields.</summary>
    public static TownPassengerActionCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int passenger = stream.ReadVariableInt();
        int action = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new TownPassengerActionCommand(passenger, action, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(PassengerGlobalIdentifier);
        stream.WriteVariableInt(ActionCode);
        EncodeCommand(stream, environment);
    }
}
