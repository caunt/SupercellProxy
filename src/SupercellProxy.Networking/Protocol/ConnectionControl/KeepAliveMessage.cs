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
    public static KeepAliveMessage Decode(MessageStream stream)
    {
        return new KeepAliveMessage();
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
    }
}
