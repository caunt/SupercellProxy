using System.Globalization;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Json;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// <para>OwnHomeDataMessage (24180) authoritative home snapshot.</para>
/// </summary>
public sealed record OwnHomeDataMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c>ServerTimestamp</c> value.
    /// </summary>
    public int ServerTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c>ClientAvatar</c> value.
    /// </summary>
    public ClientAvatar ClientAvatar { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c>UnknownCompressedJson</c> value.
    /// </summary>
    public Memory<byte>? UnknownCompressedJson { get; init; }

    /// <summary>
    /// Gets or sets the <c>CompressedAvatarDataJson</c> value.
    /// </summary>
    public Memory<byte>? CompressedAvatarDataJson { get; init; }

    /// <summary>
    /// Gets or sets the <c>CompressedHomeDataJson</c> value.
    /// </summary>
    public Memory<byte>? CompressedHomeDataJson { get; init; }

    /// <summary>
    /// Gets or sets the <c>AvatarData</c> value.
    /// </summary>
    public AvatarDataSnapshot AvatarData { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c>Home</c> value.
    /// </summary>
    public HomeSnapshot Home { get; init; } = new();

    /// <summary>
    /// Creates a <c>OwnHomeDataMessage</c> from the supplied data.
    /// </summary>
    public static OwnHomeDataMessage Create(MessageContainer container)
    {
        var stream = container.Payload;
        var serverTimestamp = stream.ReadVarInt();
        var clientAvatar = ClientAvatar.Decode(stream);
        var compressedAvatarDataJson = ReadByteArray(stream);
        var unknownCompressedJson = ReadByteArray(stream);
        var compressedHomeDataJson =
            ReadByteArray(stream)
            ?? throw new InvalidDataException($"{nameof(OwnHomeDataMessage)} has no home data.");
        var message = new OwnHomeDataMessage
        {
            ServerTimestamp = serverTimestamp,
            ClientAvatar = clientAvatar,
            UnknownCompressedJson = unknownCompressedJson,
            CompressedAvatarDataJson = compressedAvatarDataJson,
            CompressedHomeDataJson = compressedHomeDataJson,
            AvatarData = compressedAvatarDataJson is null
                ? new AvatarDataSnapshot()
                : CompressedJson.Deserialize<AvatarDataSnapshot>(compressedAvatarDataJson.Value),
            Home = CompressedJson.Deserialize<HomeSnapshot>(compressedHomeDataJson),
        };

        if (stream.Position != stream.Length)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unexpected trailing {nameof(OwnHomeDataMessage)} data at position {stream.Position} of {stream.Length}."
                )
            );

        return message;
    }

    /// <summary>
    /// Executes the <c>ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var stream = MessageStream.Create();

        stream.WriteVarInt(ServerTimestamp);
        ClientAvatar.Encode(stream);
        WriteByteArray(stream, CompressedAvatarDataJson);
        WriteByteArray(stream, UnknownCompressedJson);
        WriteByteArray(stream, CompressedHomeDataJson);

        return new MessageContainer(id, version, stream);
    }

    private static Memory<byte>? ReadByteArray(MessageStream stream)
    {
        var length = stream.ReadInt32();

        if (length is -1)
            return null;

        if (length < 0 || length > stream.Length - stream.Position)
            throw new InvalidDataException("Invalid byte array length.");

        return stream.ReadExactly(new byte[length]).ToArray();
    }

    private static void WriteByteArray(MessageStream stream, Memory<byte>? data)
    {
        if (data is null)
        {
            stream.WriteInt32(-1);
            return;
        }

        stream.WriteByteArray(data.Value.Span);
    }
}
