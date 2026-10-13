using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Posts a notification entry to the neighborhood stream.</summary>
/// <param name="Text">The text.</param>
public sealed record PostNeighborhoodNotificationStreamMessage(string? Text) : CodecMessage<PostNeighborhoodNotificationStreamMessage>, ICodecProvider<PostNeighborhoodNotificationStreamMessage>
{
    /// <inheritdoc />
    public static ICodec<PostNeighborhoodNotificationStreamMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalString, static message => message.Text, static (text) => new PostNeighborhoodNotificationStreamMessage(text));
}
