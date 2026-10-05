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
    public static KeepAliveOkMessage Decode(MessageStream stream)
    {
        return new KeepAliveOkMessage();
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
    }
}
