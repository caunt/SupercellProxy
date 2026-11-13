using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Avatars;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">OtherHomeDataMessage</c> protocol message.
/// </summary>
public record OtherHomeDataMessage : CodecMessage<OtherHomeDataMessage>, ICodecProvider<OtherHomeDataMessage>
{
    private const string InvalidTailMessage = "Invalid compressed JSON tail.";

    /// <summary>
    /// Gets the wire codec. A payload that does not parse is kept whole in <see cref="Fallback"/> with the error in <see cref="DecodeError"/>.
    /// </summary>
    /// <value>A codec that reads the rest of the message payload and writes back either the decoded parts in wire order or the <see cref="Fallback"/> bytes unchanged.</value>
    public static ICodec<OtherHomeDataMessage> Codec { get; } = Codecs.Create(static stream => FromPayload(stream.ReadToEnd(), stream.GameVersion), static (stream, message) => message.WritePayload(stream));
    /// <summary>
    /// Gets or sets the visiting player's own avatar, sent along with the visited farm.
    /// </summary>
    /// <value>The third field on the wire, after the owner's avatar and the timestamp; <see langword="null"/> only when decoding failed, and writing a message without it throws.</value>
    public ClientAvatar? ClientAvatar { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedAvatarDataJson</c> value.
    /// </summary>
    /// <value>The visiting player's avatar data document as raw bytes: a little-endian inflated length followed by a zlib stream of JSON; <see langword="null"/> when the wire marks it absent.</value>
    [JsonPropertyName("CompressedAvatarDataJson")]
    public ReadOnlyMemory<byte>? CompressedAvatarDataDocument { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedHomeDataJson</c> value.
    /// </summary>
    /// <value>The visited farm's home document as raw bytes in the same length-plus-zlib form, the last field on the wire; it is kept compressed here and decoded by the caller.</value>
    [JsonPropertyName("CompressedHomeDataJson")]
    public ReadOnlyMemory<byte>? CompressedHomeDataDocument { get; init; }
    /// <summary>
    /// Gets or sets the visited home owner's compressed avatar data document; own homes send none.
    /// </summary>
    /// <value>The owner's avatar data document as raw bytes in the length-plus-zlib form, sent between the two other documents; <see langword="null"/> when the wire marks it absent.</value>
    public ReadOnlyMemory<byte>? CompressedHomeOwnerAvatarDataDocument { get; init; }

    /// <summary>
    /// Gets or sets why decoding failed, when it did; the payload is then kept whole in <see cref="Fallback"/>. Null after a successful decode.
    /// </summary>
    /// <value>The end-of-stream, invalid-data, argument or overflow exception the decoder raised, or <see langword="null"/> when every field decoded.</value>
    public Exception? DecodeError { get; init; }

    /// <summary>
    /// Gets or sets the whole payload when it could not be decoded, so it can still be forwarded unchanged; empty after a successful decode.
    /// </summary>
    /// <value>The undecoded message bytes; when not empty, they are written back verbatim instead of the decoded fields.</value>
    public ReadOnlyMemory<byte> Fallback { get; init; }

    /// <summary>
    /// Gets or sets the avatar of the farm's owner: name, ids, roadside shop, inventory and so on.
    /// </summary>
    /// <value>The first field on the wire; <see langword="null"/> only when decoding failed, and writing a message without it throws.</value>
    public ClientAvatar? HomeOwnerAvatar { get; init; }

    /// <summary>
    /// Gets or sets the server timestamp of the snapshot.
    /// </summary>
    /// <value>Server time in Unix seconds, sent right after the owner's avatar; 0 when decoding failed.</value>
    public int ServerTimestamp { get; init; }

    /// <summary>
    /// Gets the full payload as this message would be written back, re-encoded from its decoded parts (or the fallback bytes).
    /// </summary>
    /// <value>A fresh copy of the encoded payload, built on every read; it equals <see cref="Fallback"/> when decoding failed.</value>
    public ReadOnlyMemory<byte> UnknownData
    {
        get
        {
            MessageStream stream = MessageStream.Create();

            try
            {
                WritePayload(stream);

                return stream.ToArray();
            }
            finally
            {
                stream.Dispose();
            }
        }
    }

    private static ICodec<OtherHomeDataMessage> Payload => field ??= Codecs.Complete(Codecs.Composite(ClientAvatar.Codec,
            static message => message.HomeOwnerAvatar ?? throw new InvalidOperationException($"{nameof(HomeOwnerAvatar)} is null."),
            Codecs.VarInt,
            static message => message.ServerTimestamp,
            ClientAvatar.Codec,
            static message => message.ClientAvatar ?? throw new InvalidOperationException($"{nameof(ClientAvatar)} is null."),
            CompressedDocumentCodec(),
            static message => message.CompressedAvatarDataDocument,
            CompressedDocumentCodec(),
            static message => message.CompressedHomeOwnerAvatarDataDocument,
            CompressedDocumentCodec(),
            static message => message.CompressedHomeDataDocument,
            static (homeOwnerAvatar,
                serverTimestamp,
                clientAvatar,
                compressedAvatarDataDocument,
                compressedHomeOwnerAvatarDataDocument,
                compressedHomeDataDocument) => new OtherHomeDataMessage
                {
                    HomeOwnerAvatar = homeOwnerAvatar,
                    ServerTimestamp = serverTimestamp,
                    ClientAvatar = clientAvatar,
                    CompressedAvatarDataDocument = compressedAvatarDataDocument,
                    CompressedHomeOwnerAvatarDataDocument = compressedHomeOwnerAvatarDataDocument,
                    CompressedHomeDataDocument = compressedHomeDataDocument,
                }),
        InvalidTailMessage);

    /// <summary>
    /// Parses a visited-home payload, keeping it whole as <see cref="Fallback"/> when it does not parse.
    /// </summary>
    internal static OtherHomeDataMessage FromPayload(ReadOnlyMemory<byte> data, Version? gameVersion)
    {
        MessageStream stream = MessageStream.Create(data);

        try
        {
            stream.GameVersion = gameVersion;

            return Payload.Read(stream);
        }
        catch (Exception exception) when (exception is EndOfStreamException or InvalidDataException or ArgumentException or OverflowException)
        {
            return new OtherHomeDataMessage { DecodeError = exception, Fallback = data };
        }
        finally
        {
            stream.Dispose();
        }
    }

    private static ICodec<ReadOnlyMemory<byte>?> CompressedDocumentCodec()
    {
        return Codecs.Map(Codecs.OptionalByteArray,
            static document => CompressedDocumentValidator.IsValid(document) ? document : throw new InvalidDataException(InvalidTailMessage),
            static document => document);
    }

    private void WritePayload(ProtocolEncoder encoder)
    {
        if (Fallback.IsEmpty)
            Payload.Write(encoder, this);
        else
            encoder.Write(Fallback.Span);
    }
}
