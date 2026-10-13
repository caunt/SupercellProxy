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
    public static ICodec<OtherHomeDataMessage> Codec { get; } = Codecs.Create(static stream => FromPayload(stream.ReadToEnd(), stream.GameVersion), static (stream, message) => message.WritePayload(stream));
    /// <summary>
    /// Gets or sets the <c language="csharp">ClientAvatar</c> value.
    /// </summary>
    public ClientAvatar? ClientAvatar { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedAvatarDataJson</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("CompressedAvatarDataJson")]
    public ReadOnlyMemory<byte>? CompressedAvatarDataDocument { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedHomeDataJson</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("CompressedHomeDataJson")]
    public ReadOnlyMemory<byte>? CompressedHomeDataDocument { get; init; }
    /// <summary>
    /// Gets the visited home owner's compressed avatar data document; own homes send none.
    /// </summary>
    public ReadOnlyMemory<byte>? CompressedHomeOwnerAvatarDataDocument { get; init; }

    /// <summary>
    /// Gets the Decode Error value.
    /// </summary>
    public Exception? DecodeError { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Fallback</c> value.
    /// </summary>
    public ReadOnlyMemory<byte> Fallback { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeOwnerAvatar</c> value.
    /// </summary>
    public ClientAvatar? HomeOwnerAvatar { get; init; }

    /// <summary>
    /// Gets the server timestamp of the snapshot.
    /// </summary>
    public int ServerTimestamp { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownData</c> value.
    /// </summary>
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

    private static ICodec<OtherHomeDataMessage> Payload => field ??= Codecs.Complete(
        Codecs.Composite(
            ClientAvatar.Codec,
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
            static (
                homeOwnerAvatar,
                serverTimestamp,
                clientAvatar,
                compressedAvatarDataDocument,
                compressedHomeOwnerAvatarDataDocument,
                compressedHomeDataDocument
            ) => new OtherHomeDataMessage
            {
                HomeOwnerAvatar = homeOwnerAvatar,
                ServerTimestamp = serverTimestamp,
                ClientAvatar = clientAvatar,
                CompressedAvatarDataDocument = compressedAvatarDataDocument,
                CompressedHomeOwnerAvatarDataDocument = compressedHomeOwnerAvatarDataDocument,
                CompressedHomeDataDocument = compressedHomeDataDocument,
            }
        ),
        InvalidTailMessage
    );

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
        catch (Exception exception)
            when (exception is EndOfStreamException or InvalidDataException or ArgumentException or OverflowException)
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
        return Codecs.Map(
            Codecs.OptionalByteArray,
            static document => CompressedDocumentValidator.IsValid(document) ? document : throw new InvalidDataException(InvalidTailMessage),
            static document => document
        );
    }

    private void WritePayload(ProtocolEncoder encoder)
    {
        if (Fallback.IsEmpty)
            Payload.Write(encoder, this);
        else
            encoder.Write(Fallback.Span);
    }
}
