using System.Globalization;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// <para>Reports the client loading state after login.</para>
/// </summary>
internal sealed record ClientLoadingFunnelMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; } = -1;

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public string UnknownString0 { get; init; } = string.Empty;

    /// <summary>
    /// Creates a <c language="csharp">ClientLoadingFunnelMessage</c> from the supplied data.
    /// </summary>
    public static ClientLoadingFunnelMessage Create(MessageContainer container)
    {
        var stream = container.Payload;
        var message = new ClientLoadingFunnelMessage
        {
            Unknown0 = stream.ReadVarInt(),
            UnknownString0 = stream.ReadString(),
        };

        if (stream.Position != stream.Length)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unexpected trailing {nameof(ClientLoadingFunnelMessage)} data at position {stream.Position} of {stream.Length}."
                )
            );

        return message;
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var stream = MessageStream.Create();

        stream.WriteVarInt(Unknown0);
        stream.WriteString(UnknownString0);

        return new MessageContainer(id, version, stream);
    }
}
