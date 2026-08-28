using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Network.Messages.Validation;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// Represents the <c language="csharp">OtherHomeDataMessage</c> protocol message.
/// </summary>
internal record OtherHomeDataMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">HomeOwnerAvatar</c> value.
    /// </summary>
    public ClientAvatar? HomeOwnerAvatar { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ClientAvatar</c> value.
    /// </summary>
    public ClientAvatar? ClientAvatar { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownCompressedJson</c> value.
    /// </summary>
    public Memory<byte>? UnknownCompressedJson { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedAvatarDataJson</c> value.
    /// </summary>
    public Memory<byte>? CompressedAvatarDataJson { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedHomeDataJson</c> value.
    /// </summary>
    public Memory<byte>? CompressedHomeDataJson { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Fallback</c> value.
    /// </summary>
    public Memory<byte> Fallback { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownData</c> value.
    /// </summary>
    public Memory<byte> UnknownData
    {
        get
        {
            var stream = MessageStream.Create();
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
    public static OtherHomeDataMessage Create(MessageContainer container) =>
        Decode(container.Payload.ReadToEnd());

    internal static OtherHomeDataMessage Decode(Memory<byte> data)
    {
        return TryDecode(data, out var message)
            ? message
            : new OtherHomeDataMessage { Fallback = data };
    }

    private static bool TryDecode(ReadOnlyMemory<byte> data, out OtherHomeDataMessage message)
    {
        try
        {
            var stream = MessageStream.Create(data);
            try
            {
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
            finally
            {
                stream.Dispose();
            }
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
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        var stream = MessageStream.Create();
        try
        {
            WritePayload(stream);
            return new MessageContainer(id, version, stream);
        }
        finally
        {
            stream.Dispose();
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
        stream.WriteVarInt(Unknown0);
        (
            ClientAvatar ?? throw new InvalidOperationException($"{nameof(ClientAvatar)} is null.")
        ).Encode(stream);
        stream.WriteOptionalByteArray(CompressedAvatarDataJson);
        stream.WriteOptionalByteArray(UnknownCompressedJson);
        stream.WriteOptionalByteArray(CompressedHomeDataJson);
    }
}
