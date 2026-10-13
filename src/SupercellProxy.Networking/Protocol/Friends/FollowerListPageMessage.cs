using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Carries an ordered follower page and whether the server has more pages available.
public sealed record FollowerListPageMessage(bool HasMorePages, FriendEntry[]? Entries) : CodecMessage<FollowerListPageMessage>, ICodecProvider<FollowerListPageMessage>
{
    /// <inheritdoc />
    public static ICodec<FollowerListPageMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.Boolean,
            static message => message.HasMorePages,
            Codecs.NullableArray(FriendEntry.Codec, CollectionCount.PerElement(FriendEntry.MinimumEncodedSize).WithStrictAbsence()),
            static message => message.Entries,
            static (hasMorePages, entries) => new FollowerListPageMessage(hasMorePages, entries)
        ),
        trailingDataMessage: "The follower page has trailing data."
    );

    /// Omits the private follower records from diagnostics.
    public override string ToString()
    {
        return nameof(FollowerListPageMessage);
    }
}
