using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Defines the <c language="csharp">IMessage</c> contract.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Executes the <c language="csharp">ToStream</c> operation.
    /// </summary>
    MessageStream ToStream();
}
