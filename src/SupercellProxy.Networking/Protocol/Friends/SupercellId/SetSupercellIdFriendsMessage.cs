using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.SupercellId;

/// <summary>Sends the Supercell ID friend list.</summary>
/// <param name="FriendIds">The friend ids; native writes their count as a 32-bit integer.</param>
public sealed record SetSupercellIdFriendsMessage(ReadOnlyMemory<string?> FriendIds) : CodecMessage<SetSupercellIdFriendsMessage>, ICodecProvider<SetSupercellIdFriendsMessage>
{
    /// <inheritdoc />
    public static ICodec<SetSupercellIdFriendsMessage> Codec { get; } = Codecs.Composite(
        Codecs.Memory(Codecs.OptionalString, CollectionCount.Int32),
        static message => message.FriendIds,
        static friendIds => new SetSupercellIdFriendsMessage(friendIds)
    );
}
