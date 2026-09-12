using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>
/// Represents the <c language="csharp">KeepAliveMessage</c> protocol message.
/// </summary>
public sealed record KeepAliveMessage : IMessage
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
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream supercellStream = MessageStream.Create();

        return new MessageContainer(identifier, version, supercellStream);
    }
}
