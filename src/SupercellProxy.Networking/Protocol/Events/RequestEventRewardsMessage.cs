using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Requests the rewards of finished events.</summary>
/// <param name="NeighborhoodId">Neighborhood the player took part in the events with; null when none.</param>
/// <param name="EventIds">The finished event runs to fetch rewards for; null when the list is absent.</param>
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
