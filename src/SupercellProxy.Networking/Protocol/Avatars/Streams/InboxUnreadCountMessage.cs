using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Reports how many inbox entries are unread; the inbox and notification tags show it.</summary>
/// <param name="UnreadCount">The unread count.</param>
public sealed record InboxUnreadCountMessage(int UnreadCount) : CodecMessage<InboxUnreadCountMessage>, ICodecProvider<InboxUnreadCountMessage>
{
    /// <inheritdoc />
    public static ICodec<InboxUnreadCountMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.UnreadCount, static (unreadCount) => new InboxUnreadCountMessage(unreadCount));
}
