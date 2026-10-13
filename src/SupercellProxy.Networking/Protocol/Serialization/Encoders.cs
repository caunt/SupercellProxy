using SupercellProxy.Networking.Protocol.Serialization.Writers;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>
/// <para>Write-only counterparts of <see cref="Codecs"/>, for layouts that are only ever encoded.</para>
/// <para>Every codec is also an encoder, so codec primitives and wire structures compose with them directly.</para>
/// </summary>
public static class Encoders
{
    /// <summary>Gets a 16-bit integer encoder.</summary>
    public static IEncoder<short> Int16 { get; } = new DelegateEncoder<short>(static (encoder, value) => encoder.WriteInt16(value));

    /// <summary>Gets an encoder that writes a count followed by every element; the count defaults to a var-int.</summary>
    public static IEncoder<IReadOnlyCollection<TElement>> Collection<TElement>(IEncoder<TElement> element, CollectionCount? count = null)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new CollectionEncoder<TElement>(element, count ?? CollectionCount.Unbounded);
    }

    /// <summary>Gets an encoder that writes 1 field of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst>(IEncoder<TFirst> first, Func<TValue, TFirst> getFirst)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);

        return new SequenceEncoder<TValue>([new FieldEncoder<TValue, TFirst>(first, getFirst),]);
    }

    /// <summary>Gets an encoder that writes 2 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond>(IEncoder<TFirst> first, Func<TValue, TFirst> getFirst, IEncoder<TSecond> second, Func<TValue, TSecond> getSecond)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);

        return new SequenceEncoder<TValue>([new FieldEncoder<TValue, TFirst>(first, getFirst), new FieldEncoder<TValue, TSecond>(second, getSecond),]);
    }

    /// <summary>Gets an encoder that writes 3 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 4 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 5 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 6 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 7 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 8 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 9 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth,
        IEncoder<TNinth> ninth,
        Func<TValue, TNinth> getNinth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);
        ArgumentNullException.ThrowIfNull(ninth);
        ArgumentNullException.ThrowIfNull(getNinth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 10 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth,
        IEncoder<TNinth> ninth,
        Func<TValue, TNinth> getNinth,
        IEncoder<TTenth> tenth,
        Func<TValue, TTenth> getTenth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);
        ArgumentNullException.ThrowIfNull(ninth);
        ArgumentNullException.ThrowIfNull(getNinth);
        ArgumentNullException.ThrowIfNull(tenth);
        ArgumentNullException.ThrowIfNull(getTenth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
                new FieldEncoder<TValue, TTenth>(tenth, getTenth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 11 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth,
        IEncoder<TNinth> ninth,
        Func<TValue, TNinth> getNinth,
        IEncoder<TTenth> tenth,
        Func<TValue, TTenth> getTenth,
        IEncoder<TEleventh> eleventh,
        Func<TValue, TEleventh> getEleventh
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);
        ArgumentNullException.ThrowIfNull(ninth);
        ArgumentNullException.ThrowIfNull(getNinth);
        ArgumentNullException.ThrowIfNull(tenth);
        ArgumentNullException.ThrowIfNull(getTenth);
        ArgumentNullException.ThrowIfNull(eleventh);
        ArgumentNullException.ThrowIfNull(getEleventh);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
                new FieldEncoder<TValue, TTenth>(tenth, getTenth),
                new FieldEncoder<TValue, TEleventh>(eleventh, getEleventh),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 12 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth,
        IEncoder<TNinth> ninth,
        Func<TValue, TNinth> getNinth,
        IEncoder<TTenth> tenth,
        Func<TValue, TTenth> getTenth,
        IEncoder<TEleventh> eleventh,
        Func<TValue, TEleventh> getEleventh,
        IEncoder<TTwelfth> twelfth,
        Func<TValue, TTwelfth> getTwelfth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);
        ArgumentNullException.ThrowIfNull(ninth);
        ArgumentNullException.ThrowIfNull(getNinth);
        ArgumentNullException.ThrowIfNull(tenth);
        ArgumentNullException.ThrowIfNull(getTenth);
        ArgumentNullException.ThrowIfNull(eleventh);
        ArgumentNullException.ThrowIfNull(getEleventh);
        ArgumentNullException.ThrowIfNull(twelfth);
        ArgumentNullException.ThrowIfNull(getTwelfth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
                new FieldEncoder<TValue, TTenth>(tenth, getTenth),
                new FieldEncoder<TValue, TEleventh>(eleventh, getEleventh),
                new FieldEncoder<TValue, TTwelfth>(twelfth, getTwelfth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 13 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth,
        IEncoder<TNinth> ninth,
        Func<TValue, TNinth> getNinth,
        IEncoder<TTenth> tenth,
        Func<TValue, TTenth> getTenth,
        IEncoder<TEleventh> eleventh,
        Func<TValue, TEleventh> getEleventh,
        IEncoder<TTwelfth> twelfth,
        Func<TValue, TTwelfth> getTwelfth,
        IEncoder<TThirteenth> thirteenth,
        Func<TValue, TThirteenth> getThirteenth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);
        ArgumentNullException.ThrowIfNull(ninth);
        ArgumentNullException.ThrowIfNull(getNinth);
        ArgumentNullException.ThrowIfNull(tenth);
        ArgumentNullException.ThrowIfNull(getTenth);
        ArgumentNullException.ThrowIfNull(eleventh);
        ArgumentNullException.ThrowIfNull(getEleventh);
        ArgumentNullException.ThrowIfNull(twelfth);
        ArgumentNullException.ThrowIfNull(getTwelfth);
        ArgumentNullException.ThrowIfNull(thirteenth);
        ArgumentNullException.ThrowIfNull(getThirteenth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
                new FieldEncoder<TValue, TTenth>(tenth, getTenth),
                new FieldEncoder<TValue, TEleventh>(eleventh, getEleventh),
                new FieldEncoder<TValue, TTwelfth>(twelfth, getTwelfth),
                new FieldEncoder<TValue, TThirteenth>(thirteenth, getThirteenth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 14 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth,
        IEncoder<TNinth> ninth,
        Func<TValue, TNinth> getNinth,
        IEncoder<TTenth> tenth,
        Func<TValue, TTenth> getTenth,
        IEncoder<TEleventh> eleventh,
        Func<TValue, TEleventh> getEleventh,
        IEncoder<TTwelfth> twelfth,
        Func<TValue, TTwelfth> getTwelfth,
        IEncoder<TThirteenth> thirteenth,
        Func<TValue, TThirteenth> getThirteenth,
        IEncoder<TFourteenth> fourteenth,
        Func<TValue, TFourteenth> getFourteenth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);
        ArgumentNullException.ThrowIfNull(ninth);
        ArgumentNullException.ThrowIfNull(getNinth);
        ArgumentNullException.ThrowIfNull(tenth);
        ArgumentNullException.ThrowIfNull(getTenth);
        ArgumentNullException.ThrowIfNull(eleventh);
        ArgumentNullException.ThrowIfNull(getEleventh);
        ArgumentNullException.ThrowIfNull(twelfth);
        ArgumentNullException.ThrowIfNull(getTwelfth);
        ArgumentNullException.ThrowIfNull(thirteenth);
        ArgumentNullException.ThrowIfNull(getThirteenth);
        ArgumentNullException.ThrowIfNull(fourteenth);
        ArgumentNullException.ThrowIfNull(getFourteenth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
                new FieldEncoder<TValue, TTenth>(tenth, getTenth),
                new FieldEncoder<TValue, TEleventh>(eleventh, getEleventh),
                new FieldEncoder<TValue, TTwelfth>(twelfth, getTwelfth),
                new FieldEncoder<TValue, TThirteenth>(thirteenth, getThirteenth),
                new FieldEncoder<TValue, TFourteenth>(fourteenth, getFourteenth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 15 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth,
        IEncoder<TNinth> ninth,
        Func<TValue, TNinth> getNinth,
        IEncoder<TTenth> tenth,
        Func<TValue, TTenth> getTenth,
        IEncoder<TEleventh> eleventh,
        Func<TValue, TEleventh> getEleventh,
        IEncoder<TTwelfth> twelfth,
        Func<TValue, TTwelfth> getTwelfth,
        IEncoder<TThirteenth> thirteenth,
        Func<TValue, TThirteenth> getThirteenth,
        IEncoder<TFourteenth> fourteenth,
        Func<TValue, TFourteenth> getFourteenth,
        IEncoder<TFifteenth> fifteenth,
        Func<TValue, TFifteenth> getFifteenth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);
        ArgumentNullException.ThrowIfNull(ninth);
        ArgumentNullException.ThrowIfNull(getNinth);
        ArgumentNullException.ThrowIfNull(tenth);
        ArgumentNullException.ThrowIfNull(getTenth);
        ArgumentNullException.ThrowIfNull(eleventh);
        ArgumentNullException.ThrowIfNull(getEleventh);
        ArgumentNullException.ThrowIfNull(twelfth);
        ArgumentNullException.ThrowIfNull(getTwelfth);
        ArgumentNullException.ThrowIfNull(thirteenth);
        ArgumentNullException.ThrowIfNull(getThirteenth);
        ArgumentNullException.ThrowIfNull(fourteenth);
        ArgumentNullException.ThrowIfNull(getFourteenth);
        ArgumentNullException.ThrowIfNull(fifteenth);
        ArgumentNullException.ThrowIfNull(getFifteenth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
                new FieldEncoder<TValue, TTenth>(tenth, getTenth),
                new FieldEncoder<TValue, TEleventh>(eleventh, getEleventh),
                new FieldEncoder<TValue, TTwelfth>(twelfth, getTwelfth),
                new FieldEncoder<TValue, TThirteenth>(thirteenth, getThirteenth),
                new FieldEncoder<TValue, TFourteenth>(fourteenth, getFourteenth),
                new FieldEncoder<TValue, TFifteenth>(fifteenth, getFifteenth),
            ]
        );
    }

    /// <summary>Gets an encoder that writes 16 fields of a value in order.</summary>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(
        IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        IEncoder<TSixth> sixth,
        Func<TValue, TSixth> getSixth,
        IEncoder<TSeventh> seventh,
        Func<TValue, TSeventh> getSeventh,
        IEncoder<TEighth> eighth,
        Func<TValue, TEighth> getEighth,
        IEncoder<TNinth> ninth,
        Func<TValue, TNinth> getNinth,
        IEncoder<TTenth> tenth,
        Func<TValue, TTenth> getTenth,
        IEncoder<TEleventh> eleventh,
        Func<TValue, TEleventh> getEleventh,
        IEncoder<TTwelfth> twelfth,
        Func<TValue, TTwelfth> getTwelfth,
        IEncoder<TThirteenth> thirteenth,
        Func<TValue, TThirteenth> getThirteenth,
        IEncoder<TFourteenth> fourteenth,
        Func<TValue, TFourteenth> getFourteenth,
        IEncoder<TFifteenth> fifteenth,
        Func<TValue, TFifteenth> getFifteenth,
        IEncoder<TSixteenth> sixteenth,
        Func<TValue, TSixteenth> getSixteenth
    )
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(getFifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(getSixth);
        ArgumentNullException.ThrowIfNull(seventh);
        ArgumentNullException.ThrowIfNull(getSeventh);
        ArgumentNullException.ThrowIfNull(eighth);
        ArgumentNullException.ThrowIfNull(getEighth);
        ArgumentNullException.ThrowIfNull(ninth);
        ArgumentNullException.ThrowIfNull(getNinth);
        ArgumentNullException.ThrowIfNull(tenth);
        ArgumentNullException.ThrowIfNull(getTenth);
        ArgumentNullException.ThrowIfNull(eleventh);
        ArgumentNullException.ThrowIfNull(getEleventh);
        ArgumentNullException.ThrowIfNull(twelfth);
        ArgumentNullException.ThrowIfNull(getTwelfth);
        ArgumentNullException.ThrowIfNull(thirteenth);
        ArgumentNullException.ThrowIfNull(getThirteenth);
        ArgumentNullException.ThrowIfNull(fourteenth);
        ArgumentNullException.ThrowIfNull(getFourteenth);
        ArgumentNullException.ThrowIfNull(fifteenth);
        ArgumentNullException.ThrowIfNull(getFifteenth);
        ArgumentNullException.ThrowIfNull(sixteenth);
        ArgumentNullException.ThrowIfNull(getSixteenth);

        return new SequenceEncoder<TValue>(
            [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
                new FieldEncoder<TValue, TTenth>(tenth, getTenth),
                new FieldEncoder<TValue, TEleventh>(eleventh, getEleventh),
                new FieldEncoder<TValue, TTwelfth>(twelfth, getTwelfth),
                new FieldEncoder<TValue, TThirteenth>(thirteenth, getThirteenth),
                new FieldEncoder<TValue, TFourteenth>(fourteenth, getFourteenth),
                new FieldEncoder<TValue, TFifteenth>(fifteenth, getFifteenth),
                new FieldEncoder<TValue, TSixteenth>(sixteenth, getSixteenth),
            ]
        );
    }

    /// <summary>Gets an encoder from an explicit write function, for layouts that branch on the value.</summary>
    public static IEncoder<TValue> Create<TValue>(Action<ProtocolEncoder, TValue> write)
    {
        ArgumentNullException.ThrowIfNull(write);

        return new DelegateEncoder<TValue>(write);
    }

    /// <summary>Gets an encoder that writes every element without a count.</summary>
    public static IEncoder<IEnumerable<TElement>> Each<TElement>(IEncoder<TElement> element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new EachEncoder<TElement>(element);
    }

    /// <summary>Gets an encoder that writes the value when present and nothing at all when it is absent.</summary>
    public static IEncoder<TValue?> IfPresent<TValue>(IEncoder<TValue> encoder) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(encoder);

        return new PresentEncoder<TValue>(encoder);
    }

    /// <summary>Gets an encoder that writes a presence flag followed by the value when present.</summary>
    public static IEncoder<TValue?> Optional<TValue>(IEncoder<TValue> encoder) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(encoder);

        return new OptionalEncoder<TValue>(encoder);
    }
}
