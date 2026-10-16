using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>A member's request for goods on the neighbourhood board, with the donations it got so far.</summary>
/// <param name="RequesterHomeId">Home of the member who asked.</param>
/// <param name="ItemGlobalId">Global id of the requested goods.</param>
/// <param name="RequestedAmount">How many goods the member asked for.</param>
/// <param name="ReceivedAmount">How many goods were donated so far.</param>
/// <param name="CreatedAt">When the request was made; its unit is not identified.</param>
/// <param name="Closed">Whether the request takes no more donations.</param>
/// <param name="RequesterName">Name of the member who asked.</param>
/// <param name="Donations">Each donation the request got.</param>
/// <param name="LastDonorHomeId">Home of the latest donor; null when nobody donated yet.</param>
public sealed record NeighborhoodRequest(
    LongId RequesterHomeId,
    int ItemGlobalId,
    int RequestedAmount,
    int ReceivedAmount,
    int CreatedAt,
    bool Closed,
    string? RequesterName,
    ReadOnlyMemory<NeighborhoodRequestDonation> Donations,
    LongId? LastDonorHomeId
) : ICodecProvider<NeighborhoodRequest>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodRequest> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static value => value.RequesterHomeId,
        Codecs.VarInt,
        static value => value.ItemGlobalId,
        Codecs.VarInt,
        static value => value.RequestedAmount,
        Codecs.VarInt,
        static value => value.ReceivedAmount,
        Codecs.VarInt,
        static value => value.CreatedAt,
        Codecs.Boolean,
        static value => value.Closed,
        Codecs.OptionalString,
        static value => value.RequesterName,
        Codecs.Memory(NeighborhoodRequestDonation.Codec),
        static value => value.Donations,
        Codecs.OptionalLongId,
        static value => value.LastDonorHomeId,
        static (requesterHomeId, itemGlobalId, requestedAmount, receivedAmount, createdAt, closed, requesterName, donations, lastDonorHomeId) => new NeighborhoodRequest(requesterHomeId, itemGlobalId, requestedAmount, receivedAmount, createdAt, closed, requesterName, donations, lastDonorHomeId)
    );
}
