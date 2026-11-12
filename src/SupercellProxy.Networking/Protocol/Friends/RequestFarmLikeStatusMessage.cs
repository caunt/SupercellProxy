using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Asks whether the visitor has already liked the identified farm.</summary>
/// <param name="HomeOwnerId">Home id of the owner of the farm being visited.</param>
/// <param name="VisitorId">Home id of the visiting player whose like is asked about; the answer carries it back so it can be matched.</param>
public sealed record RequestFarmLikeStatusMessage(LongId HomeOwnerId, LongId VisitorId) : CodecMessage<RequestFarmLikeStatusMessage>, ICodecProvider<RequestFarmLikeStatusMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestFarmLikeStatusMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId,
            static message => message.HomeOwnerId,
            Codecs.LongId,
            static message => message.VisitorId,
            static (homeOwnerId, visitorId) => new RequestFarmLikeStatusMessage(homeOwnerId, visitorId)),
        trailingDataMessage: "The farm-like status request has trailing data.");

    /// <summary>Omits the private identifiers from diagnostics.</summary>
    /// <returns>Always the type name <c>RequestFarmLikeStatusMessage</c>, without either home id.</returns>
    public override string ToString()
    {
        return nameof(RequestFarmLikeStatusMessage);
    }
}
