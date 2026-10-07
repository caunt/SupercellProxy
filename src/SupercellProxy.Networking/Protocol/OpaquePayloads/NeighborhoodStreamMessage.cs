using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded neighborhood stream.
public sealed record NeighborhoodStreamMessage : IMessage
{
    /// Gets the encoded collection while its inner entry schema remains unconfirmed.
    public Memory<byte> EntryCollectionData { get; init; }

    /// Decodes clientbound message 28061 without inventing inner entry fields.
    public static NeighborhoodStreamMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new NeighborhoodStreamMessage { EntryCollectionData = stream.ReadToEnd() };
    }

    /// Encodes clientbound message 28061.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.Write(EntryCollectionData.Span);
    }
}
