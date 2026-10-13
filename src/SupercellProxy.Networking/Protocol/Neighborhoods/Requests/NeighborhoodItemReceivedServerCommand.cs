using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Delivers an item a neighbor donated to the avatar's request.</summary>
/// <param name="RequesterHomeId">The requester home id.</param>
/// <param name="DonorHomeId">The donor home id.</param>
/// <param name="RequestId">The request id.</param>
/// <param name="Amount">The amount.</param>
/// <param name="Request">The request.</param>
/// <param name="DonationIndex">The donation index.</param>
public sealed record NeighborhoodItemReceivedServerCommand(LongId RequesterHomeId, LongId DonorHomeId, LongId RequestId, int Amount, NeighborhoodRequest? Request, int DonationIndex) : CodecServerCommand<NeighborhoodItemReceivedServerCommand>, ICommandCodecProvider<NeighborhoodItemReceivedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodItemReceivedServerCommand> Codec { get; } = Codecs.Command(
        type: 198,
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
