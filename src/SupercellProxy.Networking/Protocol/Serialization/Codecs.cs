using System.Collections.Frozen;
using System.Runtime.CompilerServices;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization.Combinators;
using SupercellProxy.Networking.Protocol.Serialization.Composites;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>The native wire primitives and the operations that compose them into structure codecs.</summary>
public static class Codecs
{
    /// <summary>Gets a bit-packed boolean.</summary>
    public static ICodec<bool> Boolean { get; } = new DelegateCodec<bool>(static stream => stream.ReadBoolean(), static (stream, value) => stream.WriteBoolean(value));

    /// <summary>Gets one byte.</summary>
    public static ICodec<byte> Byte { get; } = new DelegateCodec<byte>(static stream => stream.ReadByte(), static (stream, value) => stream.WriteByte(value));

    /// <summary>Gets a 32-bit length followed by that many bytes.</summary>
    public static ICodec<ReadOnlyMemory<byte>> ByteArray { get; } = new DelegateCodec<ReadOnlyMemory<byte>>(static stream => stream.ReadByteArray(), static (stream, value) => stream.WriteByteArray(value.Span));

    /// <summary>Gets a big-endian 32-bit integer.</summary>
    public static ICodec<int> Int32 { get; } = new DelegateCodec<int>(static stream => stream.ReadInt32(), static (stream, value) => stream.WriteInt32(value));

    /// <summary>Gets a big-endian 64-bit integer.</summary>
    public static ICodec<long> Int64 { get; } = new DelegateCodec<long>(static stream => stream.ReadInt64(), static (stream, value) => stream.WriteInt64(value));

    /// <summary>Gets an IEEE single carried in the bits of a big-endian 32-bit integer.</summary>
    public static ICodec<float> Float { get; } = new DelegateCodec<float>(
        static stream => BitConverter.Int32BitsToSingle(stream.ReadInt32()),
        static (stream, value) => stream.WriteInt32(BitConverter.SingleToInt32Bits(value))
    );

    /// <summary>Gets a high/low pair of 32-bit integers.</summary>
    public static ICodec<LongId> LongId { get; } = new DelegateCodec<LongId>(static stream => stream.ReadLongId(), static (stream, value) => stream.WriteLongId(value));

    /// <summary>Gets a byte array that native writes with a -1 length when it is absent.</summary>
    public static ICodec<ReadOnlyMemory<byte>?> OptionalByteArray { get; } = new DelegateCodec<ReadOnlyMemory<byte>?>(static stream => stream.ReadOptionalByteArray(), static (stream, value) => stream.WriteOptionalByteArray(value));

    /// <summary>Gets a presence flag followed by a <see cref="Protocol.LongId"/>.</summary>
    public static ICodec<LongId?> OptionalLongId { get; } = new DelegateCodec<LongId?>(static stream => stream.ReadOptionalLongId(), static (stream, value) => stream.WriteOptionalLongId(value));

    /// <summary>Gets a string that native writes with a -1 length when it is absent.</summary>
    public static ICodec<string?> OptionalString { get; } = new DelegateCodec<string?>(static stream => stream.ReadOptionalString(), static (stream, value) => stream.WriteOptionalString(value));

    /// <summary>Gets a length-prefixed UTF-8 string.</summary>
    public static ICodec<string> String { get; } = new DelegateCodec<string>(static stream => stream.ReadString(), static (stream, value) => stream.WriteString(value));

    /// <summary>Gets a big-endian 16-bit unsigned integer.</summary>
    public static ICodec<ushort> UInt16 { get; } = new DelegateCodec<ushort>(static stream => stream.ReadUInt16(), static (stream, value) => stream.WriteUInt16(value));

    /// <summary>Gets a big-endian 32-bit unsigned integer.</summary>
    public static ICodec<uint> UInt32 { get; } = new DelegateCodec<uint>(static stream => stream.ReadUInt32(), static (stream, value) => stream.WriteUInt32(value));

    /// <summary>Gets a big-endian 64-bit unsigned integer.</summary>
    public static ICodec<ulong> UInt64 { get; } = new DelegateCodec<ulong>(static stream => stream.ReadUInt64(), static (stream, value) => stream.WriteUInt64(value));

    /// <summary>Gets a native variable-length 32-bit integer.</summary>
    public static ICodec<int> VarInt { get; } = new DelegateCodec<int>(static stream => stream.ReadVarInt(), static (stream, value) => stream.WriteVarInt(value));

