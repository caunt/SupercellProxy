using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Sends an App Store receipt for verification.</summary>
/// <param name="PriceLocale">Locale the App Store priced the product in, which gives its currency.</param>
/// <param name="LocalPrice">Price the App Store showed the player, as text.</param>
/// <param name="TransactionId">App Store identifier of the purchase, which keeps the same purchase from being credited twice.</param>
/// <param name="ProductId">Store product that was bought.</param>
/// <param name="Receipt">App Store receipt bytes the server checks with Apple.</param>
public sealed record VerifyBillingReceiptMessage(string? PriceLocale, string? LocalPrice, string? TransactionId, string? ProductId, ReadOnlyMemory<byte>? Receipt) : CodecMessage<VerifyBillingReceiptMessage>, ICodecProvider<VerifyBillingReceiptMessage>
{
    /// <inheritdoc />
    public static ICodec<VerifyBillingReceiptMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.PriceLocale,
        Codecs.OptionalString,
        static message => message.LocalPrice,
        Codecs.OptionalString,
        static message => message.TransactionId,
        Codecs.OptionalString,
        static message => message.ProductId,
        Codecs.OptionalByteArray,
        static message => message.Receipt,
        static (priceLocale, localPrice, transactionId, productId, receipt) => new VerifyBillingReceiptMessage(priceLocale, localPrice, transactionId, productId, receipt)
    );
}
