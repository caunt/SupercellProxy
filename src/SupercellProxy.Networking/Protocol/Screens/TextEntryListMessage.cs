using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Screens;

/// <summary>Delivers a list of text entries the message manager keeps for the HUD.</summary>
/// <param name="Entries">The entries.</param>
public sealed record TextEntryListMessage(ReadOnlyMemory<TextEntry>? Entries) : CodecMessage<TextEntryListMessage>, ICodecProvider<TextEntryListMessage>
{
    /// <inheritdoc />
    public static ICodec<TextEntryListMessage> Codec { get; } = Codecs.Composite(Codecs.NullableMemory(TextEntry.Codec), static message => message.Entries, static (entries) => new TextEntryListMessage(entries));
}
