using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Recipients;

/// <summary>Acknowledges the most recently selected recipient.</summary>
public sealed record RecipientSelectedMessage : IMessage
{
    /// <summary>Decodes the empty acknowledgement.</summary>
    public static RecipientSelectedMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The recipient acknowledgement has trailing data.")
            : new();
    }

    /// <summary>Encodes the empty acknowledgement.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
    }
}
