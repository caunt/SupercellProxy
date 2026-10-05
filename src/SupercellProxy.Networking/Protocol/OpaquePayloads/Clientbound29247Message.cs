using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded composite payload from clientbound message 29247.
public sealed record Clientbound29247Message : IMessage
{
    /// Gets the encoded payload while its nested schema remains unconfirmed.
    public Memory<byte> PayloadData { get; init; }

    /// Decodes clientbound message 29247 without inventing nested payload fields.
    public static Clientbound29247Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new Clientbound29247Message { PayloadData = stream.ReadToEnd() };
    }

    /// Encodes clientbound message 29247.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.Write(PayloadData.Span);
    }
}
