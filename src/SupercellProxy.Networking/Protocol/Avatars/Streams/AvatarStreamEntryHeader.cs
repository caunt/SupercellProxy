using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>The fields native <c>AvatarStreamEntry::decode</c> reads before every inbox entry's own fields.</summary>
/// <param name="EntryId">Identifies the entry, so later messages can mark it removed.</param>
/// <param name="SenderAvatarId">The avatar that caused the entry.</param>
/// <param name="SenderName">The sender's name.</param>
/// <param name="SenderExpLevel">The sender's experience level.</param>
/// <param name="AgeSeconds">The entry's age in seconds.</param>
/// <param name="Removed">Whether the entry was removed; AvatarStreamEntryRemovedMessage sets it on a received entry.</param>
/// <param name="New">Whether the entry is new.</param>
public sealed record AvatarStreamEntryHeader(LongId EntryId, LongId SenderAvatarId, string? SenderName, int SenderExpLevel, int AgeSeconds, bool Removed, bool New) : ICodecProvider<AvatarStreamEntryHeader>
{
    /// <inheritdoc />
    public static ICodec<AvatarStreamEntryHeader> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static value => value.EntryId,
        Codecs.LongId,
        static value => value.SenderAvatarId,
        Codecs.OptionalString,
        static value => value.SenderName,
        Codecs.VarInt,
        static value => value.SenderExpLevel,
        Codecs.VarInt,
        static value => value.AgeSeconds,
        Codecs.Boolean,
        static value => value.Removed,
        Codecs.Boolean,
        static value => value.New,
        static (entryId, senderAvatarId, senderName, senderExpLevel, ageSeconds, removed, isNew) => new AvatarStreamEntryHeader(entryId, senderAvatarId, senderName, senderExpLevel, ageSeconds, removed, isNew)
    );
}
