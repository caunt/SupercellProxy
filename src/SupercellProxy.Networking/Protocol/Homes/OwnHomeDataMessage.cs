using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Avatars;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// <para>OwnHomeDataMessage (24180) authoritative home snapshot.</para>
/// </summary>
public sealed record OwnHomeDataMessage : CodecMessage<OwnHomeDataMessage>, ICodecProvider<OwnHomeDataMessage>
{
    /// <inheritdoc />
    public static ICodec<OwnHomeDataMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.ServerTimestamp,
            ClientAvatar.Codec,
            static message => message.ClientAvatar,
            DocumentCodec(),
            static message => message.CompressedAvatarDataDocument,
            DocumentCodec(),
            static message => message.CompressedHomeOwnerAvatarDataDocument,
            Codecs.Map<ReadOnlyMemory<byte>?, ReadOnlyMemory<byte>?>(
                DocumentCodec(),
                static document => document ?? throw new InvalidDataException($"{nameof(OwnHomeDataMessage)} has no home data."),
                static document => document
            ),
            static message => message.CompressedHomeDataDocument,
            static (serverTimestamp, clientAvatar, compressedAvatarDataDocument, compressedHomeOwnerAvatarDataDocument, compressedHomeDataDocument) => new OwnHomeDataMessage
            {
                ServerTimestamp = serverTimestamp,
                ClientAvatar = clientAvatar,
                CompressedHomeOwnerAvatarDataDocument = compressedHomeOwnerAvatarDataDocument,
                CompressedAvatarDataDocument = compressedAvatarDataDocument,
                CompressedHomeDataDocument = compressedHomeDataDocument,
                AvatarData = compressedAvatarDataDocument is null
                    ? new AvatarDataSnapshot()
                    : CompressedDocument.Deserialize<AvatarDataSnapshot>(compressedAvatarDataDocument.Value),
                Home = CompressedDocument.Deserialize<HomeSnapshot>(compressedHomeDataDocument.GetValueOrDefault()),
            }
        ),
        $"Unexpected trailing {nameof(OwnHomeDataMessage)} data."
    );

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
    /// Gets or sets the <c language="csharp">Home</c> value.
    /// </summary>
    public HomeSnapshot Home { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerTimestamp</c> value.
    /// </summary>
    public int ServerTimestamp { get; init; }

    private static ICodec<ReadOnlyMemory<byte>?> DocumentCodec()
    {
        return Codecs.Create(ReadByteArray, WriteByteArray);
    }

    private static ReadOnlyMemory<byte>? ReadByteArray(MessageStream stream)
    {
        int length = stream.ReadInt32();

        return length is -1
            ? null
            : length < 0 || length > stream.Length - stream.Position
            ? throw new InvalidDataException(message: "Invalid byte array length.")
            : (ReadOnlyMemory<byte>?)stream.ReadExactly(new byte[length]).ToArray();
    }

    private static void WriteByteArray(ProtocolEncoder encoder, ReadOnlyMemory<byte>? data)
    {
        if (data is null)
        {
            encoder.WriteInt32(value: -1);

            return;
        }

        encoder.WriteByteArray(data.Value.Span);
    }
}