    /// <summary>Gets a var-int length followed by that many bytes.</summary>
    public static ICodec<ReadOnlyMemory<byte>> VarIntByteArray { get; } = new DelegateCodec<ReadOnlyMemory<byte>>(static stream => stream.ReadVarIntByteArray(), static (stream, value) => stream.WriteVarIntByteArray(value.Span));

    /// <summary>Gets a native variable-length 64-bit integer.</summary>
    public static ICodec<long> VarLong { get; } = new DelegateCodec<long>(static stream => stream.ReadVarLong(), static (stream, value) => stream.WriteVarLong(value));

    /// <summary>Gets a counted array of elements; the count defaults to a var-int bounded by the remaining payload.</summary>
    public static ICodec<TElement[]> Array<TElement>(ICodec<TElement> element, CollectionCount? count = null)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new ArrayCodec<TElement>(element, count ?? CollectionCount.VarInt);
    }

    /// <summary>Gets a structure whose leading key selects the codec of its remaining fields, for wire forms where later fields depend on an earlier one.</summary>
    /// <param name="key">The codec of the leading field.</param>
    /// <param name="keyOf">Gets the leading field from a value.</param>
    /// <param name="rest">Gets the codec that reads the remaining fields, given the leading field, and builds the whole value.</param>
    public static ICodec<TValue> Bind<TKey, TValue>(ICodec<TKey> key, Func<TValue, TKey> keyOf, Func<TKey, ICodec<TValue>> rest)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(keyOf);
        ArgumentNullException.ThrowIfNull(rest);

        return new BindCodec<TKey, TValue>(key, keyOf, rest);
    }

    /// <summary>Gets the contract of a command record: its fields wrapped with the shared command metadata.</summary>
    public static CommandCodec<TCommand> Command<TCommand>(ICodec<TCommand> fields, CommandLayout layout = CommandLayout.FieldsFirst, bool productionAllowed = true) where TCommand : Commands.Command
    {
        return Command(typeof(TCommand).Name, fields, layout, productionAllowed);
    }

    /// <summary>Gets the contract named <paramref name="name"/> of a record that carries several native commands.</summary>
    public static CommandCodec<TCommand> Command<TCommand>(string name, ICodec<TCommand> fields, CommandLayout layout = CommandLayout.FieldsFirst, bool productionAllowed = true) where TCommand : Commands.Command
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(fields);

        return new CommandCodec<TCommand>(name, fields, layout, productionAllowed);
    }

    /// <summary>Gets a payload codec that rejects bytes left after its last field with <paramref name="trailingDataMessage"/>.</summary>
    public static ICodec<TValue> Complete<TValue>(ICodec<TValue> codec, string trailingDataMessage)
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new CompleteCodec<TValue>(codec, trailingDataMessage);
    }

    /// <summary>Composes 1 field codec: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst>(ICodec<TFirst> first, Func<TValue, TFirst> getFirst, Func<TFirst, TValue> factory)
    {
        return new CompositeCodec1<TValue, TFirst>(first, getFirst, factory);
    }

    /// <summary>Composes 2 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond>(
        ICodec<TFirst> first,
        Func<TValue, TFirst> getFirst,
        ICodec<TSecond> second,
        Func<TValue, TSecond> getSecond,
        Func<TFirst, TSecond, TValue> factory
    )
    {
        return new CompositeCodec2<TValue, TFirst, TSecond>(first, getFirst, second, getSecond, factory);
    }

    /// <summary>Composes 3 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird>(
        ICodec<TFirst> first,
        Func<TValue, TFirst> getFirst,
        ICodec<TSecond> second,
        Func<TValue, TSecond> getSecond,
        ICodec<TThird> third,
        Func<TValue, TThird> getThird,
        Func<TFirst, TSecond, TThird, TValue> factory
    )
    {
        return new CompositeCodec3<TValue, TFirst, TSecond, TThird>(first, getFirst, second, getSecond, third, getThird, factory);
    }

    /// <summary>Composes 4 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth>(
        ICodec<TFirst> first,
        Func<TValue, TFirst> getFirst,
        ICodec<TSecond> second,
        Func<TValue, TSecond> getSecond,
        ICodec<TThird> third,
        Func<TValue, TThird> getThird,
        ICodec<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        Func<TFirst, TSecond, TThird, TFourth, TValue> factory
    )
    {
        return new CompositeCodec4<TValue, TFirst, TSecond, TThird, TFourth>(first, getFirst, second, getSecond, third, getThird, fourth, getFourth, factory);
    }

    /// <summary>Composes 5 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth>(
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TValue> factory
    )
    {
        return new CompositeCodec5<TValue, TFirst, TSecond, TThird, TFourth, TFifth>(first, getFirst, second, getSecond, third, getThird, fourth, getFourth, fifth, getFifth, factory);
    }

    /// <summary>Composes 6 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TValue> factory
    )
    {
        return new CompositeCodec6<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(first, getFirst, second, getSecond, third, getThird, fourth, getFourth, fifth, getFifth, sixth, getSixth, factory);
    }

    /// <summary>Composes 7 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(
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
    )
    {
        return new CompositeCodec7<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            factory
        );
    }

    /// <summary>Composes 8 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TValue> factory
    )
    {
        return new CompositeCodec8<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            factory
        );
    }

    /// <summary>Composes 9 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TValue> factory
    )
    {
        return new CompositeCodec9<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            ninth,
            getNinth,
            factory
        );
    }

    /// <summary>Composes 10 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>(
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TValue> factory
    )
    {
        return new CompositeCodec10<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            ninth,
            getNinth,
            tenth,
            getTenth,
            factory
        );
    }

    /// <summary>Composes 11 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>(
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TValue> factory
    )
    {
        return new CompositeCodec11<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            ninth,
            getNinth,
            tenth,
            getTenth,
            eleventh,
            getEleventh,
            factory
        );
    }

    /// <summary>Composes 12 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>(
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TValue> factory
    )
    {
        return new CompositeCodec12<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            ninth,
            getNinth,
            tenth,
            getTenth,
            eleventh,
            getEleventh,
            twelfth,
            getTwelfth,
            factory
        );
    }

    /// <summary>Composes 13 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>(
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TValue> factory
    )
    {
        return new CompositeCodec13<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            ninth,
            getNinth,
            tenth,
            getTenth,
            eleventh,
            getEleventh,
            twelfth,
            getTwelfth,
            thirteenth,
            getThirteenth,
            factory
        );
    }

    /// <summary>Composes 14 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(
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
    )
    {
        return new CompositeCodec14<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            ninth,
            getNinth,
            tenth,
            getTenth,
            eleventh,
            getEleventh,
            twelfth,
            getTwelfth,
            thirteenth,
            getThirteenth,
            fourteenth,
            getFourteenth,
            factory
        );
    }

    /// <summary>Composes 15 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(
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
        ICodec<TFifteenth> fifteenth,
        Func<TValue, TFifteenth> getFifteenth,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TValue> factory
    )
    {
        return new CompositeCodec15<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            ninth,
            getNinth,
            tenth,
            getTenth,
            eleventh,
            getEleventh,
            twelfth,
            getTwelfth,
            thirteenth,
            getThirteenth,
            fourteenth,
            getFourteenth,
            fifteenth,
            getFifteenth,
            factory
        );
    }

    /// <summary>Composes 16 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(
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
        ICodec<TFifteenth> fifteenth,
        Func<TValue, TFifteenth> getFifteenth,
        ICodec<TSixteenth> sixteenth,
        Func<TValue, TSixteenth> getSixteenth,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth, TValue> factory
    )
    {
        return new CompositeCodec16<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(
            first,
            getFirst,
            second,
            getSecond,
            third,
            getThird,
            fourth,
            getFourth,
            fifth,
            getFifth,
            sixth,
            getSixth,
            seventh,
            getSeventh,
            eighth,
            getEighth,
            ninth,
            getNinth,
            tenth,
            getTenth,
            eleventh,
            getEleventh,
            twelfth,
            getTwelfth,
            thirteenth,
            getThirteenth,
            fourteenth,
            getFourteenth,
            fifteenth,
            getFifteenth,
            sixteenth,
            getSixteenth,
            factory
        );
    }

    /// <summary>Creates a codec from explicit read and write functions, for wire forms whose later fields depend on earlier ones.</summary>
    public static ICodec<TValue> Create<TValue>(Func<MessageStream, TValue> read, Action<ProtocolEncoder, TValue> write)
    {
        ArgumentNullException.ThrowIfNull(read);
        ArgumentNullException.ThrowIfNull(write);

        return new DelegateCodec<TValue>(read, write);
    }

    /// <summary>Gets a tagged value: the key selects the case codec that carries the rest.</summary>
    public static ICodec<TValue> Dispatch<TKey, TValue>(ICodec<TKey> key, Func<TValue, TKey> keyOf, IEnumerable<KeyValuePair<TKey, ICodec<TValue>>> cases)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(keyOf);
        ArgumentNullException.ThrowIfNull(cases);

        return new DispatchCodec<TKey, TValue>(key, keyOf, cases.ToFrozenDictionary());
    }

    /// <summary>Gets an int-backed enum carried by an integer codec.</summary>
    public static ICodec<TEnum> Enum<TEnum>(ICodec<int> underlying) where TEnum : struct, System.Enum
    {
        return Map(underlying, static value => Unsafe.BitCast<int, TEnum>(value), static value => Unsafe.BitCast<TEnum, int>(value));
    }

    /// <summary>Gets exactly <paramref name="length"/> elements without a count prefix.</summary>
    public static ICodec<TElement[]> FixedArray<TElement>(ICodec<TElement> element, int length)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new FixedArrayCodec<TElement>(element, length);
    }

    /// <summary>Gets a value behind a presence flag that takes <paramref name="absent"/> when the flag is clear and is written when <paramref name="isPresent"/> holds.</summary>
    public static ICodec<TValue> Flagged<TValue>(ICodec<TValue> codec, TValue absent, Func<TValue, bool> isPresent)
    {
        ArgumentNullException.ThrowIfNull(codec);
        ArgumentNullException.ThrowIfNull(isPresent);

        return new DelegateCodec<TValue>(
            stream => stream.ReadBoolean() ? codec.Read(stream) : absent,
            (stream, value) =>
            {
                bool present = isPresent(value);
                stream.WriteBoolean(present);

                if (present)
                    codec.Write(stream, value);
            }
        );
    }

    /// <summary>Gets a codec that resolves another codec on first use.</summary>
    public static ICodec<TValue> Lazy<TValue>(Func<ICodec<TValue>> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        return new LazyCodec<TValue>(factory);
    }

    /// <summary>Gets a value converted to and from another codec's wire form.</summary>
    public static ICodec<TValue> Map<TSource, TValue>(ICodec<TSource> codec, Func<TSource, TValue> fromWire, Func<TValue, TSource> toWire)
    {
        ArgumentNullException.ThrowIfNull(codec);
        ArgumentNullException.ThrowIfNull(fromWire);
        ArgumentNullException.ThrowIfNull(toWire);

        return new MappedCodec<TSource, TValue>(codec, fromWire, toWire);
    }

    /// <summary>Gets a counted array carried as read-only memory; the count defaults to a var-int bounded by the remaining payload.</summary>
    public static ICodec<ReadOnlyMemory<TElement>> Memory<TElement>(ICodec<TElement> element, CollectionCount? count = null)
    {
        return Map(Array(element, count), static values => (ReadOnlyMemory<TElement>)values, static values => values.ToArray());
    }

    /// <summary>Gets a counted array that native writes with a negative count when it is absent.</summary>
    public static ICodec<TElement[]?> NullableArray<TElement>(ICodec<TElement> element, CollectionCount? count = null)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new NullableArrayCodec<TElement>(element, count ?? CollectionCount.VarInt);
    }

    /// <summary>Gets a counted array carried as read-only memory that native writes with a negative count when it is absent.</summary>
    public static ICodec<ReadOnlyMemory<TElement>?> NullableMemory<TElement>(ICodec<TElement> element, CollectionCount? count = null)
    {
        return Map(
            NullableArray(element, count),
            static values => values is null ? null : (ReadOnlyMemory<TElement>?)values,
            static values => values?.ToArray()
        );
    }

    /// <summary>Gets a reference value preceded by a presence flag.</summary>
    public static ICodec<TValue?> Optional<TValue>(ICodec<TValue> codec) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new OptionalCodec<TValue>(codec);
    }

    /// <summary>Gets a value type preceded by a presence flag.</summary>
    public static ICodec<TValue?> OptionalValue<TValue>(ICodec<TValue> codec) where TValue : struct
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new OptionalValueCodec<TValue>(codec);
    }

    /// <summary>Gets a diagnostic field native sends only outside the production command environment; production takes <paramref name="absent"/>.</summary>
    public static ICodec<TValue> OutsideProduction<TValue>(ICodec<TValue> codec, TValue absent)
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new DelegateCodec<TValue>(
            stream => stream.CommandEnvironment is CommandEnvironment.Production ? absent : codec.Read(stream),
            (encoder, value) =>
            {
                if (encoder.CommandEnvironment is not CommandEnvironment.Production)
                    codec.Write(encoder, value);
            }
        );
    }

    /// <summary>Gets a key followed by its value.</summary>
    public static ICodec<KeyValuePair<TKey, TValue>> Pair<TKey, TValue>(ICodec<TKey> key, ICodec<TValue> value)
    {
        return Composite(
            key,
            static pair => pair.Key,
            value,
            static pair => pair.Value,
            static (pairKey, pairValue) => new KeyValuePair<TKey, TValue>(pairKey, pairValue)
        );
    }

    /// <summary>Gets a value that is always present, for use behind a presence flag or at the payload tail.</summary>
    public static ICodec<Presence<TValue>> Present<TValue>(ICodec<TValue> codec)
    {
        return Map(codec, static value => new Presence<TValue>(IsPresent: true, value), static presence => presence.Value);
    }

    /// <summary>Gets a field native sends from <paramref name="version"/> onward; earlier releases take <paramref name="absent"/>.</summary>
    public static ICodec<TValue> Since<TValue>(Version version, ICodec<TValue> codec, TValue absent)
    {
        ArgumentNullException.ThrowIfNull(version);
        ArgumentNullException.ThrowIfNull(codec);

        return new VersionGateCodec<TValue>(version, fromVersion: true, codec, absent);
    }

    /// <summary>Uses a derived type's codec where its base type is expected.</summary>
    public static ICodec<TBase> Subtype<TBase, TDerived>(ICodec<TDerived> codec) where TBase : class where TDerived : class, TBase
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new SubtypeCodec<TBase, TDerived>(codec);
    }

    /// <summary>Gets a tail reference value native reads only while payload bytes remain.</summary>
    public static ICodec<TValue?> Trailing<TValue>(ICodec<TValue> codec) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new TrailingCodec<TValue?>(
            Map<TValue, TValue?>(
                codec,
                static value => value,
                static value => value ?? throw new InvalidDataException(message: "A present trailing value is null.")
            ),
            absent: null,
            static value => value is not null
        );
    }

    /// <summary>Gets a tail field native reads only while payload bytes remain; it takes <paramref name="absent"/> otherwise and is written when <paramref name="isPresent"/> holds.</summary>
    public static ICodec<TValue> Trailing<TValue>(ICodec<TValue> codec, TValue absent, Func<TValue, bool> isPresent)
    {
        ArgumentNullException.ThrowIfNull(codec);
        ArgumentNullException.ThrowIfNull(isPresent);

        return new TrailingCodec<TValue>(codec, absent, isPresent);
    }

    /// <summary>Gets a tail value type native reads only while payload bytes remain.</summary>
    public static ICodec<TValue?> TrailingValue<TValue>(ICodec<TValue> codec) where TValue : struct
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new TrailingCodec<TValue?>(
            Map<TValue, TValue?>(codec, static value => value, static value => value.GetValueOrDefault()),
            absent: null,
            static value => value is not null
        );
    }

    /// <summary>Gets a value without wire fields.</summary>
    public static ICodec<TValue> Unit<TValue>(Func<TValue> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        return new UnitCodec<TValue>(factory);
    }

    /// <summary>Gets a field native sends only before <paramref name="version"/>; later releases take <paramref name="absent"/>.</summary>
    public static ICodec<TValue> Until<TValue>(Version version, ICodec<TValue> codec, TValue absent)
    {
        ArgumentNullException.ThrowIfNull(version);
        ArgumentNullException.ThrowIfNull(codec);

        return new VersionGateCodec<TValue>(version, fromVersion: false, codec, absent);
    }

    /// <summary>Selects a whole wire layout by game version: <paramref name="current"/> from <paramref name="version"/> onward, otherwise <paramref name="legacy"/>.</summary>
    public static ICodec<TValue> Versioned<TValue>(Version version, ICodec<TValue> current, ICodec<TValue> legacy)
    {
        ArgumentNullException.ThrowIfNull(version);
        ArgumentNullException.ThrowIfNull(current);
        ArgumentNullException.ThrowIfNull(legacy);

        return new VersionSwitchCodec<TValue>(version, current, legacy);
    }
}
