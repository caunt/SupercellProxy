using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Requests the rewards of finished events.</summary>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="EventIds">The event ids.</param>
public sealed record RequestEventRewardsMessage(LongId? NeighborhoodId, ReadOnlyMemory<EventReference>? EventIds) : CodecMessage<RequestEventRewardsMessage>, ICodecProvider<RequestEventRewardsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestEventRewardsMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.NeighborhoodId,
        Codecs.NullableMemory(EventReference.Codec),
        static message => message.EventIds,
        static (neighborhoodId, eventIds) => new RequestEventRewardsMessage(neighborhoodId, eventIds)
    );
}
