using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests the Town belonging to the selected home.</summary>
public sealed record RequestOtherTownMessage(LongId HomeId) : CodecMessage<RequestOtherTownMessage>, ICodecProvider<RequestOtherTownMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestOtherTownMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.HomeId, static (homeId) => new RequestOtherTownMessage(homeId));
}
