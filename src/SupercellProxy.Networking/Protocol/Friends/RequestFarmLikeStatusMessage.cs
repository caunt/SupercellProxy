using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Asks whether the visitor has already liked the identified farm.</summary>
public sealed record RequestFarmLikeStatusMessage(LongId HomeOwnerId, LongId VisitorId) : CodecMessage<RequestFarmLikeStatusMessage>, ICodecProvider<RequestFarmLikeStatusMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestFarmLikeStatusMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.LongId,
            static message => message.HomeOwnerId,
            Codecs.LongId,
            static message => message.VisitorId,
            static (homeOwnerId, visitorId) => new RequestFarmLikeStatusMessage(homeOwnerId, visitorId)
        ),
        trailingDataMessage: "The farm-like status request has trailing data."
    );

    /// <summary>Omits the private identifiers from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(RequestFarmLikeStatusMessage);
    }
}
