using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded state envelope from clientbound message 23074.
public sealed record Clientbound23074Message : IMessage
{
    /// Gets the encoded state while its nested schemas remain unconfirmed.
    public Memory<byte> StateData { get; init; }

    /// Decodes clientbound message 23074 without inventing nested state fields.
    public static Clientbound23074Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new Clientbound23074Message { StateData = stream.ReadToEnd() };
    }

    /// Encodes clientbound message 23074.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.Write(StateData.Span);
    }
}
