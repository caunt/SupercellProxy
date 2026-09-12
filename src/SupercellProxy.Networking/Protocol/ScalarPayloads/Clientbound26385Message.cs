using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// Carries the three restored values from clientbound message 26385.
public sealed record Clientbound26385Message : IMessage
{

    /// Gets the first decoded bit-packed flag.
    public bool FlagA { get; init; }

    /// Gets the second decoded bit-packed flag.
    public bool FlagB { get; init; }
    /// Gets the decoded signed variable-length value.
    public int Value { get; init; }

    /// Decodes clientbound message 26385.
    public static Clientbound26385Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new Clientbound26385Message
        {
            Value = container.Payload.ReadVariableInt(),
            FlagA = container.Payload.ReadBoolean(),
            FlagB = container.Payload.ReadBoolean(),
        };
    }

    /// Encodes clientbound message 26385.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Value);
        stream.WriteBoolean(FlagA);
        stream.WriteBoolean(FlagB);

        return new MessageContainer(identifier, version, stream);
    }
}
