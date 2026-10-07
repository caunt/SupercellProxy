using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded neighborhood notification stream.
public sealed record NeighborhoodNotificationStreamMessage : IMessage
{
    /// Gets the encoded collection while its inner entry schema remains unconfirmed.
    public Memory<byte> EntryCollectionData { get; init; }

    /// Decodes clientbound message 20621 without inventing inner entry fields.
    public static NeighborhoodNotificationStreamMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new NeighborhoodNotificationStreamMessage { EntryCollectionData = stream.ReadToEnd() };
    }

    /// Encodes clientbound message 20621.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.Write(EntryCollectionData.Span);
    }
}
