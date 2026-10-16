using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Sends a Supercell ID store receipt for verification.</summary>
/// <param name="ProductId">Store product that was bought.</param>
/// <param name="ReceiptToken">Token the Supercell store issued for the purchase, which the server redeems.</param>
public sealed record VerifySupercellIdBillingReceiptMessage(string? ProductId, string? ReceiptToken) : CodecMessage<VerifySupercellIdBillingReceiptMessage>, ICodecProvider<VerifySupercellIdBillingReceiptMessage>
{
    /// <inheritdoc />
    public static ICodec<VerifySupercellIdBillingReceiptMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.ProductId,
        Codecs.OptionalString,
        static message => message.ReceiptToken,
        static (productId, receiptToken) => new VerifySupercellIdBillingReceiptMessage(productId, receiptToken)
    );
}
