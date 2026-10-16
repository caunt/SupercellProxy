using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Preferences;

/// <summary>Confirms the action of a generic popup.</summary>
/// <param name="PopupId">Which popup the player answered.</param>
/// <param name="Choice">Button the player picked.</param>
/// <param name="Price">Price the popup showed, when it asked for payment.</param>
/// <param name="Currency">Money type of that price, as native numbers currencies.</param>
/// <param name="Context">Extra number the popup was opened with; its use depends on the popup.</param>
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
