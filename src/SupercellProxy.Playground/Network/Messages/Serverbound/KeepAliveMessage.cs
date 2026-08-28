using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// Represents the <c language="csharp">KeepAliveMessage</c> protocol message.
/// </summary>
internal sealed record KeepAliveMessage : IMessage
{
    /// <summary>
    /// Creates a <c language="csharp">KeepAliveMessage</c> from the supplied data.
    /// </summary>
    public static KeepAliveMessage Create(MessageContainer container)
    {
        return new KeepAliveMessage();
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
