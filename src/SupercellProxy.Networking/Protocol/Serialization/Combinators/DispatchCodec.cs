using System.Collections.Frozen;
using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A tagged value: a key on the wire selects the codec of the rest.</summary>
internal sealed class DispatchCodec<TKey, TValue>(ICodec<TKey> key, Func<TValue, TKey> keyOf, FrozenDictionary<TKey, ICodec<TValue>> cases) : ICodec<TValue>
    where TKey : notnull
{
    public TValue Read(MessageStream stream)
    {
        return Select(key.Read(stream)).Read(stream);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        TKey tag = keyOf(value);
        ICodec<TValue> codec = Select(tag);
        key.Write(encoder, tag);
        codec.Write(encoder, value);
    }

    private ICodec<TValue> Select(TKey tag)
    {
        return cases.TryGetValue(tag, out ICodec<TValue>? codec)
            ? codec
            : throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Unsupported {typeof(TValue).Name} variant {tag}."));
    }
}
