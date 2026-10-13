using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Writers;

/// <summary>Writes a value through an explicit function.</summary>
internal sealed class DelegateEncoder<TValue>(Action<ProtocolEncoder, TValue> write) : IEncoder<TValue>
{
    public void Write(ProtocolEncoder encoder, TValue value)
    {
        write(encoder, value);
    }
}
