using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Reports the result of a follow relationship operation and its optional friend entry.
public sealed record FollowResponseMessage(FollowResultCode Result, FollowOperation Operation, FriendEntry? Entry) : CodecMessage<FollowResponseMessage>, ICodecProvider<FollowResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<FollowResponseMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.Map(Codecs.VarInt, static value => new FollowResultCode(value), static code => code.Value),
            static message => message.Result,
            Codecs.Map(Codecs.VarInt, static value => new FollowOperation(value), static operation => operation.Value),
            static message => message.Operation,
            Codecs.Optional(FriendEntry.Codec),
            static message => message.Entry,
            static (result, operation, entry) => new FollowResponseMessage(result, operation, entry)
        ),
        trailingDataMessage: "The follow response has trailing data."
    );

    /// Omits the private friend entry from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowResponseMessage);
    }
}
