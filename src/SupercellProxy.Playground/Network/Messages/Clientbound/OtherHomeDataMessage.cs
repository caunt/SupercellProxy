using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Network.Messages.Validation;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// Represents the <c>OtherHomeDataMessage</c> protocol message.
/// </summary>
public record OtherHomeDataMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c>HomeOwnerAvatar</c> value.
    /// </summary>
    public ClientAvatar? HomeOwnerAvatar { get; init; }

    /// <summary>
    /// Gets or sets the <c>Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c>ClientAvatar</c> value.
    /// </summary>
    public ClientAvatar? ClientAvatar { get; init; }

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
    /// Gets or sets the <c>Fallback</c> value.
    /// </summary>
    public Memory<byte> Fallback { get; init; }

    /// <summary>
    /// Gets the <c>UnknownData</c> value.
    /// </summary>
    public Memory<byte> UnknownData
    {
        get
        {
            using var stream = MessageStream.Create();
            WritePayload(stream);
            return stream.ToArray();
        }
    }

    /// <summary>
    /// Creates a <c>OtherHomeDataMessage</c> from the supplied data.
    /// </summary>
    public static OtherHomeDataMessage Create(MessageContainer container) =>
        Decode(container.Payload.ReadToEnd());

    internal static OtherHomeDataMessage Decode(Memory<byte> data)
    {
        return TryDecode(data, out var message)
            ? message
            : new OtherHomeDataMessage { Fallback = data };
    }

    private static bool TryDecode(Memory<byte> data, out OtherHomeDataMessage message)
    {
        try
        {
            var stream = new MessageStream(new MemoryStream(data.ToArray()));
            var homeOwnerAvatar = ClientAvatar.Decode(stream);
            var unknown0 = stream.ReadVarInt();
            var clientAvatar = ClientAvatar.Decode(stream);
            var compressedAvatarDataJson = stream.ReadOptionalByteArray();
            var unknownCompressedJson = stream.ReadOptionalByteArray();
            var compressedHomeDataJson = stream.ReadOptionalByteArray();

            if (
                stream.Position != stream.Length
                || !CompressedJsonPayloadValidator.IsValid(unknownCompressedJson)
                || !CompressedJsonPayloadValidator.IsValid(compressedAvatarDataJson)
                || !CompressedJsonPayloadValidator.IsValid(compressedHomeDataJson)
            )
            {
                throw new InvalidDataException("Invalid compressed JSON tail.");
            }

            message = new OtherHomeDataMessage
            {
                HomeOwnerAvatar = homeOwnerAvatar,
                Unknown0 = unknown0,
                ClientAvatar = clientAvatar,
                UnknownCompressedJson = unknownCompressedJson,
                CompressedAvatarDataJson = compressedAvatarDataJson,
                CompressedHomeDataJson = compressedHomeDataJson,
            };
            return true;
        }
        catch (Exception exception)
            when (exception
                    is EndOfStreamException
                        or InvalidDataException
                        or ArgumentException
                        or OverflowException
            )
        {
            message = new OtherHomeDataMessage();
            return false;
        }
    }

    /// <summary>
    /// Executes the <c>ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var stream = MessageStream.Create();
        WritePayload(stream);
        return new MessageContainer(id, version, stream);
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
        stream.WriteVarInt(Unknown0);
        (
            ClientAvatar ?? throw new InvalidOperationException($"{nameof(ClientAvatar)} is null.")
        ).Encode(stream);
        stream.WriteOptionalByteArray(CompressedAvatarDataJson);
        stream.WriteOptionalByteArray(UnknownCompressedJson);
        stream.WriteOptionalByteArray(CompressedHomeDataJson);
    }
}
