using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Visited Boat State Server Command contract.
/// </summary>
/// <summary>
/// Defines the New State contract.
/// </summary>
/// <summary>
/// Defines the Expected State contract.
/// </summary>
/// <summary>
/// Defines the Order Index contract.
/// </summary>
/// <summary>
/// Defines the Owner High contract.
/// </summary>
/// <summary>
/// Defines the Owner Low contract.
/// </summary>
/// <summary>
/// Defines the State Duration contract.
/// </summary>
/// <summary>
/// Defines the State Ticks contract.
/// </summary>
public sealed record VisitedBoatStateServerCommand(
    int NewState,
    int ExpectedState,
    int OrderIndex,
    int OwnerHigh,
    int OwnerLow,
    int StateDuration,
    int StateTicks,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.VisitedBoatStateServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static VisitedBoatStateServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new VisitedBoatStateServerCommand(
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            serverCommandIdentifier,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeServerCommand(stream, environment);
        stream.WriteVariableInt(NewState);
        stream.WriteVariableInt(ExpectedState);
        stream.WriteVariableInt(OrderIndex);
        stream.WriteVariableInt(OwnerHigh);
        stream.WriteVariableInt(OwnerLow);
        stream.WriteVariableInt(StateDuration);
        stream.WriteVariableInt(StateTicks);
    }
}
