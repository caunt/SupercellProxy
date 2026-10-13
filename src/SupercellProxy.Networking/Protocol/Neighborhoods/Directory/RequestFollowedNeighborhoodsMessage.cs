using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>Requests the neighborhoods the avatar follows.</summary>
public sealed record RequestFollowedNeighborhoodsMessage : CodecMessage<RequestFollowedNeighborhoodsMessage>, ICodecProvider<RequestFollowedNeighborhoodsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestFollowedNeighborhoodsMessage> Codec { get; } = Codecs.Unit(static () => new RequestFollowedNeighborhoodsMessage());
}
