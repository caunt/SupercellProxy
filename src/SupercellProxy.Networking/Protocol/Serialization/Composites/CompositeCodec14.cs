using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Composites;

/// <summary>Reads and writes 14 fields in wire order and builds the value from them.</summary>
internal sealed class CompositeCodec14<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(
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
    ICodec<TEighth> eighth,
    Func<TValue, TEighth> getEighth,
    ICodec<TNinth> ninth,
    Func<TValue, TNinth> getNinth,
    ICodec<TTenth> tenth,
    Func<TValue, TTenth> getTenth,
    ICodec<TEleventh> eleventh,
    Func<TValue, TEleventh> getEleventh,
    ICodec<TTwelfth> twelfth,
    Func<TValue, TTwelfth> getTwelfth,
    ICodec<TThirteenth> thirteenth,
    Func<TValue, TThirteenth> getThirteenth,
    ICodec<TFourteenth> fourteenth,
    Func<TValue, TFourteenth> getFourteenth,
    Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TValue> factory
) : ICodec<TValue>
{
    private readonly Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TValue> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
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
    private readonly ICodec<TEighth> _eighth = eighth ?? throw new ArgumentNullException(nameof(eighth));
    private readonly Func<TValue, TEighth> _getEighth = getEighth ?? throw new ArgumentNullException(nameof(getEighth));
    private readonly ICodec<TNinth> _ninth = ninth ?? throw new ArgumentNullException(nameof(ninth));
    private readonly Func<TValue, TNinth> _getNinth = getNinth ?? throw new ArgumentNullException(nameof(getNinth));
    private readonly ICodec<TTenth> _tenth = tenth ?? throw new ArgumentNullException(nameof(tenth));
    private readonly Func<TValue, TTenth> _getTenth = getTenth ?? throw new ArgumentNullException(nameof(getTenth));
    private readonly ICodec<TEleventh> _eleventh = eleventh ?? throw new ArgumentNullException(nameof(eleventh));
    private readonly Func<TValue, TEleventh> _getEleventh = getEleventh ?? throw new ArgumentNullException(nameof(getEleventh));
    private readonly ICodec<TTwelfth> _twelfth = twelfth ?? throw new ArgumentNullException(nameof(twelfth));
    private readonly Func<TValue, TTwelfth> _getTwelfth = getTwelfth ?? throw new ArgumentNullException(nameof(getTwelfth));
    private readonly ICodec<TThirteenth> _thirteenth = thirteenth ?? throw new ArgumentNullException(nameof(thirteenth));
    private readonly Func<TValue, TThirteenth> _getThirteenth = getThirteenth ?? throw new ArgumentNullException(nameof(getThirteenth));
    private readonly ICodec<TFourteenth> _fourteenth = fourteenth ?? throw new ArgumentNullException(nameof(fourteenth));
    private readonly Func<TValue, TFourteenth> _getFourteenth = getFourteenth ?? throw new ArgumentNullException(nameof(getFourteenth));

    public TValue Read(MessageStream stream)
    {
        TFirst firstValue = _first.Read(stream);
        TSecond secondValue = _second.Read(stream);
        TThird thirdValue = _third.Read(stream);
        TFourth fourthValue = _fourth.Read(stream);
        TFifth fifthValue = _fifth.Read(stream);
        TSixth sixthValue = _sixth.Read(stream);
        TSeventh seventhValue = _seventh.Read(stream);
        TEighth eighthValue = _eighth.Read(stream);
        TNinth ninthValue = _ninth.Read(stream);
        TTenth tenthValue = _tenth.Read(stream);
        TEleventh eleventhValue = _eleventh.Read(stream);
        TTwelfth twelfthValue = _twelfth.Read(stream);
        TThirteenth thirteenthValue = _thirteenth.Read(stream);
        TFourteenth fourteenthValue = _fourteenth.Read(stream);

        return _factory(
            firstValue,
            secondValue,
            thirdValue,
            fourthValue,
            fifthValue,
            sixthValue,
            seventhValue,
            eighthValue,
            ninthValue,
            tenthValue,
            eleventhValue,
            twelfthValue,
            thirteenthValue,
            fourteenthValue
        );
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
        _eighth.Write(encoder, _getEighth(value));
        _ninth.Write(encoder, _getNinth(value));
        _tenth.Write(encoder, _getTenth(value));
        _eleventh.Write(encoder, _getEleventh(value));
        _twelfth.Write(encoder, _getTwelfth(value));
        _thirteenth.Write(encoder, _getThirteenth(value));
        _fourteenth.Write(encoder, _getFourteenth(value));
    }
}
