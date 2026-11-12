using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Updates the neighborhood-wide points for an active Neighborhood Object event.</summary>
/// <param name="NeighborhoodPoints">The neighbourhood's new combined event points as a 32-bit integer.</param>
/// <param name="ActiveEventId">Id of the live event the points belong to.</param>
/// <param name="EventStateValue">Variant id of that live event.</param>
public sealed record NeighborhoodObjectPointsServerCommand(int NeighborhoodPoints, int ActiveEventId, int EventStateValue) : CodecServerCommand<NeighborhoodObjectPointsServerCommand>, ICommandCodecProvider<NeighborhoodObjectPointsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodObjectPointsServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Int32,
        static command => command.NeighborhoodPoints,
        Codecs.Int32,
        static command => command.ActiveEventId,
        Codecs.Int32,
        static command => command.EventStateValue,
        static (points, eventId, stateValue) => new NeighborhoodObjectPointsServerCommand(points, eventId, stateValue)));
}
