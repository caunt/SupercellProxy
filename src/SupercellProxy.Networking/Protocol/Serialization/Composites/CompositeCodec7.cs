using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Composites;

/// <summary>Reads and writes 7 fields in wire order and builds the value from them.</summary>
internal sealed class CompositeCodec7<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(
    ICodec<TFirst> first,
    Func<TValue, TFirst> getFirst,
    ICodec<TSecond> second,
    Func<TValue, TSecond> getSecond,
    ICodec<TThird> third,
    Func<TValue, TThird> getThird,
    ICodec<TFourth> fourth,
    Func<TValue, TFourth> getFourth,
    ICodec<TFifth> fifth,
    Func<TValue, TFifth> getFifth,
    ICodec<TSixth> sixth,
    Func<TValue, TSixth> getSixth,
    ICodec<TSeventh> seventh,
    Func<TValue, TSeventh> getSeventh,
    Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TValue> factory
) : ICodec<TValue>
{
    private readonly Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TValue> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    private readonly ICodec<TFirst> _first = first ?? throw new ArgumentNullException(nameof(first));
    private readonly Func<TValue, TFirst> _getFirst = getFirst ?? throw new ArgumentNullException(nameof(getFirst));
    private readonly ICodec<TSecond> _second = second ?? throw new ArgumentNullException(nameof(second));
    private readonly Func<TValue, TSecond> _getSecond = getSecond ?? throw new ArgumentNullException(nameof(getSecond));
    private readonly ICodec<TThird> _third = third ?? throw new ArgumentNullException(nameof(third));
    private readonly Func<TValue, TThird> _getThird = getThird ?? throw new ArgumentNullException(nameof(getThird));
    private readonly ICodec<TFourth> _fourth = fourth ?? throw new ArgumentNullException(nameof(fourth));
    private readonly Func<TValue, TFourth> _getFourth = getFourth ?? throw new ArgumentNullException(nameof(getFourth));
    private readonly ICodec<TFifth> _fifth = fifth ?? throw new ArgumentNullException(nameof(fifth));
    private readonly Func<TValue, TFifth> _getFifth = getFifth ?? throw new ArgumentNullException(nameof(getFifth));
    private readonly ICodec<TSixth> _sixth = sixth ?? throw new ArgumentNullException(nameof(sixth));
    private readonly Func<TValue, TSixth> _getSixth = getSixth ?? throw new ArgumentNullException(nameof(getSixth));
    private readonly ICodec<TSeventh> _seventh = seventh ?? throw new ArgumentNullException(nameof(seventh));
    private readonly Func<TValue, TSeventh> _getSeventh = getSeventh ?? throw new ArgumentNullException(nameof(getSeventh));

    public TValue Read(MessageStream stream)
    {
        TFirst firstValue = _first.Read(stream);
        TSecond secondValue = _second.Read(stream);
        TThird thirdValue = _third.Read(stream);
        TFourth fourthValue = _fourth.Read(stream);
        TFifth fifthValue = _fifth.Read(stream);
        TSixth sixthValue = _sixth.Read(stream);
        TSeventh seventhValue = _seventh.Read(stream);

        return _factory(firstValue, secondValue, thirdValue, fourthValue, fifthValue, sixthValue, seventhValue);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        _first.Write(encoder, _getFirst(value));
        _second.Write(encoder, _getSecond(value));
        _third.Write(encoder, _getThird(value));
        _fourth.Write(encoder, _getFourth(value));
        _fifth.Write(encoder, _getFifth(value));
        _sixth.Write(encoder, _getSixth(value));
        _seventh.Write(encoder, _getSeventh(value));
    }
}
