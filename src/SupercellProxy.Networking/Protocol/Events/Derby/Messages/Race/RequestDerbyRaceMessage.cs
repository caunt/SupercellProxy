using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Requests the selected derby instance's race page.</summary>
public sealed record RequestDerbyRaceMessage(LongId? InstanceId, LongId? NeighborhoodId, LongId? PlayerId) : CodecMessage<RequestDerbyRaceMessage>, ICodecProvider<RequestDerbyRaceMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDerbyRaceMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.OptionalLongId,
            static message => message.InstanceId,
            Codecs.OptionalLongId,
            static message => message.NeighborhoodId,
            Codecs.OptionalLongId,
            static message => message.PlayerId,
            static (instanceId, neighborhoodId, playerId) => new RequestDerbyRaceMessage(instanceId, neighborhoodId, playerId)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(RequestDerbyRaceMessage);
    }
}
