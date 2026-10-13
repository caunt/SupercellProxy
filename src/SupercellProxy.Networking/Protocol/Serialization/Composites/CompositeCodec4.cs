using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Composites;

/// <summary>Reads and writes 4 fields in wire order and builds the value from them.</summary>
internal sealed class CompositeCodec4<TValue, TFirst, TSecond, TThird, TFourth>(
    ICodec<TFirst> first,
    Func<TValue, TFirst> getFirst,
    ICodec<TSecond> second,
    Func<TValue, TSecond> getSecond,
    ICodec<TThird> third,
    Func<TValue, TThird> getThird,
    ICodec<TFourth> fourth,
    Func<TValue, TFourth> getFourth,
    Func<TFirst, TSecond, TThird, TFourth, TValue> factory
) : ICodec<TValue>
{
    private readonly Func<TFirst, TSecond, TThird, TFourth, TValue> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    private readonly ICodec<TFirst> _first = first ?? throw new ArgumentNullException(nameof(first));
    private readonly Func<TValue, TFirst> _getFirst = getFirst ?? throw new ArgumentNullException(nameof(getFirst));
    private readonly ICodec<TSecond> _second = second ?? throw new ArgumentNullException(nameof(second));
    private readonly Func<TValue, TSecond> _getSecond = getSecond ?? throw new ArgumentNullException(nameof(getSecond));
    private readonly ICodec<TThird> _third = third ?? throw new ArgumentNullException(nameof(third));
    private readonly Func<TValue, TThird> _getThird = getThird ?? throw new ArgumentNullException(nameof(getThird));
    private readonly ICodec<TFourth> _fourth = fourth ?? throw new ArgumentNullException(nameof(fourth));
    private readonly Func<TValue, TFourth> _getFourth = getFourth ?? throw new ArgumentNullException(nameof(getFourth));

    public TValue Read(MessageStream stream)
    {
        TFirst firstValue = _first.Read(stream);
        TSecond secondValue = _second.Read(stream);
        TThird thirdValue = _third.Read(stream);
        TFourth fourthValue = _fourth.Read(stream);

        return _factory(firstValue, secondValue, thirdValue, fourthValue);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        _first.Write(encoder, _getFirst(value));
        _second.Write(encoder, _getSecond(value));
        _third.Write(encoder, _getThird(value));
        _fourth.Write(encoder, _getFourth(value));
    }
}
