using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;

/// <summary>Removes an avatar's reaction from a stream entry in the neighborhood chat and the game log.</summary>
/// <param name="EntryId">Stream entry that lost the reaction.</param>
/// <param name="AvatarId">The avatar whose reaction is removed.</param>
public sealed record StreamEntryReactionRemovedMessage(StreamEntryId? EntryId, LongId? AvatarId) : CodecMessage<StreamEntryReactionRemovedMessage>, ICodecProvider<StreamEntryReactionRemovedMessage>
{
    /// <inheritdoc />
    public static ICodec<StreamEntryReactionRemovedMessage> Codec { get; } = Codecs.Composite(
        Codecs.Optional(SupercellProxy.Networking.Protocol.Neighborhoods.Chat.StreamEntryId.Codec),
        static message => message.EntryId,
        Codecs.OptionalLongId,
        static message => message.AvatarId,
        static (entryId, avatarId) => new StreamEntryReactionRemovedMessage(entryId, avatarId)
    );
}
