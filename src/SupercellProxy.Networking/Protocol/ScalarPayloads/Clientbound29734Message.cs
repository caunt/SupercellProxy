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
    public static Clientbound29734Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        Clientbound29734Message message = new()
        {
            Flag = container.Payload.ReadBoolean(),
            Value = container.Payload.ReadVariableInt(),
        };

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Clientbound message 29734 has trailing data.")
            : message;
    }

    /// Encodes clientbound message 29734.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteBoolean(Flag);
        stream.WriteVariableInt(Value);

        return new MessageContainer(identifier, version, stream);
    }
}
