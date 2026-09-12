using System.Globalization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// <para>Requests the player's own home data.</para>
/// </summary>
public sealed record RequestOwnHomeMessage : IMessage
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
    /// Creates a <c language="csharp">RequestOwnHomeMessage</c> from the supplied data.
    /// </summary>
    public static RequestOwnHomeMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;

        RequestOwnHomeMessage message = new()
        {
            Unknown0 = stream.ReadVariableInt(),
            UnknownString0 = stream.ReadString(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unexpected trailing {nameof(RequestOwnHomeMessage)} data at position {stream.Position} of {stream.Length}."
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

        stream.WriteVariableInt(Unknown0);
        stream.WriteString(UnknownString0);

        return new MessageContainer(identifier, version, stream);
    }
}
