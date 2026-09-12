using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Avatars;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">OtherHomeDataMessage</c> protocol message.
/// </summary>
public record OtherHomeDataMessage : IMessage
{

    /// <summary>
    /// Gets or sets the <c language="csharp">ClientAvatar</c> value.
    /// </summary>
    public ClientAvatar? ClientAvatar { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedAvatarDataJson</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("CompressedAvatarDataJson")]
    public Memory<byte>? CompressedAvatarDataDocument { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedHomeDataJson</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("CompressedHomeDataJson")]
    public Memory<byte>? CompressedHomeDataDocument { get; init; }
    /// <summary>
    /// Gets the Decode Error value.
    /// </summary>
    public Exception? DecodeError { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Fallback</c> value.
    /// </summary>
    public Memory<byte> Fallback { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeOwnerAvatar</c> value.
    /// </summary>
    public ClientAvatar? HomeOwnerAvatar { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownCompressedJson</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownCompressedJson")]
    public Memory<byte>? UnknownCompressedDocument { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownData</c> value.
    /// </summary>
    public Memory<byte> UnknownData
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

    /// <summary>
    /// Creates a <c language="csharp">OtherHomeDataMessage</c> from the supplied data.
    /// </summary>
    public static OtherHomeDataMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return Decode(container.Payload.ReadToEnd());
    }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static OtherHomeDataMessage Decode(Memory<byte> data)
    {
        return TryDecode(data, out OtherHomeDataMessage? message) ? message : message with { Fallback = data };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        MessageStream stream = MessageStream.Create();

        try
        {
            WritePayload(stream);

            return new MessageContainer(identifier, version, stream);
        }
        finally
        {
            stream.Dispose();
        }
    }

    private static bool TryDecode(ReadOnlyMemory<byte> data, out OtherHomeDataMessage message)
    {
        try
        {
            MessageStream stream = MessageStream.Create(data);

            try
            {
                ClientAvatar homeOwnerAvatar = ClientAvatar.Decode(stream);
                int unknown0 = stream.ReadVariableInt();
                ClientAvatar clientAvatar = ClientAvatar.Decode(stream);
                Memory<byte>? compressedAvatarDataDocument = stream.ReadOptionalByteArray();
                Memory<byte>? unknownCompressedDocument = stream.ReadOptionalByteArray();
                Memory<byte>? compressedHomeDataDocument = stream.ReadOptionalByteArray();

                if (stream.Position != stream.Length || !CompressedDocumentValidator.IsValid(unknownCompressedDocument))
                    throw new InvalidDataException(message: "Invalid compressed JSON tail.");

                if (!CompressedDocumentValidator.IsValid(compressedAvatarDataDocument))
                    throw new InvalidDataException(message: "Invalid compressed JSON tail.");

                if (!CompressedDocumentValidator.IsValid(compressedHomeDataDocument))
                    throw new InvalidDataException(message: "Invalid compressed JSON tail.");

                message = new OtherHomeDataMessage
                {
                    HomeOwnerAvatar = homeOwnerAvatar,
                    Unknown0 = unknown0,
                    ClientAvatar = clientAvatar,
                    UnknownCompressedDocument = unknownCompressedDocument,
                    CompressedAvatarDataDocument = compressedAvatarDataDocument,
                    CompressedHomeDataDocument = compressedHomeDataDocument,
                };

                return true;
            }
            finally
            {
                stream.Dispose();
            }
        }
        catch (Exception exception)
            when (exception is EndOfStreamException or InvalidDataException or ArgumentException or OverflowException)
        {
            message = new OtherHomeDataMessage { DecodeError = exception };

            return false;
        }
    }

    private void WritePayload(MessageStream stream)
    {
        if (!Fallback.IsEmpty)
        {
            stream.Write(Fallback.Span);

            return;
        }

        (
            HomeOwnerAvatar
            ?? throw new InvalidOperationException($"{nameof(HomeOwnerAvatar)} is null.")
        ).Encode(stream);
        stream.WriteVariableInt(Unknown0);
        (
            ClientAvatar ?? throw new InvalidOperationException($"{nameof(ClientAvatar)} is null.")
        ).Encode(stream);
        stream.WriteOptionalByteArray(CompressedAvatarDataDocument);
        stream.WriteOptionalByteArray(UnknownCompressedDocument);
        stream.WriteOptionalByteArray(CompressedHomeDataDocument);
    }
}
