using System.Text.Json.Serialization;

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
    public static ICodec<OwnHomeDataMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
            static message => message.ServerTimestamp,
            ClientAvatar.Codec,
            static message => message.ClientAvatar,
            DocumentCodec(),
            static message => message.CompressedAvatarDataDocument,
            DocumentCodec(),
            static message => message.CompressedHomeOwnerAvatarDataDocument,
            Codecs.Map<ReadOnlyMemory<byte>?, ReadOnlyMemory<byte>?>(DocumentCodec(),
                static document => document ?? throw new InvalidDataException($"{nameof(OwnHomeDataMessage)} has no home data."),
                static document => document),
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
            }),
        $"Unexpected trailing {nameof(OwnHomeDataMessage)} data.");

    /// <summary>
    /// Gets or sets the player's own avatar: name, ids, inventory, mail, roadside shop and so on.
    /// </summary>
    /// <value>The avatar decoded right after <see cref="ServerTimestamp"/> on the wire; an empty avatar by default.</value>
    public ClientAvatar ClientAvatar { get; init; } = new();

    /// <summary>
    /// Gets or sets the decoded avatar data document: per-area object metadata and the saved state of account-wide game systems.
    /// </summary>
    /// <value>The JSON of <see cref="CompressedAvatarDataDocument"/> after inflating it, or an empty snapshot when the message carries no avatar data document.</value>
    public AvatarDataSnapshot AvatarData { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedAvatarDataJson</c> value.
    /// </summary>
    /// <value>The raw document bytes: a little-endian inflated length followed by a zlib stream of JSON. <see langword="null"/> when the wire length is -1, which leaves <see cref="AvatarData"/> empty.</value>
    [JsonPropertyName("CompressedAvatarDataJson")]
    public ReadOnlyMemory<byte>? CompressedAvatarDataDocument { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedHomeDataJson</c> value.
    /// </summary>
    /// <value>The raw home document bytes in the same length-plus-zlib form, decoded into <see cref="Home"/>; decoding fails when the message carries none.</value>
    [JsonPropertyName("CompressedHomeDataJson")]
    public ReadOnlyMemory<byte>? CompressedHomeDataDocument { get; init; }

    /// <summary>
    /// Gets or sets the visited home owner's compressed avatar data document; own homes send none.
    /// </summary>
    /// <value>The owner's raw document bytes in the length-plus-zlib form, kept undecoded; <see langword="null"/> when the wire length is -1, as it is for the player's own home.</value>
    public ReadOnlyMemory<byte>? CompressedHomeOwnerAvatarDataDocument { get; init; }

    /// <summary>
    /// Gets or sets the decoded home data document: the farm's objects, tile size and running product counts.
    /// </summary>
    /// <value>The inflated JSON of <see cref="CompressedHomeDataDocument"/>; an empty home by default.</value>
    public HomeSnapshot Home { get; init; } = new();

    /// <summary>
    /// Gets or sets the server time of the snapshot in Unix seconds.
    /// </summary>
    /// <value>Seconds since 1970-01-01 UTC, the first field on the wire; 0 by default.</value>
    public int ServerTimestamp { get; init; }

    private static ICodec<ReadOnlyMemory<byte>?> DocumentCodec()
    {
        return Codecs.Create(ReadByteArray, WriteByteArray);
    }

    private static ReadOnlyMemory<byte>? ReadByteArray(MessageChannel stream)
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
