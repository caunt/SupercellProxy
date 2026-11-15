using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests the authenticated player's native town snapshot.</summary>
public sealed record RequestOwnTownMessage : CodecMessage<RequestOwnTownMessage>, ICodecProvider<RequestOwnTownMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestOwnTownMessage> Codec { get; } = Codecs.Complete(Codecs.Unit(static () => new RequestOwnTownMessage()), trailingDataMessage: "The own-town request must have an empty payload.");
}
