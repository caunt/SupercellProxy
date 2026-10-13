using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Opens the friend book; the server answers with the friend list and pending friend requests.</summary>
public sealed record RequestFriendListMessage : CodecMessage<RequestFriendListMessage>, ICodecProvider<RequestFriendListMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestFriendListMessage> Codec { get; } = Codecs.Unit(static () => new RequestFriendListMessage());
}
