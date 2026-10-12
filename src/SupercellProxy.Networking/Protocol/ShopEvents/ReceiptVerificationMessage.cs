using System.Text;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>Reports platform receipt verification independently of the inventory-grant command.</summary>
public sealed record ReceiptVerificationMessage(int ResultCode, string? ReceiptId) : IMessage
{
    private const int MaximumReceiptBytes = 900000;

    /// <summary>Decodes the native result code and optional receipt identifier.</summary>
    public static ReceiptVerificationMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int result = stream.ReadVarInt();
        string? receipt = stream.ReadOptionalString();

        return receipt is not null && Encoding.UTF8.GetByteCount(receipt) > MaximumReceiptBytes
            ? throw new InvalidDataException(message: "The receipt verification identifier exceeds the native limit.")
            : stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The receipt verification response has trailing data.")
            : new ReceiptVerificationMessage(result, receipt);
    }

    /// <inheritdoc />
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(ResultCode);
        stream.WriteOptionalString(ReceiptId);
    }

    /// <summary>Omits the private receipt identifier from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(ReceiptVerificationMessage);
    }
}
