using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>Carries a value through another codec's wire form.</summary>
internal sealed class MappedCodec<TSource, TValue>(ICodec<TSource> codec, Func<TSource, TValue> fromWire, Func<TValue, TSource> toWire) : ICodec<TValue>
{
    public TValue Read(MessageStream stream)
    {
        return fromWire(codec.Read(stream));
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        codec.Write(encoder, toWire(value));
    }
}
