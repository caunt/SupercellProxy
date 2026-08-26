using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// Represents the <c>KeepAliveMessage</c> protocol message.
/// </summary>
public record KeepAliveMessage : IMessage
{
    /// <summary>
    /// Creates a <c>KeepAliveMessage</c> from the supplied data.
    /// </summary>
    public static KeepAliveMessage Create(MessageContainer container)
    {
        return new KeepAliveMessage();
    }

    /// <summary>
    /// Executes the <c>ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        return new MessageContainer(id, version, supercellStream);
    }
}
