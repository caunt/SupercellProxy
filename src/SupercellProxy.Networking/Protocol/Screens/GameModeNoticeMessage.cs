using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Screens;

/// <summary>Stores a pending notice of kind 1 to 3 with its text on the current game mode.</summary>
/// <param name="Text">The text.</param>
/// <param name="Kind">The kind.</param>
public sealed record GameModeNoticeMessage(string? Text, int Kind) : CodecMessage<GameModeNoticeMessage>, ICodecProvider<GameModeNoticeMessage>
{
    /// <inheritdoc />
    public static ICodec<GameModeNoticeMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Text,
        Codecs.VarInt,
        static message => message.Kind,
        static (text, kind) => new GameModeNoticeMessage(text, kind)
    );
}
