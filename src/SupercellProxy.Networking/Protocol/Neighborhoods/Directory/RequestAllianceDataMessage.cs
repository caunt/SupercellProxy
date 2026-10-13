using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>Requests the data of the avatar's neighborhood.</summary>
public sealed record RequestAllianceDataMessage : CodecMessage<RequestAllianceDataMessage>, ICodecProvider<RequestAllianceDataMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestAllianceDataMessage> Codec { get; } = Codecs.Unit(static () => new RequestAllianceDataMessage());
}
