using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Marks an inbox entry as removed in the game log manager.</summary>
/// <param name="EntryId">The entry id.</param>
public sealed record AvatarStreamEntryRemovedMessage(LongId EntryId) : CodecMessage<AvatarStreamEntryRemovedMessage>, ICodecProvider<AvatarStreamEntryRemovedMessage>
{
    /// <inheritdoc />
    public static ICodec<AvatarStreamEntryRemovedMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.EntryId, static (entryId) => new AvatarStreamEntryRemovedMessage(entryId));
}
