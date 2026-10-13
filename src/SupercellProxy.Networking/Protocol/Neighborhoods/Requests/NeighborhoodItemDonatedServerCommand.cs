using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Confirms an item the avatar donated to a neighborhood request.</summary>
/// <param name="RequesterHomeId">The requester home id.</param>
/// <param name="DonorHomeId">The donor home id.</param>
/// <param name="RequestId">The request id.</param>
/// <param name="Amount">The amount.</param>
/// <param name="Request">The request.</param>
public sealed record NeighborhoodItemDonatedServerCommand(LongId RequesterHomeId, LongId DonorHomeId, LongId RequestId, int Amount, NeighborhoodRequest? Request) : CodecServerCommand<NeighborhoodItemDonatedServerCommand>, ICommandCodecProvider<NeighborhoodItemDonatedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodItemDonatedServerCommand> Codec { get; } = Codecs.Command(
        type: 197,
        Codecs.Composite(
            Codecs.LongId,
            static command => command.RequesterHomeId,
            Codecs.LongId,
            static command => command.DonorHomeId,
            Codecs.LongId,
            static command => command.RequestId,
            Codecs.VarInt,
            static command => command.Amount,
            Codecs.Optional(NeighborhoodRequest.Codec),
            static command => command.Request,
            static (requesterHomeId, donorHomeId, requestId, amount, request) => new NeighborhoodItemDonatedServerCommand(requesterHomeId, donorHomeId, requestId, amount, request)
        )
    );
}
