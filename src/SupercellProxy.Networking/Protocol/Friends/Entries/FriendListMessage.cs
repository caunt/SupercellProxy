using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// Carries a nullable, mode-specific list of friend entries.
public sealed record FriendListMessage(int Mode, FriendEntry[]? Entries) : CodecMessage<FriendListMessage>, ICodecProvider<FriendListMessage>
{
    /// Selects the separate recipient-list consumer.
    public const int RecipientMode = 4;

    /// Selects the native in-game friend and pending-request list.
    public const int RelationshipMode = 3;
    private const int MaximumEntryCount = 100_000;

    /// <inheritdoc />
    public static ICodec<FriendListMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.Mode,
            Codecs.NullableArray(FriendEntry.Codec, CollectionCount.PerElement(FriendEntry.MinimumEncodedSize).WithMaximum(MaximumEntryCount).WithStrictAbsence()),
            static message => message.Entries,
            static (mode, entries) => new FriendListMessage(mode, entries)
        ),
        trailingDataMessage: "The friend list has trailing data."
    );

    /// Omits private entries from diagnostics.
    public override string ToString()
    {
        return nameof(FriendListMessage);
    }
}
