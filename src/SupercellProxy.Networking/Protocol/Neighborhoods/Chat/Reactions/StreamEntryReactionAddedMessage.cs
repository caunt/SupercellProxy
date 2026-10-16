using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;

/// <summary>Adds a reaction to a stream entry in the neighborhood chat and the game log; the client plays the reaction sound when the reactor is the player.</summary>
/// <param name="EntryId">Stream entry that got the reaction.</param>
/// <param name="Reaction">Which reaction was added; its values are not identified.</param>
/// <param name="AvatarId">The reacting avatar.</param>
public sealed record StreamEntryReactionAddedMessage(StreamEntryId? EntryId, int Reaction, LongId? AvatarId) : CodecMessage<StreamEntryReactionAddedMessage>, ICodecProvider<StreamEntryReactionAddedMessage>
{
    /// <inheritdoc />
    public static ICodec<StreamEntryReactionAddedMessage> Codec { get; } = Codecs.Composite(
        Codecs.Optional(SupercellProxy.Networking.Protocol.Neighborhoods.Chat.StreamEntryId.Codec),
        static message => message.EntryId,
        Codecs.VarInt,
        static message => message.Reaction,
        Codecs.OptionalLongId,
        static message => message.AvatarId,
        static (entryId, reaction, avatarId) => new StreamEntryReactionAddedMessage(entryId, reaction, avatarId)
    );
}
