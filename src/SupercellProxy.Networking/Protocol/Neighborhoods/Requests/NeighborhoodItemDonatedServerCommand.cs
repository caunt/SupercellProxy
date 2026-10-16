using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Confirms an item the avatar donated to a neighborhood request.</summary>
/// <param name="RequesterHomeId">Home of the member who asked.</param>
/// <param name="DonorHomeId">Home of the player who donated.</param>
/// <param name="RequestId">Id of the request on the neighbourhood board.</param>
/// <param name="Amount">How many goods the player gave.</param>
/// <param name="Request">The request after the donation.</param>
public sealed record NeighborhoodItemDonatedServerCommand(LongId RequesterHomeId, LongId DonorHomeId, LongId RequestId, int Amount, NeighborhoodRequest? Request) : CodecServerCommand<NeighborhoodItemDonatedServerCommand>, ICommandCodecProvider<NeighborhoodItemDonatedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodItemDonatedServerCommand> Codec { get; } = Codecs.Command(
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
