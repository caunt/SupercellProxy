using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A leading key whose value selects the codec of the rest of the structure.</summary>
internal sealed class BindCodec<TKey, TValue>(ICodec<TKey> key, Func<TValue, TKey> keyOf, Func<TKey, ICodec<TValue>> rest) : ICodec<TValue>
{
    public TValue Read(MessageStream stream)
    {
        return rest(key.Read(stream)).Read(stream);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        TKey tag = keyOf(value);
        key.Write(encoder, tag);
        rest(tag).Write(encoder, value);
    }
}
