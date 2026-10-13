using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Updates the neighborhood-wide points for an active Neighborhood Object event.</summary>
public sealed record NeighborhoodObjectPointsServerCommand(int NeighborhoodPoints, int ActiveEventId, int EventStateValue) : CodecServerCommand<NeighborhoodObjectPointsServerCommand>, ICommandCodecProvider<NeighborhoodObjectPointsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodObjectPointsServerCommand> Codec { get; } = Codecs.Command(
        type: 384,
        Codecs.Composite(
            Codecs.Int32,
            static command => command.NeighborhoodPoints,
            Codecs.Int32,
            static command => command.ActiveEventId,
            Codecs.Int32,
            static command => command.EventStateValue,
            static (points, eventId, stateValue) => new NeighborhoodObjectPointsServerCommand(points, eventId, stateValue)
        )
    );
}
