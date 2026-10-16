using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// A protocol message that can be sent: every serverbound and clientbound message type implements it, and the message registry maps
/// each type to its wire id.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Encodes this message's payload into the supplied stream.
    /// </summary>
    void Encode(MessageStream stream);
}
