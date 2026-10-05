using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Updates the neighborhood-wide points for an active Neighborhood Object event.</summary>
public sealed record NeighborhoodObjectPointsServerCommand(int NeighborhoodPoints, int ActiveEventId, int EventStateValue) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.NeighborhoodObjectPointsServerCommandType;

    /// <summary>Decodes the points, event identity, and state value.</summary>
    public static NeighborhoodObjectPointsServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int points = stream.ReadInt32();
        int eventId = stream.ReadInt32();
        int stateValue = stream.ReadInt32();


        return new NeighborhoodObjectPointsServerCommand(points, eventId, stateValue);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteInt32(NeighborhoodPoints);
        stream.WriteInt32(ActiveEventId);
        stream.WriteInt32(EventStateValue);
    }
}
