using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// Carries an opaque sequence of ten-byte friend-meta records.
internal sealed record Clientbound26199Message : IMessage
{
    private const int RecordSize = 10;

    /// Gets the encoded friend-meta records without inventing an unconfirmed inner schema.
    public Memory<byte> FriendMetaRecords { get; init; }

    /// Decodes the byte-counted friend-meta sequence.
    public static Clientbound26199Message Create(MessageContainer container)
    {
        var count = container.Payload.ReadByte();
        var records = new byte[checked(count * RecordSize)];
        _ = container.Payload.ReadExactly(records);
        return new Clientbound26199Message { FriendMetaRecords = records };
    }

    /// Encodes the byte-counted friend-meta sequence.
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        if (FriendMetaRecords.Length % RecordSize is not 0)
            throw new InvalidDataException("Friend-meta data is not aligned to ten-byte records.");

        var count = FriendMetaRecords.Length / RecordSize;
        if (count > byte.MaxValue)
            throw new InvalidDataException("Friend-meta record count exceeds one byte.");

        using var stream = MessageStream.Create();
        stream.WriteByte(byte.CreateChecked(count));
        stream.Write(FriendMetaRecords.Span);
        return new MessageContainer(id, version, stream);
    }
}
