using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// <summary>Carries one server-pushed friend-list update.</summary>
/// <param name="Entry">The changed farm; its <see cref="FriendEntry.RelationshipStatus"/> says what happened, such as a new friend, a block, a hide or a removal.</param>
public sealed record FriendListUpdateMessage(FriendEntry Entry) : CodecMessage<FriendListUpdateMessage>, ICodecProvider<FriendListUpdateMessage>
{
    /// <inheritdoc />
    public static ICodec<FriendListUpdateMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(FriendEntry.Codec, static message => message.Entry, static entry => new FriendListUpdateMessage(entry)),
        trailingDataMessage: "The friend-list update has trailing data.");

    /// <summary>Omits the private entry from diagnostics.</summary>
    /// <returns>Always the type name <c>FriendListUpdateMessage</c>, without the entry.</returns>
    public override string ToString()
    {
        return nameof(FriendListUpdateMessage);
    }
}
