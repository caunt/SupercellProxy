using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Donates goods to a neighborhood request.</summary>
/// <param name="ItemGlobalId">Global id of the donated goods.</param>
/// <param name="Amount">How many goods the player gives.</param>
/// <param name="RequesterHomeId">Home of the member who asked.</param>
/// <param name="RequestId">Id of the request on the neighbourhood board.</param>
/// <param name="DonorHomeId">Home of the player who donates.</param>
/// <param name="RequesterName">Name of the member who asked.</param>
/// <param name="Premium">Flag sent with the donation; what it changes is not identified.</param>
public sealed record DonateToNeighborhoodRequestMessage(int ItemGlobalId, int Amount, LongId RequesterHomeId, LongId RequestId, LongId DonorHomeId, string? RequesterName, bool Premium) : CodecMessage<DonateToNeighborhoodRequestMessage>, ICodecProvider<DonateToNeighborhoodRequestMessage>
{
    /// <inheritdoc />
    public static ICodec<DonateToNeighborhoodRequestMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.ItemGlobalId,
        Codecs.VarInt,
        static message => message.Amount,
        Codecs.LongId,
        static message => message.RequesterHomeId,
        Codecs.LongId,
        static message => message.RequestId,
        Codecs.LongId,
        static message => message.DonorHomeId,
        Codecs.OptionalString,
        static message => message.RequesterName,
        Codecs.Boolean,
        static message => message.Premium,
        static (itemGlobalId, amount, requesterHomeId, requestId, donorHomeId, requesterName, premium) => new DonateToNeighborhoodRequestMessage(itemGlobalId, amount, requesterHomeId, requestId, donorHomeId, requesterName, premium)
    );
}
