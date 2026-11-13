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
    /// <value>A shared encoder that writes a <see cref="short"/> through <see cref="ProtocolEncoder.WriteInt16"/>, two big-endian bytes on a <see cref="MessageChannel"/>; <see cref="Codecs"/> has no signed 16-bit counterpart.</value>
    public static IEncoder<short> BigEndianInt16 { get; } = new DelegateEncoder<short>(static (encoder, value) => encoder.WriteInt16(value));

    /// <summary>Gets an encoder that writes a count followed by every element; the count defaults to a var-int.</summary>
    /// <typeparam name="TElement">The type of one collection element; <paramref name="element"/> must be able to write it.</typeparam>
    /// <param name="element">Writes each element in turn, after the count, in the order the collection enumerates them.</param>
    /// <param name="count">How the element count is written and capped, such as <see cref="CollectionCount.BigEndianInt32"/> or <see cref="CollectionCount.AtMost"/>; <see langword="null"/> means <see cref="CollectionCount.Unbounded"/>, a var-int with no cap.</param>
    /// <returns>An encoder that writes <see cref="IReadOnlyCollection{T}.Count"/> in the chosen width and then every element; an empty collection writes only a zero count, and writing more elements than the count allows throws <see cref="InvalidDataException"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    public static IEncoder<IReadOnlyCollection<TElement>> Collection<TElement>(IEncoder<TElement> element, CollectionCount? count = null)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new CollectionEncoder<TElement>(element, count ?? CollectionCount.Unbounded);
    }

    /// <summary>Gets an encoder that writes 1 field of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded; only the one field that <paramref name="getFirst"/> picks out of it reaches the wire.</typeparam>
    /// <typeparam name="TFirst">The type of that single field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <param name="first">Writes the field; its bytes are the whole encoding of the value.</param>
    /// <param name="getFirst">Reads the field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <returns>An encoder that writes only the selected field, which lets a wider value reuse a field's encoder without a wrapper type.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="getFirst"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code language="csharp">
    /// IEncoder&lt;TimerSnapshot&gt; ticks = Encoders.Composite(Codecs.VarInt, static (TimerSnapshot timer) =&gt; timer.TicksLeft);
    /// </code>
    /// </example>
    public static IEncoder<TValue> Composite<TValue, TFirst>(IEncoder<TFirst> first, Func<TValue, TFirst> getFirst)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);

        return new SequenceEncoder<TValue>(parts: [new FieldEncoder<TValue, TFirst>(first, getFirst),]);
    }

    /// <summary>Gets an encoder that writes 2 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, often a small record or pair; both getters read from it.</typeparam>
    /// <typeparam name="TFirst">The type of the leading field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the trailing field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <param name="first">Writes the leading field, before the other one.</param>
    /// <param name="getFirst">Reads the leading field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the trailing field, right after the leading one; nothing follows it.</param>
    /// <param name="getSecond">Reads the trailing field from the value; it runs once per write, after <paramref name="first"/> has written.</param>
    /// <returns>An encoder that writes the two selected fields back to back, the leading one first, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="getFirst"/>, <paramref name="second"/> or <paramref name="getSecond"/> is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond>(IEncoder<TFirst> first, Func<TValue, TFirst> getFirst, IEncoder<TSecond> second, Func<TValue, TSecond> getSecond)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);

        return new SequenceEncoder<TValue>(parts: [new FieldEncoder<TValue, TFirst>(first, getFirst), new FieldEncoder<TValue, TSecond>(second, getSecond),]);
    }

    /// <summary>Gets an encoder that writes 3 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the three getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the middle field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third and last field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before the other two.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the middle field, right after the first one.</param>
    /// <param name="getSecond">Reads the middle field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one; nothing follows it.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, after the other two getters.</param>
    /// <returns>An encoder that writes the three selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the three encoders or three getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird>(IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);

        return new SequenceEncoder<TValue>(parts: [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
            ]);
    }

    /// <summary>Gets an encoder that writes 4 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the four getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth and last field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one; nothing follows it.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the four selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the four encoders or four getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth>(IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(getFirst);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(getSecond);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(getThird);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(getFourth);

        return new SequenceEncoder<TValue>(parts: [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
            ]);
    }

    /// <summary>Gets an encoder that writes 5 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the five getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth and last field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one; nothing follows it.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the five selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the five encoders or five getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth>(IEncoder<TFirst> first,
        Func<TValue, TFirst> getFirst,
        IEncoder<TSecond> second,
        Func<TValue, TSecond> getSecond,
        IEncoder<TThird> third,
        Func<TValue, TThird> getThird,
        IEncoder<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        IEncoder<TFifth> fifth,
        Func<TValue, TFifth> getFifth)
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

        return new SequenceEncoder<TValue>(parts: [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
            ]);
    }

    /// <summary>Gets an encoder that writes 6 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the six getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth and last field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one; nothing follows it.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the six selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the six encoders or six getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(IEncoder<TFirst> first,
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
        Func<TValue, TSixth> getSixth)
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

        return new SequenceEncoder<TValue>(parts: [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
            ]);
    }

    /// <summary>Gets an encoder that writes 7 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the seven getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh and last field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one; nothing follows it.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the seven selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the seven encoders or seven getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(IEncoder<TFirst> first,
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
        Func<TValue, TSeventh> getSeventh)
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

        return new SequenceEncoder<TValue>(parts: [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
            ]);
    }

    /// <summary>Gets an encoder that writes 8 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the eight getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth and last field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one; nothing follows it.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the eight selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the eight encoders or eight getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(IEncoder<TFirst> first,
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
        Func<TValue, TEighth> getEighth)
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

        return new SequenceEncoder<TValue>(parts: [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
            ]);
    }

    /// <summary>Gets an encoder that writes 9 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the nine getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <typeparam name="TNinth">The type of the ninth and last field, as <paramref name="getNinth"/> returns it and <paramref name="ninth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, just before <paramref name="eighth"/>.</param>
    /// <param name="ninth">Writes the ninth field, right after the eighth one; nothing follows it.</param>
    /// <param name="getNinth">Reads the ninth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the nine selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the nine encoders or nine getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(IEncoder<TFirst> first,
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
        Func<TValue, TNinth> getNinth)
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

        return new SequenceEncoder<TValue>(parts: [
                new FieldEncoder<TValue, TFirst>(first, getFirst),
                new FieldEncoder<TValue, TSecond>(second, getSecond),
                new FieldEncoder<TValue, TThird>(third, getThird),
                new FieldEncoder<TValue, TFourth>(fourth, getFourth),
                new FieldEncoder<TValue, TFifth>(fifth, getFifth),
                new FieldEncoder<TValue, TSixth>(sixth, getSixth),
                new FieldEncoder<TValue, TSeventh>(seventh, getSeventh),
                new FieldEncoder<TValue, TEighth>(eighth, getEighth),
                new FieldEncoder<TValue, TNinth>(ninth, getNinth),
            ]);
    }

    /// <summary>Gets an encoder that writes 10 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the ten getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <typeparam name="TNinth">The type of the ninth field, as <paramref name="getNinth"/> returns it and <paramref name="ninth"/> writes it.</typeparam>
    /// <typeparam name="TTenth">The type of the tenth and last field, as <paramref name="getTenth"/> returns it and <paramref name="tenth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, just before <paramref name="eighth"/>.</param>
    /// <param name="ninth">Writes the ninth field, right after the eighth one.</param>
    /// <param name="getNinth">Reads the ninth field from the value; it runs once per write, just before <paramref name="ninth"/>.</param>
    /// <param name="tenth">Writes the tenth field, right after the ninth one; nothing follows it.</param>
    /// <param name="getTenth">Reads the tenth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the ten selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the ten encoders or ten getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>(IEncoder<TFirst> first,
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
        Func<TValue, TTenth> getTenth)
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

        return new SequenceEncoder<TValue>(parts: [
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
            ]);
    }

    /// <summary>Gets an encoder that writes 11 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the eleven getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <typeparam name="TNinth">The type of the ninth field, as <paramref name="getNinth"/> returns it and <paramref name="ninth"/> writes it.</typeparam>
    /// <typeparam name="TTenth">The type of the tenth field, as <paramref name="getTenth"/> returns it and <paramref name="tenth"/> writes it.</typeparam>
    /// <typeparam name="TEleventh">The type of the eleventh and last field, as <paramref name="getEleventh"/> returns it and <paramref name="eleventh"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, just before <paramref name="eighth"/>.</param>
    /// <param name="ninth">Writes the ninth field, right after the eighth one.</param>
    /// <param name="getNinth">Reads the ninth field from the value; it runs once per write, just before <paramref name="ninth"/>.</param>
    /// <param name="tenth">Writes the tenth field, right after the ninth one.</param>
    /// <param name="getTenth">Reads the tenth field from the value; it runs once per write, just before <paramref name="tenth"/>.</param>
    /// <param name="eleventh">Writes the eleventh field, right after the tenth one; nothing follows it.</param>
    /// <param name="getEleventh">Reads the eleventh field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the eleven selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the eleven encoders or eleven getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>(IEncoder<TFirst> first,
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
        Func<TValue, TEleventh> getEleventh)
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

        return new SequenceEncoder<TValue>(parts: [
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
            ]);
    }

    /// <summary>Gets an encoder that writes 12 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the twelve getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <typeparam name="TNinth">The type of the ninth field, as <paramref name="getNinth"/> returns it and <paramref name="ninth"/> writes it.</typeparam>
    /// <typeparam name="TTenth">The type of the tenth field, as <paramref name="getTenth"/> returns it and <paramref name="tenth"/> writes it.</typeparam>
    /// <typeparam name="TEleventh">The type of the eleventh field, as <paramref name="getEleventh"/> returns it and <paramref name="eleventh"/> writes it.</typeparam>
    /// <typeparam name="TTwelfth">The type of the twelfth and last field, as <paramref name="getTwelfth"/> returns it and <paramref name="twelfth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, just before <paramref name="eighth"/>.</param>
    /// <param name="ninth">Writes the ninth field, right after the eighth one.</param>
    /// <param name="getNinth">Reads the ninth field from the value; it runs once per write, just before <paramref name="ninth"/>.</param>
    /// <param name="tenth">Writes the tenth field, right after the ninth one.</param>
    /// <param name="getTenth">Reads the tenth field from the value; it runs once per write, just before <paramref name="tenth"/>.</param>
    /// <param name="eleventh">Writes the eleventh field, right after the tenth one.</param>
    /// <param name="getEleventh">Reads the eleventh field from the value; it runs once per write, just before <paramref name="eleventh"/>.</param>
    /// <param name="twelfth">Writes the twelfth field, right after the eleventh one; nothing follows it.</param>
    /// <param name="getTwelfth">Reads the twelfth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the twelve selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the twelve encoders or twelve getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>(IEncoder<TFirst> first,
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
        Func<TValue, TTwelfth> getTwelfth)
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

        return new SequenceEncoder<TValue>(parts: [
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
            ]);
    }

    /// <summary>Gets an encoder that writes 13 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the thirteen getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <typeparam name="TNinth">The type of the ninth field, as <paramref name="getNinth"/> returns it and <paramref name="ninth"/> writes it.</typeparam>
    /// <typeparam name="TTenth">The type of the tenth field, as <paramref name="getTenth"/> returns it and <paramref name="tenth"/> writes it.</typeparam>
    /// <typeparam name="TEleventh">The type of the eleventh field, as <paramref name="getEleventh"/> returns it and <paramref name="eleventh"/> writes it.</typeparam>
    /// <typeparam name="TTwelfth">The type of the twelfth field, as <paramref name="getTwelfth"/> returns it and <paramref name="twelfth"/> writes it.</typeparam>
    /// <typeparam name="TThirteenth">The type of the thirteenth and last field, as <paramref name="getThirteenth"/> returns it and <paramref name="thirteenth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, just before <paramref name="eighth"/>.</param>
    /// <param name="ninth">Writes the ninth field, right after the eighth one.</param>
    /// <param name="getNinth">Reads the ninth field from the value; it runs once per write, just before <paramref name="ninth"/>.</param>
    /// <param name="tenth">Writes the tenth field, right after the ninth one.</param>
    /// <param name="getTenth">Reads the tenth field from the value; it runs once per write, just before <paramref name="tenth"/>.</param>
    /// <param name="eleventh">Writes the eleventh field, right after the tenth one.</param>
    /// <param name="getEleventh">Reads the eleventh field from the value; it runs once per write, just before <paramref name="eleventh"/>.</param>
    /// <param name="twelfth">Writes the twelfth field, right after the eleventh one.</param>
    /// <param name="getTwelfth">Reads the twelfth field from the value; it runs once per write, just before <paramref name="twelfth"/>.</param>
    /// <param name="thirteenth">Writes the thirteenth field, right after the twelfth one; nothing follows it.</param>
    /// <param name="getThirteenth">Reads the thirteenth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the thirteen selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the thirteen encoders or thirteen getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>(IEncoder<TFirst> first,
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
        Func<TValue, TThirteenth> getThirteenth)
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

        return new SequenceEncoder<TValue>(parts: [
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
            ]);
    }

    /// <summary>Gets an encoder that writes 14 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the fourteen getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <typeparam name="TNinth">The type of the ninth field, as <paramref name="getNinth"/> returns it and <paramref name="ninth"/> writes it.</typeparam>
    /// <typeparam name="TTenth">The type of the tenth field, as <paramref name="getTenth"/> returns it and <paramref name="tenth"/> writes it.</typeparam>
    /// <typeparam name="TEleventh">The type of the eleventh field, as <paramref name="getEleventh"/> returns it and <paramref name="eleventh"/> writes it.</typeparam>
    /// <typeparam name="TTwelfth">The type of the twelfth field, as <paramref name="getTwelfth"/> returns it and <paramref name="twelfth"/> writes it.</typeparam>
    /// <typeparam name="TThirteenth">The type of the thirteenth field, as <paramref name="getThirteenth"/> returns it and <paramref name="thirteenth"/> writes it.</typeparam>
    /// <typeparam name="TFourteenth">The type of the fourteenth and last field, as <paramref name="getFourteenth"/> returns it and <paramref name="fourteenth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, just before <paramref name="eighth"/>.</param>
    /// <param name="ninth">Writes the ninth field, right after the eighth one.</param>
    /// <param name="getNinth">Reads the ninth field from the value; it runs once per write, just before <paramref name="ninth"/>.</param>
    /// <param name="tenth">Writes the tenth field, right after the ninth one.</param>
    /// <param name="getTenth">Reads the tenth field from the value; it runs once per write, just before <paramref name="tenth"/>.</param>
    /// <param name="eleventh">Writes the eleventh field, right after the tenth one.</param>
    /// <param name="getEleventh">Reads the eleventh field from the value; it runs once per write, just before <paramref name="eleventh"/>.</param>
    /// <param name="twelfth">Writes the twelfth field, right after the eleventh one.</param>
    /// <param name="getTwelfth">Reads the twelfth field from the value; it runs once per write, just before <paramref name="twelfth"/>.</param>
    /// <param name="thirteenth">Writes the thirteenth field, right after the twelfth one.</param>
    /// <param name="getThirteenth">Reads the thirteenth field from the value; it runs once per write, just before <paramref name="thirteenth"/>.</param>
    /// <param name="fourteenth">Writes the fourteenth field, right after the thirteenth one; nothing follows it.</param>
    /// <param name="getFourteenth">Reads the fourteenth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the fourteen selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the fourteen encoders or fourteen getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(IEncoder<TFirst> first,
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
        Func<TValue, TFourteenth> getFourteenth)
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

        return new SequenceEncoder<TValue>(parts: [
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
            ]);
    }

    /// <summary>Gets an encoder that writes 15 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a record; the fifteen getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <typeparam name="TNinth">The type of the ninth field, as <paramref name="getNinth"/> returns it and <paramref name="ninth"/> writes it.</typeparam>
    /// <typeparam name="TTenth">The type of the tenth field, as <paramref name="getTenth"/> returns it and <paramref name="tenth"/> writes it.</typeparam>
    /// <typeparam name="TEleventh">The type of the eleventh field, as <paramref name="getEleventh"/> returns it and <paramref name="eleventh"/> writes it.</typeparam>
    /// <typeparam name="TTwelfth">The type of the twelfth field, as <paramref name="getTwelfth"/> returns it and <paramref name="twelfth"/> writes it.</typeparam>
    /// <typeparam name="TThirteenth">The type of the thirteenth field, as <paramref name="getThirteenth"/> returns it and <paramref name="thirteenth"/> writes it.</typeparam>
    /// <typeparam name="TFourteenth">The type of the fourteenth field, as <paramref name="getFourteenth"/> returns it and <paramref name="fourteenth"/> writes it.</typeparam>
    /// <typeparam name="TFifteenth">The type of the fifteenth and last field, as <paramref name="getFifteenth"/> returns it and <paramref name="fifteenth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, just before <paramref name="eighth"/>.</param>
    /// <param name="ninth">Writes the ninth field, right after the eighth one.</param>
    /// <param name="getNinth">Reads the ninth field from the value; it runs once per write, just before <paramref name="ninth"/>.</param>
    /// <param name="tenth">Writes the tenth field, right after the ninth one.</param>
    /// <param name="getTenth">Reads the tenth field from the value; it runs once per write, just before <paramref name="tenth"/>.</param>
    /// <param name="eleventh">Writes the eleventh field, right after the tenth one.</param>
    /// <param name="getEleventh">Reads the eleventh field from the value; it runs once per write, just before <paramref name="eleventh"/>.</param>
    /// <param name="twelfth">Writes the twelfth field, right after the eleventh one.</param>
    /// <param name="getTwelfth">Reads the twelfth field from the value; it runs once per write, just before <paramref name="twelfth"/>.</param>
    /// <param name="thirteenth">Writes the thirteenth field, right after the twelfth one.</param>
    /// <param name="getThirteenth">Reads the thirteenth field from the value; it runs once per write, just before <paramref name="thirteenth"/>.</param>
    /// <param name="fourteenth">Writes the fourteenth field, right after the thirteenth one.</param>
    /// <param name="getFourteenth">Reads the fourteenth field from the value; it runs once per write, just before <paramref name="fourteenth"/>.</param>
    /// <param name="fifteenth">Writes the fifteenth field, right after the fourteenth one; nothing follows it.</param>
    /// <param name="getFifteenth">Reads the fifteenth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the fifteen selected fields back to back in argument order, with no count, flag or padding between them.</returns>
    /// <exception cref="ArgumentNullException">Any of the fifteen encoders or fifteen getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(IEncoder<TFirst> first,
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
        Func<TValue, TFifteenth> getFifteenth)
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

        return new SequenceEncoder<TValue>(parts: [
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
            ]);
    }

    /// <summary>Gets an encoder that writes 16 fields of a value in order.</summary>
    /// <typeparam name="TValue">The type being encoded, usually a large record; the sixteen getters each read one of its fields.</typeparam>
    /// <typeparam name="TFirst">The type of the first field, as <paramref name="getFirst"/> returns it and <paramref name="first"/> writes it.</typeparam>
    /// <typeparam name="TSecond">The type of the second field, as <paramref name="getSecond"/> returns it and <paramref name="second"/> writes it.</typeparam>
    /// <typeparam name="TThird">The type of the third field, as <paramref name="getThird"/> returns it and <paramref name="third"/> writes it.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth field, as <paramref name="getFourth"/> returns it and <paramref name="fourth"/> writes it.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth field, as <paramref name="getFifth"/> returns it and <paramref name="fifth"/> writes it.</typeparam>
    /// <typeparam name="TSixth">The type of the sixth field, as <paramref name="getSixth"/> returns it and <paramref name="sixth"/> writes it.</typeparam>
    /// <typeparam name="TSeventh">The type of the seventh field, as <paramref name="getSeventh"/> returns it and <paramref name="seventh"/> writes it.</typeparam>
    /// <typeparam name="TEighth">The type of the eighth field, as <paramref name="getEighth"/> returns it and <paramref name="eighth"/> writes it.</typeparam>
    /// <typeparam name="TNinth">The type of the ninth field, as <paramref name="getNinth"/> returns it and <paramref name="ninth"/> writes it.</typeparam>
    /// <typeparam name="TTenth">The type of the tenth field, as <paramref name="getTenth"/> returns it and <paramref name="tenth"/> writes it.</typeparam>
    /// <typeparam name="TEleventh">The type of the eleventh field, as <paramref name="getEleventh"/> returns it and <paramref name="eleventh"/> writes it.</typeparam>
    /// <typeparam name="TTwelfth">The type of the twelfth field, as <paramref name="getTwelfth"/> returns it and <paramref name="twelfth"/> writes it.</typeparam>
    /// <typeparam name="TThirteenth">The type of the thirteenth field, as <paramref name="getThirteenth"/> returns it and <paramref name="thirteenth"/> writes it.</typeparam>
    /// <typeparam name="TFourteenth">The type of the fourteenth field, as <paramref name="getFourteenth"/> returns it and <paramref name="fourteenth"/> writes it.</typeparam>
    /// <typeparam name="TFifteenth">The type of the fifteenth field, as <paramref name="getFifteenth"/> returns it and <paramref name="fifteenth"/> writes it.</typeparam>
    /// <typeparam name="TSixteenth">The type of the sixteenth and last field, as <paramref name="getSixteenth"/> returns it and <paramref name="sixteenth"/> writes it.</typeparam>
    /// <param name="first">Writes the first field, before any other field.</param>
    /// <param name="getFirst">Reads the first field from the value; it runs once per write, just before <paramref name="first"/>.</param>
    /// <param name="second">Writes the second field, right after the first one.</param>
    /// <param name="getSecond">Reads the second field from the value; it runs once per write, just before <paramref name="second"/>.</param>
    /// <param name="third">Writes the third field, right after the second one.</param>
    /// <param name="getThird">Reads the third field from the value; it runs once per write, just before <paramref name="third"/>.</param>
    /// <param name="fourth">Writes the fourth field, right after the third one.</param>
    /// <param name="getFourth">Reads the fourth field from the value; it runs once per write, just before <paramref name="fourth"/>.</param>
    /// <param name="fifth">Writes the fifth field, right after the fourth one.</param>
    /// <param name="getFifth">Reads the fifth field from the value; it runs once per write, just before <paramref name="fifth"/>.</param>
    /// <param name="sixth">Writes the sixth field, right after the fifth one.</param>
    /// <param name="getSixth">Reads the sixth field from the value; it runs once per write, just before <paramref name="sixth"/>.</param>
    /// <param name="seventh">Writes the seventh field, right after the sixth one.</param>
    /// <param name="getSeventh">Reads the seventh field from the value; it runs once per write, just before <paramref name="seventh"/>.</param>
    /// <param name="eighth">Writes the eighth field, right after the seventh one.</param>
    /// <param name="getEighth">Reads the eighth field from the value; it runs once per write, just before <paramref name="eighth"/>.</param>
    /// <param name="ninth">Writes the ninth field, right after the eighth one.</param>
    /// <param name="getNinth">Reads the ninth field from the value; it runs once per write, just before <paramref name="ninth"/>.</param>
    /// <param name="tenth">Writes the tenth field, right after the ninth one.</param>
    /// <param name="getTenth">Reads the tenth field from the value; it runs once per write, just before <paramref name="tenth"/>.</param>
    /// <param name="eleventh">Writes the eleventh field, right after the tenth one.</param>
    /// <param name="getEleventh">Reads the eleventh field from the value; it runs once per write, just before <paramref name="eleventh"/>.</param>
    /// <param name="twelfth">Writes the twelfth field, right after the eleventh one.</param>
    /// <param name="getTwelfth">Reads the twelfth field from the value; it runs once per write, just before <paramref name="twelfth"/>.</param>
    /// <param name="thirteenth">Writes the thirteenth field, right after the twelfth one.</param>
    /// <param name="getThirteenth">Reads the thirteenth field from the value; it runs once per write, just before <paramref name="thirteenth"/>.</param>
    /// <param name="fourteenth">Writes the fourteenth field, right after the thirteenth one.</param>
    /// <param name="getFourteenth">Reads the fourteenth field from the value; it runs once per write, just before <paramref name="fourteenth"/>.</param>
    /// <param name="fifteenth">Writes the fifteenth field, right after the fourteenth one.</param>
    /// <param name="getFifteenth">Reads the fifteenth field from the value; it runs once per write, just before <paramref name="fifteenth"/>.</param>
    /// <param name="sixteenth">Writes the sixteenth field, right after the fifteenth one; nothing follows it.</param>
    /// <param name="getSixteenth">Reads the sixteenth field from the value; it runs once per write, last of all the getters.</param>
    /// <returns>An encoder that writes the sixteen selected fields back to back in argument order, with no count, flag or padding between them; for wider layouts, nest a composite as one of the fields.</returns>
    /// <exception cref="ArgumentNullException">Any of the sixteen encoders or sixteen getters is <see langword="null"/>.</exception>
    public static IEncoder<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(IEncoder<TFirst> first,
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
        Func<TValue, TSixteenth> getSixteenth)
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

        return new SequenceEncoder<TValue>(parts: [
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
            ]);
    }

    /// <summary>Gets an encoder from an explicit write function, for layouts that branch on the value.</summary>
    /// <typeparam name="TValue">The type the function knows how to write; it is also the type the returned encoder accepts.</typeparam>
    /// <param name="write">Called once per value with the target and the value; it writes whatever bytes the layout needs, and the encoder adds nothing around them.</param>
    /// <returns>An encoder that hands every write straight to <paramref name="write"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="write"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code language="csharp">
    /// IEncoder&lt;int?&gt; level = Encoders.Create&lt;int?&gt;(static (encoder, value) =&gt;
    /// {
    ///     encoder.WriteBoolean(value.HasValue);
    ///     if (value is { } present)
    ///         encoder.WriteVarInt(present);
    /// });
    /// </code>
    /// </example>
    public static IEncoder<TValue> Create<TValue>(Action<ProtocolEncoder, TValue> write)
    {
        ArgumentNullException.ThrowIfNull(write);

        return new DelegateEncoder<TValue>(write);
    }

    /// <summary>Gets an encoder that writes every element without a count.</summary>
    /// <typeparam name="TElement">The type of one element of the sequence; <paramref name="element"/> must be able to write it.</typeparam>
    /// <param name="element">Writes each element in turn, in the order the sequence yields them.</param>
    /// <returns>An encoder that enumerates the sequence once and writes the elements back to back, so an empty sequence writes no bytes; a <see langword="null"/> sequence throws <see cref="ArgumentNullException"/> when written.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    public static IEncoder<IEnumerable<TElement>> Each<TElement>(IEncoder<TElement> element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new EachEncoder<TElement>(element);
    }

    /// <summary>Gets an encoder that writes the value when present and nothing at all when it is absent.</summary>
    /// <typeparam name="TValue">The reference type that may be absent; <see langword="null"/> stands for a missing value.</typeparam>
    /// <param name="encoder">Writes a present value; it is never called for <see langword="null"/>.</param>
    /// <returns>An encoder that writes a non-<see langword="null"/> value through <paramref name="encoder"/> and writes no bytes, not even a flag, for <see langword="null"/>; a reader must learn about absence some other way, such as the end of the payload.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="encoder"/> is <see langword="null"/>.</exception>
    public static IEncoder<TValue?> IfPresent<TValue>(IEncoder<TValue> encoder) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(encoder);

        return new PresentEncoder<TValue>(encoder);
    }

    /// <summary>Gets an encoder that writes a presence flag followed by the value when present.</summary>
    /// <typeparam name="TValue">The reference type behind the flag; a <see langword="null"/> value is written as an absent flag.</typeparam>
    /// <param name="encoder">Writes the value after a set flag; it is not called when the value is <see langword="null"/>.</param>
    /// <returns>An encoder that writes <see langword="true"/> and then the value, or only <see langword="false"/> for <see langword="null"/>; it is the write side of <see cref="Codecs.Optional{TValue}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="encoder"/> is <see langword="null"/>.</exception>
    public static IEncoder<TValue?> Optional<TValue>(IEncoder<TValue> encoder) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(encoder);

        return new OptionalEncoder<TValue>(encoder);
    }
}
