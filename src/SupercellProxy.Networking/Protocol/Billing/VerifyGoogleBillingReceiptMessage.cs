using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Sends a Google Play receipt for verification.</summary>
/// <param name="TransactionId">Google Play order identifier, which keeps the same purchase from being credited twice.</param>
/// <param name="ProductId">Store product that was bought.</param>
/// <param name="CurrencyCode">Currency the player paid in.</param>
/// <param name="Price">Price the store showed the player, as text.</param>
/// <param name="PurchaseData">Google Play's signed purchase JSON.</param>
/// <param name="Signature">Google Play's signature over the purchase JSON, which the server checks with the app's public key.</param>
/// <param name="Value0">The first trailing value, an Int32 read after the signature only when bytes remain; <see langword="null"/> when the message ends there. Its meaning is not yet identified.</param>
/// <param name="Value1">The second trailing value, an Int32 that follows <paramref name="Value0"/> when bytes still remain; <see langword="null"/> otherwise. Its meaning is not yet identified.</param>
/// <param name="Value2">The third trailing value, a VarInt rather than an Int32, read last when bytes still remain; <see langword="null"/> otherwise. Its meaning is not yet identified.</param>
public sealed record VerifyGoogleBillingReceiptMessage(string? TransactionId,
    string? ProductId,
    string? CurrencyCode,
    string? Price,
    string? PurchaseData,
    string? Signature,
    int? Value0,
    int? Value1,
    int? Value2) : CodecMessage<VerifyGoogleBillingReceiptMessage>, ICodecProvider<VerifyGoogleBillingReceiptMessage>
{
    /// <inheritdoc />
    public static ICodec<VerifyGoogleBillingReceiptMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalUtf8String,
        static message => message.TransactionId,
        Codecs.OptionalUtf8String,
        static message => message.ProductId,
        Codecs.OptionalUtf8String,
        static message => message.CurrencyCode,
        Codecs.OptionalUtf8String,
        static message => message.Price,
        Codecs.OptionalUtf8String,
        static message => message.PurchaseData,
        Codecs.OptionalUtf8String,
        static message => message.Signature,
        Codecs.TrailingValue(Codecs.BigEndianInt32),
        static message => message.Value0,
        Codecs.TrailingValue(Codecs.BigEndianInt32),
        static message => message.Value1,
        Codecs.TrailingValue(Codecs.VarInt),
        static message => message.Value2,
        static (transactionId, productId, currencyCode, price, purchaseData, signature, value0, value1, value2) => new VerifyGoogleBillingReceiptMessage(transactionId, productId, currencyCode, price, purchaseData, signature, value0, value1, value2));
}
