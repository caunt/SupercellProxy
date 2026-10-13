using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>The neighborhood request.</summary>
/// <param name="RequesterHomeId">The requester home id.</param>
/// <param name="ItemGlobalId">The item data id.</param>
/// <param name="RequestedAmount">The requested amount.</param>
/// <param name="ReceivedAmount">The received amount.</param>
/// <param name="CreatedAt">The created at.</param>
/// <param name="Closed">The closed.</param>
/// <param name="RequesterName">The requester name.</param>
/// <param name="Donations">The donations.</param>
/// <param name="LastDonorHomeId">The last donor home id.</param>
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
