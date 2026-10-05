using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Defines the <c language="csharp">IMessage</c> contract.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Encodes this message's payload into the supplied stream.
    /// </summary>
    void Encode(MessageStream stream);
}
