using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>Writes one value to a <see cref="ProtocolEncoder"/>, such as the wire bytes of a <see cref="MessageStream"/>.</summary>
/// <typeparam name="TValue">The value the encoder writes.</typeparam>
public interface IEncoder<in TValue>
{
    /// <summary>Writes one value.</summary>
    /// <param name="encoder">The target that receives the encoded value, appended after whatever it already holds; its game version and command environment can steer versioned layouts.</param>
    /// <param name="value">The value to encode; whether <see langword="null"/> or empty values are accepted depends on the implementation.</param>
    void Write(ProtocolEncoder encoder, TValue value);
}
