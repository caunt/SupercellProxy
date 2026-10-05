using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// Carries the recipient flag and value from clientbound message 29734.
public sealed record Clientbound29734Message : IMessage
{
    /// Gets the recipient flag.
    public bool Flag { get; init; }

    /// Gets the recipient value.
    public int Value { get; init; }

    /// Decodes clientbound message 29734.
    public static Clientbound29734Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        Clientbound29734Message message = new()
        {
            Flag = stream.ReadBoolean(),
            Value = stream.ReadVarInt(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "Clientbound message 29734 has trailing data.")
            : message;
    }

    /// Encodes clientbound message 29734.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteBoolean(Flag);
        stream.WriteVarInt(Value);
    }
}
