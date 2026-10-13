using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>Reads and writes one value in the native binary wire format.</summary>
/// <typeparam name="TValue">The value the codec carries.</typeparam>
public interface ICodec<TValue> : IEncoder<TValue>
{
    /// <summary>Reads one value from the stream.</summary>
    TValue Read(MessageStream stream);
}
