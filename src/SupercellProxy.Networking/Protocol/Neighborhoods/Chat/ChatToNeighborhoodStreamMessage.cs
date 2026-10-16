using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Posts a chat message to the neighborhood stream.</summary>
/// <param name="Text">What the player wrote.</param>
public sealed record ChatToNeighborhoodStreamMessage(string? Text) : CodecMessage<ChatToNeighborhoodStreamMessage>, ICodecProvider<ChatToNeighborhoodStreamMessage>
{
    /// <inheritdoc />
    public static ICodec<ChatToNeighborhoodStreamMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalString, static message => message.Text, static text => new ChatToNeighborhoodStreamMessage(text));
}
