using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;

/// <summary>Removes the avatar's reaction from a neighborhood stream entry.</summary>
/// <param name="EntryId">Stream entry whose reaction is taken back.</param>
public sealed record RemoveStreamEntryReactionMessage(StreamEntryId? EntryId) : CodecMessage<RemoveStreamEntryReactionMessage>, ICodecProvider<RemoveStreamEntryReactionMessage>
{
    /// <inheritdoc />
    public static ICodec<RemoveStreamEntryReactionMessage> Codec { get; } = Codecs.Composite(
        Codecs.Optional(SupercellProxy.Networking.Protocol.Neighborhoods.Chat.StreamEntryId.Codec),
        static message => message.EntryId,
        static entryId => new RemoveStreamEntryReactionMessage(entryId)
    );
}
