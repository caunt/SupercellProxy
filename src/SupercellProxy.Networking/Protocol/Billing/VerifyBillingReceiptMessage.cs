using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Sends an App Store receipt for verification.</summary>
/// <param name="PriceLocale">The price locale.</param>
/// <param name="LocalPrice">The local price.</param>
/// <param name="TransactionId">The transaction id.</param>
/// <param name="ProductId">The product id.</param>
/// <param name="Receipt">The receipt.</param>
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
