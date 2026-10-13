using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Requests the friend book.</summary>
public sealed record RequestFriendBookMessage : CodecMessage<RequestFriendBookMessage>, ICodecProvider<RequestFriendBookMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestFriendBookMessage> Codec { get; } = Codecs.Unit(static () => new RequestFriendBookMessage());
}
