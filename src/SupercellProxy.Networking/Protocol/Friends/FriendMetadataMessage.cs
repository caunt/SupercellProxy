using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Friends;

/// Carries an opaque sequence of ten-byte friend-meta records.
public sealed record FriendMetadataMessage : IMessage
{
    private const int RecordSize = 10;

    /// Gets the encoded friend-meta records without inventing an unconfirmed inner schema.
    public Memory<byte> FriendMetaRecords { get; init; }

    /// Decodes the byte-counted friend-meta sequence.
    public static FriendMetadataMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        byte count = container.Payload.ReadByte();
        byte[] records = container.Payload.ReadBytes(checked(count * RecordSize));

        return new FriendMetadataMessage { FriendMetaRecords = records };
    }

    /// Encodes the byte-counted friend-meta sequence.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        if (FriendMetaRecords.Length % RecordSize is not 0)
            throw new InvalidDataException(message: "Friend-meta data is not aligned to ten-byte records.");

        int count = FriendMetaRecords.Length / RecordSize;

        if (count > byte.MaxValue)
            throw new InvalidDataException(message: "Friend-meta record count exceeds one byte.");

        using MessageStream stream = MessageStream.Create();

        stream.WriteByte(byte.CreateChecked(count));
        stream.Write(FriendMetaRecords.Span);

        return new MessageContainer(identifier, version, stream);
    }
}
