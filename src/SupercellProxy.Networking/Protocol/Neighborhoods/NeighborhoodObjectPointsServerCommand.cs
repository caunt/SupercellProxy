using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Updates the neighborhood-wide points for an active Neighborhood Object event.</summary>
public sealed record NeighborhoodObjectPointsServerCommand(
    int NeighborhoodPoints,
    int ActiveEventIdentifier,
    int EventStateValue,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.NeighborhoodObjectPointsServerCommandType;

    /// <summary>Decodes the points, event identity, state value, and base server fields.</summary>
    public static NeighborhoodObjectPointsServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int points = stream.ReadInt32();
        int eventIdentifier = stream.ReadInt32();
        int stateValue = stream.ReadInt32();
        (int identifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields) = DecodeServerCommand(stream, environment);

        return new NeighborhoodObjectPointsServerCommand(points, eventIdentifier, stateValue, identifier, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteInt32(NeighborhoodPoints);
        stream.WriteInt32(ActiveEventIdentifier);
        stream.WriteInt32(EventStateValue);
        EncodeServerCommand(stream, environment);
    }
}
