using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Delivers an item a neighbor donated to the avatar's request.</summary>
/// <param name="RequesterHomeId">Home of the player who asked.</param>
/// <param name="DonorHomeId">Home of the neighbour who donated.</param>
/// <param name="RequestId">Id of the request on the neighbourhood board.</param>
/// <param name="Amount">How many goods the neighbour gave.</param>
/// <param name="Request">The request after the donation.</param>
/// <param name="DonationIndex">Position of this donation in the request's donation list.</param>
public sealed record NeighborhoodItemReceivedServerCommand(LongId RequesterHomeId, LongId DonorHomeId, LongId RequestId, int Amount, NeighborhoodRequest? Request, int DonationIndex) : CodecServerCommand<NeighborhoodItemReceivedServerCommand>, ICommandCodecProvider<NeighborhoodItemReceivedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodItemReceivedServerCommand> Codec { get; } = Codecs.Command(
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
            Codecs.VarInt,
            static command => command.DonationIndex,
            static (requesterHomeId, donorHomeId, requestId, amount, request, donationIndex) => new NeighborhoodItemReceivedServerCommand(requesterHomeId, donorHomeId, requestId, amount, request, donationIndex)
        )
    );
}
