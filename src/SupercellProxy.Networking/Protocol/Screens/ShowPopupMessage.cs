using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Screens;

/// <summary>Asks the client to open one of five fixed popups.</summary>
/// <param name="Value">The value.</param>
/// <param name="Popup">The popup to open, from 1 to 5.</param>
public sealed record ShowPopupMessage(int Value, int Popup) : CodecMessage<ShowPopupMessage>, ICodecProvider<ShowPopupMessage>
{
    /// <inheritdoc />
    public static ICodec<ShowPopupMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Value,
        Codecs.VarInt,
        static message => message.Popup,
        static (@value, popup) => new ShowPopupMessage(value, popup)
    );
}
