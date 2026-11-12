using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Requests the selected derby instance's race page.</summary>
/// <param name="InstanceId">The derby race instance to show, such as <see cref="DerbyBoardMessage.CurrentInstanceId"/> or <see cref="DerbyBoardMessage.PreviousInstanceId"/>, or <see langword="null"/> to send it with a clear presence flag.</param>
/// <param name="NeighborhoodId">The neighborhood whose race page is wanted, or <see langword="null"/> to leave it absent on the wire.</param>
/// <param name="PlayerId">The account identifier of the requesting player, or <see langword="null"/> to leave it absent on the wire.</param>
public sealed record RequestDerbyRaceMessage(LongId? InstanceId, LongId? NeighborhoodId, LongId? PlayerId) : CodecMessage<RequestDerbyRaceMessage>, ICodecProvider<RequestDerbyRaceMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDerbyRaceMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.OptionalLongId,
            static message => message.InstanceId,
            Codecs.OptionalLongId,
            static message => message.NeighborhoodId,
            Codecs.OptionalLongId,
            static message => message.PlayerId,
            static (instanceId, neighborhoodId, playerId) => new RequestDerbyRaceMessage(instanceId, neighborhoodId, playerId)),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>RequestDerbyRaceMessage</c> alone, so the instance, neighborhood and player identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(RequestDerbyRaceMessage);
    }
}
