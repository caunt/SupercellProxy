using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>
/// Represents the <c language="csharp">KeepAliveOkMessage</c> protocol message.
/// </summary>
public sealed record KeepAliveOkMessage : IMessage
{
    /// <summary>
    /// Creates a <c language="csharp">KeepAliveOkMessage</c> from the supplied data.
    /// </summary>
    public static KeepAliveOkMessage Create(MessageContainer container)
    {
        return new KeepAliveOkMessage();
    }

    /// <summary>
    /// Executes the <c language="csharp">ToStream</c> operation.
    /// </summary>
    public MessageStream ToStream()
    {
        using MessageStream supercellStream = MessageStream.Create();

        return supercellStream;
    }
}
