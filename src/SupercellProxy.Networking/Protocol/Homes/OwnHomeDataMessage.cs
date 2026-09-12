using System.Globalization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Avatars;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// <para>OwnHomeDataMessage (24180) authoritative home snapshot.</para>
/// </summary>
public sealed record OwnHomeDataMessage : IMessage
{

    /// <summary>
    /// Gets or sets the <c language="csharp">ClientAvatar</c> value.
    /// </summary>
    public ClientAvatar ClientAvatar { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">AvatarData</c> value.
    /// </summary>
    public AvatarDataSnapshot AvatarData { get; init; } = new();

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
    /// Gets or sets the <c language="csharp">Home</c> value.
    /// </summary>
    public HomeSnapshot Home { get; init; } = new();
    /// <summary>
    /// Gets or sets the <c language="csharp">ServerTimestamp</c> value.
    /// </summary>
    public int ServerTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownCompressedJson</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownCompressedJson")]
    public Memory<byte>? UnknownCompressedDocument { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">OwnHomeDataMessage</c> from the supplied data.
    /// </summary>
    public static OwnHomeDataMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int serverTimestamp = stream.ReadVariableInt();
        ClientAvatar clientAvatar = ClientAvatar.Decode(stream);
        Memory<byte>? compressedAvatarDataDocument = ReadByteArray(stream);
        Memory<byte>? unknownCompressedDocument = ReadByteArray(stream);

        Memory<byte> compressedHomeDataDocument =
            ReadByteArray(stream)
            ?? throw new InvalidDataException($"{nameof(OwnHomeDataMessage)} has no home data.");

        OwnHomeDataMessage message = new()
        {
            ServerTimestamp = serverTimestamp,
            ClientAvatar = clientAvatar,
            UnknownCompressedDocument = unknownCompressedDocument,
            CompressedAvatarDataDocument = compressedAvatarDataDocument,
            CompressedHomeDataDocument = compressedHomeDataDocument,
            AvatarData = compressedAvatarDataDocument is null
                ? new AvatarDataSnapshot()
                : CompressedDocument.Deserialize<AvatarDataSnapshot>(compressedAvatarDataDocument.Value),
            Home = CompressedDocument.Deserialize<HomeSnapshot>(compressedHomeDataDocument),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unexpected trailing {nameof(OwnHomeDataMessage)} data at position {stream.Position} of {stream.Length}."
                )
            )
            : message;
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(ServerTimestamp);
        ClientAvatar.Encode(stream);
        WriteByteArray(stream, CompressedAvatarDataDocument);
        WriteByteArray(stream, UnknownCompressedDocument);
        WriteByteArray(stream, CompressedHomeDataDocument);

        return new MessageContainer(identifier, version, stream);
    }

    private static Memory<byte>? ReadByteArray(MessageStream stream)
    {
        int length = stream.ReadInt32();

        return length is -1
            ? null
            : length < 0 || length > stream.Length - stream.Position
            ? throw new InvalidDataException(message: "Invalid byte array length.")
            : (Memory<byte>?)stream.ReadExactly(new byte[length]).ToArray();
    }

    private static void WriteByteArray(MessageStream stream, Memory<byte>? data)
    {
        if (data is null)
        {
            stream.WriteInt32(value: -1);

            return;
        }

        stream.WriteByteArray(data.Value.Span);
    }
}
