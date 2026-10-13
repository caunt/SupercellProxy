using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Composites;

/// <summary>Reads and writes 1 field in wire order and builds the value from them.</summary>
internal sealed class CompositeCodec1<TValue, TFirst>(ICodec<TFirst> first, Func<TValue, TFirst> getFirst, Func<TFirst, TValue> factory) : ICodec<TValue>
{
    private readonly Func<TFirst, TValue> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    private readonly ICodec<TFirst> _first = first ?? throw new ArgumentNullException(nameof(first));
    private readonly Func<TValue, TFirst> _getFirst = getFirst ?? throw new ArgumentNullException(nameof(getFirst));

    public TValue Read(MessageStream stream)
    {
        TFirst firstValue = _first.Read(stream);

        return _factory(firstValue);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        _first.Write(encoder, _getFirst(value));
    }
}
