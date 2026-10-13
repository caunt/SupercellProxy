using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Sends a Google Play receipt for verification.</summary>
/// <param name="TransactionId">The transaction id.</param>
/// <param name="ProductId">The product id.</param>
/// <param name="CurrencyCode">The currency code.</param>
/// <param name="Price">The price.</param>
/// <param name="PurchaseData">The purchase data.</param>
/// <param name="Signature">The signature.</param>
/// <param name="Value0">A value native reads only when bytes remain.</param>
/// <param name="Value1">A value native reads only when bytes remain.</param>
/// <param name="Value2">A value native reads only when bytes remain.</param>
public sealed record VerifyGoogleBillingReceiptMessage(
    string? TransactionId,
    string? ProductId,
    string? CurrencyCode,
    string? Price,
    string? PurchaseData,
    string? Signature,
    int? Value0,
    int? Value1,
    int? Value2
) : CodecMessage<VerifyGoogleBillingReceiptMessage>, ICodecProvider<VerifyGoogleBillingReceiptMessage>
{
    /// <inheritdoc />
    public static ICodec<VerifyGoogleBillingReceiptMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.TransactionId,
        Codecs.OptionalString,
        static message => message.ProductId,
        Codecs.OptionalString,
        static message => message.CurrencyCode,
        Codecs.OptionalString,
        static message => message.Price,
        Codecs.OptionalString,
        static message => message.PurchaseData,
        Codecs.OptionalString,
        static message => message.Signature,
        Codecs.TrailingValue(Codecs.Int32),
        static message => message.Value0,
        Codecs.TrailingValue(Codecs.Int32),
        static message => message.Value1,
        Codecs.TrailingValue(Codecs.VarInt),
        static message => message.Value2,
        static (transactionId, productId, currencyCode, price, purchaseData, signature, value0, value1, value2) => new VerifyGoogleBillingReceiptMessage(transactionId, productId, currencyCode, price, purchaseData, signature, value0, value1, value2)
    );
}
