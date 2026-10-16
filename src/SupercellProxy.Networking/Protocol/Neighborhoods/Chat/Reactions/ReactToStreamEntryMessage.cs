using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;

/// <summary>Reacts to a neighborhood stream entry.</summary>
/// <param name="Reaction">Which reaction the player picked; its values are not identified.</param>
/// <param name="EntryId">Stream entry the player reacts to.</param>
public sealed record ReactToStreamEntryMessage(int Reaction, StreamEntryId? EntryId) : CodecMessage<ReactToStreamEntryMessage>, ICodecProvider<ReactToStreamEntryMessage>
{
    /// <inheritdoc />
    public static ICodec<ReactToStreamEntryMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Reaction,
        Codecs.Optional(SupercellProxy.Networking.Protocol.Neighborhoods.Chat.StreamEntryId.Codec),
        static message => message.EntryId,
        static (reaction, entryId) => new ReactToStreamEntryMessage(reaction, entryId)
    );
}
