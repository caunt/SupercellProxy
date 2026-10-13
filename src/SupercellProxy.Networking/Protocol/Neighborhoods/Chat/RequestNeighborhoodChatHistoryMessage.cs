using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Requests the neighborhood chat history.</summary>
public sealed record RequestNeighborhoodChatHistoryMessage : CodecMessage<RequestNeighborhoodChatHistoryMessage>, ICodecProvider<RequestNeighborhoodChatHistoryMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodChatHistoryMessage> Codec { get; } = Codecs.Unit(static () => new RequestNeighborhoodChatHistoryMessage());
}
