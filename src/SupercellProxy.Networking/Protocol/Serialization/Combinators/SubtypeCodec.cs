using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>Uses a derived type's codec where its base type is expected.</summary>
internal sealed class SubtypeCodec<TBase, TDerived>(ICodec<TDerived> codec) : ICodec<TBase> where TBase : class where TDerived : class, TBase
{
    public TBase Read(MessageStream stream)
    {
        return codec.Read(stream);
    }

    public void Write(ProtocolEncoder encoder, TBase value)
    {
        codec.Write(encoder, value as TDerived ?? throw new InvalidDataException($"Expected a {typeof(TDerived).Name}."));
    }
}
