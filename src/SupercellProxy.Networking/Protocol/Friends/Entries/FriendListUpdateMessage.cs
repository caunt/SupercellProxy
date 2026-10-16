using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// <summary>Carries one server-pushed friend-list update.</summary>
public sealed record FriendListUpdateMessage(FriendEntry Entry) : CodecMessage<FriendListUpdateMessage>, ICodecProvider<FriendListUpdateMessage>
{
    /// <inheritdoc />
    public static ICodec<FriendListUpdateMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(FriendEntry.Codec, static message => message.Entry, static entry => new FriendListUpdateMessage(entry)),
        trailingDataMessage: "The friend-list update has trailing data."
    );

    /// <summary>Omits the private entry from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(FriendListUpdateMessage);
    }
}
