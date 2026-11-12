using System.Text;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>Reports platform receipt verification independently of the inventory-grant command.</summary>
/// <param name="ResultCode">The verification outcome the server reports, sent first as a var-int; what each code value means is not yet identified.</param>
/// <param name="ReceiptId">The platform receipt the result is for, or <see langword="null"/> when the server sends none; decoding rejects one longer than 900000 UTF-8 bytes, native's limit.</param>
public sealed record ReceiptVerificationMessage(int ResultCode, string? ReceiptId) : CodecMessage<ReceiptVerificationMessage>, ICodecProvider<ReceiptVerificationMessage>
{
    private const int MaximumReceiptBytes = 900000;

    /// <inheritdoc />
    public static ICodec<ReceiptVerificationMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
            static message => message.ResultCode,
            Codecs.Map(Codecs.OptionalString,
                static receipt => receipt is not null && Encoding.UTF8.GetByteCount(receipt) > MaximumReceiptBytes
                    ? throw new InvalidDataException(message: "The receipt verification identifier exceeds the native limit.")
                    : receipt,
                static receipt => receipt),
            static message => message.ReceiptId,
            static (resultCode, receiptId) => new ReceiptVerificationMessage(resultCode, receiptId)),
        trailingDataMessage: "The receipt verification response has trailing data.");

    /// <summary>Omits the private receipt identifier from diagnostic text.</summary>
    /// <returns>Always the type name <c>ReceiptVerificationMessage</c>, without the result code or receipt, so logs never carry the receipt.</returns>
    public override string ToString()
    {
        return nameof(ReceiptVerificationMessage);
    }
}
