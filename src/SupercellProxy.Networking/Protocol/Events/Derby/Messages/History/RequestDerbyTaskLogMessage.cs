using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Requests the neighborhood's derby task-log tab.</summary>
public sealed record RequestDerbyTaskLogMessage(LongId PlayerId, LongId NeighborhoodId) : CodecMessage<RequestDerbyTaskLogMessage>, ICodecProvider<RequestDerbyTaskLogMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDerbyTaskLogMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            static (playerId, neighborhoodId) => new RequestDerbyTaskLogMessage(playerId, neighborhoodId)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(RequestDerbyTaskLogMessage);
    }
}
