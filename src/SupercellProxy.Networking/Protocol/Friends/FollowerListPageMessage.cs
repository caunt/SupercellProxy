using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Carries an ordered follower page and whether the server has more pages available.</summary>
/// <param name="HasMorePages"><see langword="true"/> if another <see cref="RequestFollowerListPageMessage"/> would return further followers; otherwise, <see langword="false"/>.</param>
/// <param name="Entries">The page's followers in server order, or <see langword="null"/> when the server sends the collection as absent.</param>
public sealed record FollowerListPageMessage(bool HasMorePages, ImmutableArray<FriendEntry>? Entries) : CodecMessage<FollowerListPageMessage>, ICodecProvider<FollowerListPageMessage>
{
    /// <inheritdoc />
    public static ICodec<FollowerListPageMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.Boolean,
            static message => message.HasMorePages,
            Codecs.NullableArray(FriendEntry.Codec, CollectionCount.PerElement(FriendEntry.MinimumEncodedSize).WithStrictAbsence()),
            static message => message.Entries,
            static (hasMorePages, entries) => new FollowerListPageMessage(hasMorePages, entries)),
        trailingDataMessage: "The follower page has trailing data.");

    /// <summary>Omits the private follower records from diagnostics.</summary>
    /// <returns>Always the type name <c>FollowerListPageMessage</c>, without any follower entry.</returns>
    public override string ToString()
    {
        return nameof(FollowerListPageMessage);
    }
}
