using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Requests the neighborhood's derby task-log tab.</summary>
/// <param name="PlayerId">The account identifier of the requesting player, written first.</param>
/// <param name="NeighborhoodId">The identifier of the neighborhood whose task log is wanted; the server answers with <see cref="DerbyTaskLogMessage"/>.</param>
public sealed record RequestDerbyTaskLogMessage(LongId PlayerId, LongId NeighborhoodId) : CodecMessage<RequestDerbyTaskLogMessage>, ICodecProvider<RequestDerbyTaskLogMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDerbyTaskLogMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId,
            static message => message.PlayerId,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            static (playerId, neighborhoodId) => new RequestDerbyTaskLogMessage(playerId, neighborhoodId)),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>RequestDerbyTaskLogMessage</c> alone, so the player and neighborhood identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(RequestDerbyTaskLogMessage);
    }
}
