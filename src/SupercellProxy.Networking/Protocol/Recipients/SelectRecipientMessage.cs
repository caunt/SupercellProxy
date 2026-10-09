using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Recipients;

/// <summary>Sends the selected recipient's identifier.</summary>
public sealed record SelectRecipientMessage(LongId RecipientId) : IMessage
{
    /// <summary>Decodes the selected recipient.</summary>
    public static SelectRecipientMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        SelectRecipientMessage result = new(stream.ReadLongId());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The recipient request has trailing data.")
            : result;
    }

    /// <summary>Encodes the selected recipient.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(RecipientId);
    }

    /// <summary>Omits the private recipient identifier from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(SelectRecipientMessage);
    }
}
