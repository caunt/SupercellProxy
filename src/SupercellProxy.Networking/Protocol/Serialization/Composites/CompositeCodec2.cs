using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Composites;

/// <summary>Reads and writes 2 fields in wire order and builds the value from them.</summary>
internal sealed class CompositeCodec2<TValue, TFirst, TSecond>(
    ICodec<TFirst> first,
    Func<TValue, TFirst> getFirst,
    ICodec<TSecond> second,
    Func<TValue, TSecond> getSecond,
    Func<TFirst, TSecond, TValue> factory
) : ICodec<TValue>
{
    private readonly Func<TFirst, TSecond, TValue> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    private readonly ICodec<TFirst> _first = first ?? throw new ArgumentNullException(nameof(first));
    private readonly Func<TValue, TFirst> _getFirst = getFirst ?? throw new ArgumentNullException(nameof(getFirst));
    private readonly ICodec<TSecond> _second = second ?? throw new ArgumentNullException(nameof(second));
    private readonly Func<TValue, TSecond> _getSecond = getSecond ?? throw new ArgumentNullException(nameof(getSecond));

    public TValue Read(MessageStream stream)
    {
        TFirst firstValue = _first.Read(stream);
        TSecond secondValue = _second.Read(stream);

        return _factory(firstValue, secondValue);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        _first.Write(encoder, _getFirst(value));
        _second.Write(encoder, _getSecond(value));
    }
}
