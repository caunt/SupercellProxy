using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Sends an Amazon receipt for verification.</summary>
/// <param name="TransactionId">Amazon's identifier of the purchase, which keeps the same purchase from being credited twice.</param>
/// <param name="ProductId">Store product that was bought.</param>
/// <param name="CurrencyCode">Currency the player paid in.</param>
/// <param name="Price">Price the store showed the player, as text.</param>
/// <param name="PurchaseData">Receipt data from Amazon that the server checks with Amazon.</param>
/// <param name="Signature">Signature that comes with the receipt data, if Amazon supplied one.</param>
/// <param name="Value0">A value native reads only when bytes remain.</param>
/// <param name="Value1">A value native reads only when bytes remain.</param>
/// <param name="Value2">A value native reads only when bytes remain.</param>
public sealed record VerifyAmazonBillingReceiptMessage(
    string? TransactionId,
    string? ProductId,
    string? CurrencyCode,
    string? Price,
    string? PurchaseData,
    string? Signature,
    int? Value0,
    int? Value1,
    int? Value2
) : CodecMessage<VerifyAmazonBillingReceiptMessage>, ICodecProvider<VerifyAmazonBillingReceiptMessage>
{
    /// <inheritdoc />
    public static ICodec<VerifyAmazonBillingReceiptMessage> Codec { get; } = Codecs.Composite(
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
        static (transactionId, productId, currencyCode, price, purchaseData, signature, value0, value1, value2) => new VerifyAmazonBillingReceiptMessage(transactionId, productId, currencyCode, price, purchaseData, signature, value0, value1, value2)
    );
}
