using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Events;

/// <summary>Requests the neighborhood event board.</summary>
public sealed record RequestNeighborhoodEventBoardMessage : CodecMessage<RequestNeighborhoodEventBoardMessage>, ICodecProvider<RequestNeighborhoodEventBoardMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodEventBoardMessage> Codec { get; } = Codecs.Unit(static () => new RequestNeighborhoodEventBoardMessage());
}
