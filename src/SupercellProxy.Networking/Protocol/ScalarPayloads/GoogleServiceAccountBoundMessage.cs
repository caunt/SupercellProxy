using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// Carries the Google service account binding result.
public sealed record GoogleServiceAccountBoundMessage : IMessage
{
    /// Gets the binding result code.
    public int Mode { get; init; } = -1;

    /// Decodes clientbound message 22302.
    public static GoogleServiceAccountBoundMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        GoogleServiceAccountBoundMessage message = new() { Mode = stream.ReadVarInt() };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "Clientbound message 22302 has trailing data.")
            : message;
    }

    /// Encodes clientbound message 22302.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Mode);
    }
}
