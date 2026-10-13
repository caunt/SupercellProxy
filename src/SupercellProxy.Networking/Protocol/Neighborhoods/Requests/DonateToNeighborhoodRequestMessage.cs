using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Donates goods to a neighborhood request.</summary>
/// <param name="ItemGlobalId">The item data id.</param>
/// <param name="Amount">The amount.</param>
/// <param name="RequesterHomeId">The requester home id.</param>
/// <param name="RequestId">The request id.</param>
/// <param name="DonorHomeId">The donor home id.</param>
/// <param name="RequesterName">The requester name.</param>
/// <param name="Premium">The premium.</param>
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
