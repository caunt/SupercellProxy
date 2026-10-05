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
    public static FriendMetadataMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        byte count = stream.ReadByte();
        byte[] records = stream.ReadBytes(checked(count * RecordSize));

        return new FriendMetadataMessage { FriendMetaRecords = records };
    }

    /// Encodes the byte-counted friend-meta sequence.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (FriendMetaRecords.Length % RecordSize is not 0)
            throw new InvalidDataException(message: "Friend-meta data is not aligned to ten-byte records.");

        int count = FriendMetaRecords.Length / RecordSize;

        if (count > byte.MaxValue)
            throw new InvalidDataException(message: "Friend-meta record count exceeds one byte.");

        stream.WriteByte(byte.CreateChecked(count));
        stream.Write(FriendMetaRecords.Span);
    }
}
