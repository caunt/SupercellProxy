using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>Reads and writes one value in the native binary wire format.</summary>
/// <typeparam name="TValue">The value the codec carries.</typeparam>
public interface ICodec<TValue> : IEncoder<TValue>
{
    /// <summary>Reads one value from the stream.</summary>
    /// <param name="stream">The payload positioned at the first byte of the value; on return its position is just past the bytes the value took, and its game version can steer versioned layouts.</param>
    /// <returns>The value decoded from the bytes at the stream's position, laid out exactly as <see cref="IEncoder{TValue}.Write"/> writes it.</returns>
    TValue Read(MessageStream stream);
}
