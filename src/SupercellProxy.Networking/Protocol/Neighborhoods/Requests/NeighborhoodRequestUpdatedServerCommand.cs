using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Replaces the state of a neighborhood request.</summary>
/// <param name="RequesterHomeId">The requester home id.</param>
/// <param name="RequestId">The request id.</param>
/// <param name="State">The state.</param>
/// <param name="Request">The request.</param>
public sealed record NeighborhoodRequestUpdatedServerCommand(LongId RequesterHomeId, LongId RequestId, int State, NeighborhoodRequest? Request) : CodecServerCommand<NeighborhoodRequestUpdatedServerCommand>, ICommandCodecProvider<NeighborhoodRequestUpdatedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodRequestUpdatedServerCommand> Codec { get; } = Codecs.Command(
        type: 200,
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
