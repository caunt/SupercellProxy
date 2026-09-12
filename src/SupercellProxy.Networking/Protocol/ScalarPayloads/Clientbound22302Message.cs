using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// Carries the runtime mode selected by clientbound message 22302.
public sealed record Clientbound22302Message : IMessage
{
    /// Gets the selected runtime mode.
    public int Mode { get; init; } = -1;

    /// Decodes clientbound message 22302.
    public static Clientbound22302Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        Clientbound22302Message message = new() { Mode = container.Payload.ReadVariableInt() };

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Clientbound message 22302 has trailing data.")
            : message;
    }

    /// Encodes clientbound message 22302.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Mode);

        return new MessageContainer(identifier, version, stream);
    }
}
