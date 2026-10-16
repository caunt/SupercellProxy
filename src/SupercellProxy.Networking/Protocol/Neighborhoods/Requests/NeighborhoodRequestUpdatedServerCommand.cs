using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Replaces the state of a neighborhood request.</summary>
/// <param name="RequesterHomeId">Home of the member who asked.</param>
/// <param name="RequestId">Id of the request on the neighbourhood board.</param>
/// <param name="State">New state of the request; its values are not identified.</param>
/// <param name="Request">The request as it is now.</param>
public sealed record NeighborhoodRequestUpdatedServerCommand(LongId RequesterHomeId, LongId RequestId, int State, NeighborhoodRequest? Request) : CodecServerCommand<NeighborhoodRequestUpdatedServerCommand>, ICommandCodecProvider<NeighborhoodRequestUpdatedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodRequestUpdatedServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.RequesterHomeId,
            Codecs.LongId,
            static command => command.RequestId,
            Codecs.VarInt,
            static command => command.State,
            Codecs.Optional(NeighborhoodRequest.Codec),
            static command => command.Request,
            static (requesterHomeId, requestId, state, request) => new NeighborhoodRequestUpdatedServerCommand(requesterHomeId, requestId, state, request)
        )
    );
}
