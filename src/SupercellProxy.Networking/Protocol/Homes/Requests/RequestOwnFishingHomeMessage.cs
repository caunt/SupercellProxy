using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests the authenticated player's native fishing-area snapshot.</summary>
public sealed record RequestOwnFishingHomeMessage : CodecMessage<RequestOwnFishingHomeMessage>, ICodecProvider<RequestOwnFishingHomeMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestOwnFishingHomeMessage> Codec { get; } = Codecs.Complete(
        Codecs.Unit(static () => new RequestOwnFishingHomeMessage()),
        trailingDataMessage: "The own-fishing-home request must have an empty payload."
    );
}
