using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A value without wire fields.</summary>
internal sealed class UnitCodec<TValue>(Func<TValue> factory) : ICodec<TValue>
{
    public TValue Read(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return factory();
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        ArgumentNullException.ThrowIfNull(encoder);
    }
}
