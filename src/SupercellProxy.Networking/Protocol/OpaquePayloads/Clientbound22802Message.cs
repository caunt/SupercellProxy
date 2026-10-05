using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded collection envelope from clientbound message 22802.
public sealed record Clientbound22802Message : IMessage
{
    /// Gets the encoded collections while their inner schemas remain unconfirmed.
    public Memory<byte> CollectionData { get; init; }

    /// Decodes clientbound message 22802 without inventing inner collection fields.
    public static Clientbound22802Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new Clientbound22802Message { CollectionData = stream.ReadToEnd() };
    }

    /// Encodes clientbound message 22802.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.Write(CollectionData.Span);
    }
}
