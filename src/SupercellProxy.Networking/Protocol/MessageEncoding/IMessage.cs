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
    /// <param name="stream">
    /// The payload stream to write into, positioned where the payload starts; its game version, when set, selects the release layout. The
    /// frame header is not written here.
    /// </param>
    void Encode(MessageStream stream);
}
