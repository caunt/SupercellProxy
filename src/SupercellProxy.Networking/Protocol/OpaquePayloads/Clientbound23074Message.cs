using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded state envelope from clientbound message 23074.
public sealed record Clientbound23074Message : IMessage
{
    /// Gets the encoded state while its nested schemas remain unconfirmed.
    public Memory<byte> StateData { get; init; }

    /// Decodes clientbound message 23074 without inventing nested state fields.
    public static Clientbound23074Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new Clientbound23074Message { StateData = container.Payload.ReadToEnd() };
    }

    /// Encodes clientbound message 23074.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.Write(StateData.Span);

        return new MessageContainer(identifier, version, stream);
    }
}
