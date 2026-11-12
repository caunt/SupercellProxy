using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Reports the result of a follow relationship operation and its optional friend entry.</summary>
/// <param name="Result">How the operation ended, such as <see cref="FollowResultCode.Success"/> or <see cref="FollowResultCode.TargetHasTooManyFollowers"/>.</param>
/// <param name="Operation">Which operation the response answers: follow, unfollow or remove follower.</param>
/// <param name="Entry">The farm the operation was about, or <see langword="null"/> when the server sends no entry.</param>
public sealed record FollowResponseMessage(FollowResultCode Result, FollowOperation Operation, FriendEntry? Entry) : CodecMessage<FollowResponseMessage>, ICodecProvider<FollowResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<FollowResponseMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.Map(Codecs.VarInt, static value => new FollowResultCode(value), static code => code.Value),
            static message => message.Result,
            Codecs.Map(Codecs.VarInt, static value => new FollowOperation(value), static operation => operation.Value),
            static message => message.Operation,
            Codecs.Optional(FriendEntry.Codec),
            static message => message.Entry,
            static (result, operation, entry) => new FollowResponseMessage(result, operation, entry)),
        trailingDataMessage: "The follow response has trailing data.");

    /// <summary>Omits the private friend entry from diagnostic text.</summary>
    /// <returns>Always the type name <c>FollowResponseMessage</c>, without the result, operation or entry.</returns>
    public override string ToString()
    {
        return nameof(FollowResponseMessage);
    }
}
