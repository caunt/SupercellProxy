using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Preferences;

/// <summary>Confirms the action of a generic popup.</summary>
/// <param name="PopupId">The popup id.</param>
/// <param name="Choice">The choice.</param>
/// <param name="Price">The price.</param>
/// <param name="Currency">The currency.</param>
/// <param name="Context">The context.</param>
public sealed record ConfirmPopupActionMessage(int PopupId, int Choice, int Price, int Currency, int Context) : CodecMessage<ConfirmPopupActionMessage>, ICodecProvider<ConfirmPopupActionMessage>
{
    /// <inheritdoc />
    public static ICodec<ConfirmPopupActionMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.PopupId,
        Codecs.VarInt,
        static message => message.Choice,
        Codecs.VarInt,
        static message => message.Price,
        Codecs.VarInt,
        static message => message.Currency,
        Codecs.VarInt,
        static message => message.Context,
        static (popupId, choice, price, currency, context) => new ConfirmPopupActionMessage(popupId, choice, price, currency, context)
    );
}
