using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// Carries one server-pushed friend-list update.
public sealed record FriendListUpdateMessage(FriendEntry Entry) : CodecMessage<FriendListUpdateMessage>, ICodecProvider<FriendListUpdateMessage>
{
    /// <inheritdoc />
    public static ICodec<FriendListUpdateMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(FriendEntry.Codec, static message => message.Entry, static (entry) => new FriendListUpdateMessage(entry)),
        trailingDataMessage: "The friend-list update has trailing data."
    );

    /// Omits the private entry from diagnostics.
    public override string ToString()
    {
        return nameof(FriendListUpdateMessage);
    }
}
