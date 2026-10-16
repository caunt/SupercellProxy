using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;

/// <summary>Carries a reaction value for a stream entry; the client ignores it.</summary>
/// <param name="Count">Number sent with the reaction, likely how many players gave it; not confirmed.</param>
/// <param name="EntryId">Stream entry the reaction belongs to.</param>
/// <param name="Reaction">Which reaction it is; its values are not identified.</param>
public sealed record StreamEntryReactionCountMessage(int Count, StreamEntryId? EntryId, int Reaction) : CodecMessage<StreamEntryReactionCountMessage>, ICodecProvider<StreamEntryReactionCountMessage>
{
    /// <inheritdoc />
    public static ICodec<StreamEntryReactionCountMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Count,
        Codecs.Optional(SupercellProxy.Networking.Protocol.Neighborhoods.Chat.StreamEntryId.Codec),
        static message => message.EntryId,
        Codecs.VarInt,
        static message => message.Reaction,
        static (count, entryId, reaction) => new StreamEntryReactionCountMessage(count, entryId, reaction)
    );
}
