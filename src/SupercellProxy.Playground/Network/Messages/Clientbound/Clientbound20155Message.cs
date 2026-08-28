using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// Carries the decoded scalar value from clientbound message 20155.
internal sealed record Clientbound20155Message : IMessage
{
    /// Gets the decoded signed variable-length value.
    public int Value { get; init; }

    /// Decodes clientbound message 20155.
    public static Clientbound20155Message Create(MessageContainer container)
    {
        return new Clientbound20155Message { Value = container.Payload.ReadVarInt() };
    }

    /// Encodes clientbound message 20155.
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var stream = MessageStream.Create();
        stream.WriteVarInt(Value);
        return new MessageContainer(id, version, stream);
    }
}
