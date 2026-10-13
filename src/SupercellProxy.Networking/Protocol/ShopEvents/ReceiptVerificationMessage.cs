using System.Text;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>Reports platform receipt verification independently of the inventory-grant command.</summary>
public sealed record ReceiptVerificationMessage(int ResultCode, string? ReceiptId) : CodecMessage<ReceiptVerificationMessage>, ICodecProvider<ReceiptVerificationMessage>
{
    private const int MaximumReceiptBytes = 900000;

    /// <inheritdoc />
    public static ICodec<ReceiptVerificationMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.ResultCode,
            Codecs.Map(
                Codecs.OptionalString,
                static receipt => receipt is not null && Encoding.UTF8.GetByteCount(receipt) > MaximumReceiptBytes
                    ? throw new InvalidDataException(message: "The receipt verification identifier exceeds the native limit.")
                    : receipt,
                static receipt => receipt
            ),
            static message => message.ReceiptId,
            static (resultCode, receiptId) => new ReceiptVerificationMessage(resultCode, receiptId)
        ),
        trailingDataMessage: "The receipt verification response has trailing data."
    );

    /// <summary>Omits the private receipt identifier from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(ReceiptVerificationMessage);
    }
}
