using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>Resolves another codec on first use, which lets codecs refer to each other.</summary>
internal sealed class LazyCodec<TValue>(Func<ICodec<TValue>> factory) : ICodec<TValue>
{
    private readonly Lazy<ICodec<TValue>> _codec = new(factory);

    public TValue Read(MessageStream stream)
    {
        return _codec.Value.Read(stream);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        _codec.Value.Write(encoder, value);
    }
}
