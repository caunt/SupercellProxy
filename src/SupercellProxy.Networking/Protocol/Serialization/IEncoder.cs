using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>Writes one value to a <see cref="ProtocolEncoder"/>, such as the wire bytes of a <see cref="MessageStream"/>.</summary>
/// <typeparam name="TValue">The value the encoder writes.</typeparam>
public interface IEncoder<in TValue>
{
    /// <summary>Writes one value.</summary>
    void Write(ProtocolEncoder encoder, TValue value);
}
