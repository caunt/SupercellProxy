using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// Carries the decoded scalar value from clientbound message 20155.
public sealed record Clientbound20155Message : IMessage
{
    /// Gets the decoded signed variable-length value.
    public int Value { get; init; }

    /// Decodes clientbound message 20155.
    public static Clientbound20155Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new Clientbound20155Message { Value = container.Payload.ReadVariableInt() };
    }

    /// Encodes clientbound message 20155.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Value);

        return new MessageContainer(identifier, version, stream);
    }
}
