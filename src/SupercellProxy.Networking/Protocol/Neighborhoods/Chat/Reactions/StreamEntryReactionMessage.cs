using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;

/// <summary>Carries a reaction for a stream entry; the client ignores it.</summary>
/// <param name="Reaction">Which reaction it is; its values are not identified.</param>
/// <param name="EntryId">Stream entry the reaction belongs to.</param>
public sealed record StreamEntryReactionMessage(int Reaction, StreamEntryId? EntryId) : CodecMessage<StreamEntryReactionMessage>, ICodecProvider<StreamEntryReactionMessage>
{
    /// <inheritdoc />
    public static ICodec<StreamEntryReactionMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Reaction,
        Codecs.Optional(SupercellProxy.Networking.Protocol.Neighborhoods.Chat.StreamEntryId.Codec),
        static message => message.EntryId,
        static (reaction, entryId) => new StreamEntryReactionMessage(reaction, entryId)
    );
}
