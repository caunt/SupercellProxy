using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Updates the friend-bar state of friends and neighbours.</summary>
/// <param name="Friends">The friend-bar state of each listed home.</param>
public sealed record FriendMetadataMessage(ReadOnlyMemory<FriendMeta> Friends) : CodecMessage<FriendMetadataMessage>, ICodecProvider<FriendMetadataMessage>
{
    /// <inheritdoc />
    public static ICodec<FriendMetadataMessage> Codec { get; } = Codecs.Composite(
        Codecs.Memory(FriendMeta.Codec, CollectionCount.Unbounded),
        static message => message.Friends,
        static friends => new FriendMetadataMessage(friends)
    );
}
