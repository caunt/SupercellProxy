using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Writers;

/// <summary>Writes a value through each part in order.</summary>
internal sealed class SequenceEncoder<TValue>(IEncoder<TValue>[] parts) : IEncoder<TValue>
{
    public void Write(ProtocolEncoder encoder, TValue value)
    {
        foreach (IEncoder<TValue> part in parts)
            part.Write(encoder, value);
    }
}
