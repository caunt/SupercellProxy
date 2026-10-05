using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// Carries the runtime mode selected by clientbound message 22302.
public sealed record Clientbound22302Message : IMessage
{
    /// Gets the selected runtime mode.
    public int Mode { get; init; } = -1;

    /// Decodes clientbound message 22302.
    public static Clientbound22302Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        Clientbound22302Message message = new() { Mode = stream.ReadVarInt() };

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
