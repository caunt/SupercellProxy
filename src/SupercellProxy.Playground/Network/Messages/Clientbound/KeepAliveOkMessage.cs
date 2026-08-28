using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// Represents the <c language="csharp">KeepAliveOkMessage</c> protocol message.
/// </summary>
internal sealed record KeepAliveOkMessage : IMessage
{
    /// <summary>
    /// Creates a <c language="csharp">KeepAliveOkMessage</c> from the supplied data.
    /// </summary>
    public static KeepAliveOkMessage Create(MessageContainer container)
    {
        return new KeepAliveOkMessage();
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        return new MessageContainer(id, version, supercellStream);
    }
}
