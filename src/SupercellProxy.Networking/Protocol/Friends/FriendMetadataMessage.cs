using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Carries an opaque sequence of ten-byte friend-meta records.
public sealed record FriendMetadataMessage : CodecMessage<FriendMetadataMessage>, ICodecProvider<FriendMetadataMessage>
{
    private const int RecordSize = 10;

    /// <inheritdoc />
    /// <remarks>Native prefixes the ten-byte records with a one-byte record count.</remarks>
    public static ICodec<FriendMetadataMessage> Codec { get; } = Codecs.Create(
        static stream => new FriendMetadataMessage { FriendMetaRecords = stream.ReadBytes(checked(stream.ReadByte() * RecordSize)) },
        static (stream, message) =>
        {
            if (message.FriendMetaRecords.Length % RecordSize is not 0)
                throw new InvalidDataException(message: "Friend-meta data is not aligned to ten-byte records.");

            int count = message.FriendMetaRecords.Length / RecordSize;

            if (count > byte.MaxValue)
                throw new InvalidDataException(message: "Friend-meta record count exceeds one byte.");

            stream.WriteByte(byte.CreateChecked(count));
            stream.Write(message.FriendMetaRecords.Span);
        }
    );

    /// Gets the encoded friend-meta records without inventing an unconfirmed inner schema.
    public ReadOnlyMemory<byte> FriendMetaRecords { get; init; }
}
