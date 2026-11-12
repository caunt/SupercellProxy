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
    /// <value>A shared codec that stores each flag as one bit: consecutive booleans fill one byte from its lowest bit upward, and any other field starts a new byte.</value>
    public static ICodec<bool> Boolean { get; } = new DelegateCodec<bool>(static stream => stream.ReadBoolean(), static (stream, value) => stream.WriteBoolean(value));

    /// <summary>Gets one byte.</summary>
    /// <value>A shared codec for a single raw byte from 0 to 255; it also closes any partly filled byte of packed booleans.</value>
    public static ICodec<byte> Byte { get; } = new DelegateCodec<byte>(static stream => stream.ReadByte(), static (stream, value) => stream.WriteByte(value));

    /// <summary>Gets a 32-bit length followed by that many bytes.</summary>
    /// <value>A shared codec whose big-endian length counts the bytes that follow; a zero length reads as empty memory, and a negative one fails with <see cref="InvalidDataException"/>.</value>
    public static ICodec<ReadOnlyMemory<byte>> ByteArray { get; } = new DelegateCodec<ReadOnlyMemory<byte>>(static stream => stream.ReadByteArray(), static (stream, value) => stream.WriteByteArray(value.Span));

    /// <summary>Gets a big-endian 32-bit integer.</summary>
    /// <value>A shared codec for a signed two's-complement integer stored in four bytes, most significant byte first.</value>
    public static ICodec<int> Int32 { get; } = new DelegateCodec<int>(static stream => stream.ReadInt32(), static (stream, value) => stream.WriteInt32(value));

    /// <summary>Gets a big-endian 64-bit integer.</summary>
    /// <value>A shared codec for a signed two's-complement integer stored in eight bytes, most significant byte first.</value>
    public static ICodec<long> Int64 { get; } = new DelegateCodec<long>(static stream => stream.ReadInt64(), static (stream, value) => stream.WriteInt64(value));

    /// <summary>Gets an IEEE single carried in the bits of a big-endian 32-bit integer.</summary>
    /// <value>A shared codec that reinterprets the four bytes bit for bit, so NaN payloads and negative zero survive a round trip unchanged.</value>
    public static ICodec<float> Float { get; } = new DelegateCodec<float>(static stream => BitConverter.Int32BitsToSingle(stream.ReadInt32()),
        static (stream, value) => stream.WriteInt32(BitConverter.SingleToInt32Bits(value)));

    /// <summary>Gets a high/low pair of 32-bit integers.</summary>
    /// <value>A shared codec that writes the high half and then the low half, each as a big-endian <see cref="Int32"/>, for eight bytes in all.</value>
    public static ICodec<LongId> LongId { get; } = new DelegateCodec<LongId>(static stream => stream.ReadLongId(), static (stream, value) => stream.WriteLongId(value));

    /// <summary>Gets a byte array that native writes with a -1 length when it is absent.</summary>
    /// <value>A shared codec with a big-endian 32-bit length: any negative length reads as <see langword="null"/>, zero reads as empty memory, and <see langword="null"/> is written as -1.</value>
    public static ICodec<ReadOnlyMemory<byte>?> OptionalByteArray { get; } = new DelegateCodec<ReadOnlyMemory<byte>?>(static stream => stream.ReadOptionalByteArray(), static (stream, value) => stream.WriteOptionalByteArray(value));

    /// <summary>Gets a presence flag followed by a <see cref="Protocol.LongId"/>.</summary>
    /// <value>A shared codec whose flag is a bit-packed <see cref="Boolean"/>; the eight id bytes follow only when it is set, and a clear flag reads as <see langword="null"/>.</value>
    public static ICodec<LongId?> OptionalLongId { get; } = new DelegateCodec<LongId?>(static stream => stream.ReadOptionalLongId(), static (stream, value) => stream.WriteOptionalLongId(value));

    /// <summary>Gets a string that native writes with a -1 length when it is absent.</summary>
    /// <value>A shared codec with a big-endian 32-bit UTF-8 byte length: any negative length reads as <see langword="null"/>, zero reads as an empty string, and <see langword="null"/> is written as -1.</value>
    public static ICodec<string?> OptionalString { get; } = new DelegateCodec<string?>(static stream => stream.ReadOptionalString(), static (stream, value) => stream.WriteOptionalString(value));

    /// <summary>Gets a length-prefixed UTF-8 string.</summary>
    /// <value>A shared codec whose big-endian 32-bit prefix counts UTF-8 bytes, not characters; a zero length reads as an empty string and a negative one fails with <see cref="InvalidDataException"/>.</value>
    public static ICodec<string> String { get; } = new DelegateCodec<string>(static stream => stream.ReadString(), static (stream, value) => stream.WriteString(value));

    /// <summary>Gets a big-endian 16-bit unsigned integer.</summary>
    /// <value>A shared codec for a value from 0 to 65535 stored in two bytes, most significant byte first.</value>
    public static ICodec<ushort> UInt16 { get; } = new DelegateCodec<ushort>(static stream => stream.ReadUInt16(), static (stream, value) => stream.WriteUInt16(value));

    /// <summary>Gets a big-endian 32-bit unsigned integer.</summary>
    /// <value>A shared codec for an unsigned value stored in four bytes, most significant byte first, with no sign handling.</value>
    public static ICodec<uint> UInt32 { get; } = new DelegateCodec<uint>(static stream => stream.ReadUInt32(), static (stream, value) => stream.WriteUInt32(value));

    /// <summary>Gets a big-endian 64-bit unsigned integer.</summary>
    /// <value>A shared codec for an unsigned value stored in eight bytes, most significant byte first, with no sign handling.</value>
    public static ICodec<ulong> UInt64 { get; } = new DelegateCodec<ulong>(static stream => stream.ReadUInt64(), static (stream, value) => stream.WriteUInt64(value));

    /// <summary>Gets a native variable-length 32-bit integer.</summary>
    /// <value>A shared codec of one to five bytes: the first holds the low 6 bits, a sign flag (0x40) and a continuation flag (0x80), each later byte adds 7 bits, and negative values are sign-extended on read.</value>
    public static ICodec<int> VarInt { get; } = new DelegateCodec<int>(static stream => stream.ReadVarInt(), static (stream, value) => stream.WriteVarInt(value));

    /// <summary>Gets a var-int length followed by that many bytes.</summary>
    /// <value>A shared codec whose <see cref="VarInt"/> prefix counts the bytes that follow; zero reads as empty memory, and a negative length or one above <see cref="MessageStream.MaximumPayloadLength"/> fails with <see cref="InvalidDataException"/>.</value>
    public static ICodec<ReadOnlyMemory<byte>> VarIntByteArray { get; } = new DelegateCodec<ReadOnlyMemory<byte>>(static stream => stream.ReadVarIntByteArray(), static (stream, value) => stream.WriteVarIntByteArray(value.Span));

    /// <summary>Gets a native variable-length 64-bit integer.</summary>
    /// <value>A shared codec that uses the <see cref="VarInt"/> scheme extended to 64 bits; a read whose last byte still carries the continuation flag fails with <see cref="InvalidDataException"/>.</value>
    public static ICodec<long> VarLong { get; } = new DelegateCodec<long>(static stream => stream.ReadVarLong(), static (stream, value) => stream.WriteVarLong(value));

    /// <summary>Gets a counted array of elements; the count defaults to a var-int bounded by the remaining payload.</summary>
    /// <typeparam name="TElement">The type of each array item, which <paramref name="element"/> reads and writes one at a time.</typeparam>
    /// <param name="element">The codec applied to every item in order, after the count prefix.</param>
    /// <param name="count">The width and bounds of the count prefix, or <see langword="null"/> for <see cref="CollectionCount.VarInt"/>, which rejects a count larger than the bytes left in the payload.</param>
    /// <returns>A codec that reads the count and then that many items, and writes the array's length followed by each item; a count outside the bounds of <paramref name="count"/> fails with <see cref="InvalidDataException"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    public static ICodec<TElement[]> Array<TElement>(ICodec<TElement> element, CollectionCount? count = null)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new ArrayCodec<TElement>(element, count ?? CollectionCount.VarInt);
    }

    /// <summary>Gets a structure whose leading key selects the codec of its remaining fields, for wire forms where later fields depend on an earlier one.</summary>
    /// <typeparam name="TKey">The type of the leading field whose value decides how the rest of the structure is laid out.</typeparam>
    /// <typeparam name="TValue">The whole structure, including the leading field, that the returned codec reads and writes.</typeparam>
    /// <param name="key">The codec of the leading field.</param>
    /// <param name="keyOf">Gets the leading field from a value.</param>
    /// <param name="rest">Gets the codec that reads the remaining fields, given the leading field, and builds the whole value.</param>
    /// <returns>A codec that reads the key and then reads the rest with the codec <paramref name="rest"/> gives for it; on write it takes the key from the value, writes it, and writes the rest with the codec chosen for that key.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key"/>, <paramref name="keyOf"/> or <paramref name="rest"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Bind<TKey, TValue>(ICodec<TKey> key, Func<TValue, TKey> keyOf, Func<TKey, ICodec<TValue>> rest)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(keyOf);
        ArgumentNullException.ThrowIfNull(rest);

        return new BindCodec<TKey, TValue>(key, keyOf, rest);
    }

    /// <summary>Gets the contract of a command record: its fields wrapped with the shared command metadata.</summary>
    /// <typeparam name="TCommand">The command record; its type name becomes the contract name used in logs, error messages and JSON.</typeparam>
    /// <param name="fields">The codec of the command's own fields, without the shared metadata.</param>
    /// <param name="layout">Whether the shared metadata follows the fields (the default) or precedes them.</param>
    /// <param name="productionAllowed"><see langword="true"/> if the command may be sent in the production environment, where the command registry refuses debug-only commands; otherwise, <see langword="false"/>.</param>
    /// <returns>A contract named after <typeparamref name="TCommand"/> that reads and writes the fields and the metadata in <paramref name="layout"/> order.</returns>
    public static CommandCodec<TCommand> Command<TCommand>(ICodec<TCommand> fields, CommandLayout layout = CommandLayout.FieldsFirst, bool productionAllowed = true) where TCommand : Command
    {
        return Command(typeof(TCommand).Name, fields, layout, productionAllowed);
    }

    /// <summary>Gets the contract named <paramref name="name"/> of a record that carries several native commands.</summary>
    /// <typeparam name="TCommand">The command record shared by every native command it stands for.</typeparam>
    /// <param name="name">The native command's name, such as <c>CollectDuckCommand</c>, which this contract reports in logs, error messages and JSON in place of the record's name.</param>
    /// <param name="fields">The codec of this native command's own fields, without the shared metadata.</param>
    /// <param name="layout">Whether the shared metadata follows the fields (the default) or precedes them.</param>
    /// <param name="productionAllowed"><see langword="true"/> if this command may be sent in the production environment, as the default assumes, while a debug-only command is refused there by the command registry; otherwise, <see langword="false"/>.</param>
    /// <returns>A contract named <paramref name="name"/> that reads and writes the fields and the metadata in <paramref name="layout"/> order.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> or <paramref name="fields"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty or only white space.</exception>
    public static CommandCodec<TCommand> Command<TCommand>(string name, ICodec<TCommand> fields, CommandLayout layout = CommandLayout.FieldsFirst, bool productionAllowed = true) where TCommand : Command
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(fields);

        return new CommandCodec<TCommand>(name, fields, layout, productionAllowed);
    }

    /// <summary>Gets a payload codec that rejects bytes left after its last field with <paramref name="trailingDataMessage"/>.</summary>
    /// <typeparam name="TValue">The whole payload that <paramref name="codec"/> reads and writes.</typeparam>
    /// <param name="codec">The codec of the payload's fields; it must consume every byte of the stream.</param>
    /// <param name="trailingDataMessage">The message of the <see cref="InvalidDataException"/> thrown when bytes remain after <paramref name="codec"/> has read.</param>
    /// <returns>A codec that reads through <paramref name="codec"/> and fails with <see cref="InvalidDataException"/> unless the stream is then at its end; writing is unchanged.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Complete<TValue>(ICodec<TValue> codec, string trailingDataMessage)
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new CompleteCodec<TValue>(codec, trailingDataMessage);
    }

    /// <summary>Composes 1 field codec: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from its single wire field, such as a command with one argument.</typeparam>
    /// <typeparam name="TFirst">The value type of the only field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the only field.</param>
    /// <param name="getFirst">Gets the field from a value being written.</param>
    /// <param name="factory">Builds the value from the field once it has been read.</param>
    /// <returns>A codec that reads the field and hands it to <paramref name="factory"/>, and writes the field that <paramref name="getFirst"/> takes from the value.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst>(ICodec<TFirst> first, Func<TValue, TFirst> getFirst, Func<TFirst, TValue> factory)
    {
        return new CompositeCodec1<TValue, TFirst>(first, getFirst, factory);
    }

    /// <summary>Composes 2 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the two wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second and last field, which follows the first.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="factory">Builds the value from both fields, passed in wire order, once they have been read.</param>
    /// <returns>A codec that reads the 2 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond>(ICodec<TFirst> first,
        Func<TValue, TFirst> getFirst,
        ICodec<TSecond> second,
        Func<TValue, TSecond> getSecond,
        Func<TFirst, TSecond, TValue> factory)
    {
        return new CompositeCodec2<TValue, TFirst, TSecond>(first, getFirst, second, getSecond, factory);
    }

    /// <summary>Composes 3 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the three wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third and last field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="factory">Builds the value from the three fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 3 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird>(ICodec<TFirst> first,
        Func<TValue, TFirst> getFirst,
        ICodec<TSecond> second,
        Func<TValue, TSecond> getSecond,
        ICodec<TThird> third,
        Func<TValue, TThird> getThird,
        Func<TFirst, TSecond, TThird, TValue> factory)
    {
        return new CompositeCodec3<TValue, TFirst, TSecond, TThird>(first, getFirst, second, getSecond, third, getThird, factory);
    }

    /// <summary>Composes 4 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the four wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth and last field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="factory">Builds the value from the four fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 4 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth>(ICodec<TFirst> first,
        Func<TValue, TFirst> getFirst,
        ICodec<TSecond> second,
        Func<TValue, TSecond> getSecond,
        ICodec<TThird> third,
        Func<TValue, TThird> getThird,
        ICodec<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        Func<TFirst, TSecond, TThird, TFourth, TValue> factory)
    {
        return new CompositeCodec4<TValue, TFirst, TSecond, TThird, TFourth>(first, getFirst, second, getSecond, third, getThird, fourth, getFourth, factory);
    }

    /// <summary>Composes 5 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the five wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth and last field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="factory">Builds the value from the five fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 5 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth>(ICodec<TFirst> first,
        Func<TValue, TFirst> getFirst,
        ICodec<TSecond> second,
        Func<TValue, TSecond> getSecond,
        ICodec<TThird> third,
        Func<TValue, TThird> getThird,
        ICodec<TFourth> fourth,
        Func<TValue, TFourth> getFourth,
        ICodec<TFifth> fifth,
        Func<TValue, TFifth> getFifth,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TValue> factory)
    {
        return new CompositeCodec5<TValue, TFirst, TSecond, TThird, TFourth, TFifth>(first, getFirst, second, getSecond, third, getThird, fourth, getFourth, fifth, getFifth, factory);
    }

    /// <summary>Composes 6 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the six wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth and last field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="factory">Builds the value from the six fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 6 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TValue> factory)
    {
        return new CompositeCodec6<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(first, getFirst, second, getSecond, third, getThird, fourth, getFourth, fifth, getFifth, sixth, getSixth, factory);
    }

    /// <summary>Composes 7 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the seven wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh and last field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="factory">Builds the value from the seven fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 7 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TValue> factory)
    {
        return new CompositeCodec7<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(first,
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
            factory);
    }

    /// <summary>Composes 8 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the eight wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth and last field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="factory">Builds the value from the eight fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 8 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TValue> factory)
    {
        return new CompositeCodec8<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(first,
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
            factory);
    }

    /// <summary>Composes 9 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the nine wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <typeparam name="TNinth">The value type of the ninth field, which <paramref name="ninth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="ninth">The codec of the ninth and last field.</param>
    /// <param name="getNinth">Gets the ninth field from a value being written.</param>
    /// <param name="factory">Builds the value from the nine fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 9 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TValue> factory)
    {
        return new CompositeCodec9<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(first,
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
            factory);
    }

    /// <summary>Composes 10 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the ten wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <typeparam name="TNinth">The value type of the ninth field, which <paramref name="ninth"/> reads and writes.</typeparam>
    /// <typeparam name="TTenth">The value type of the tenth field, which <paramref name="tenth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="ninth">The codec of the ninth field.</param>
    /// <param name="getNinth">Gets the ninth field from a value being written.</param>
    /// <param name="tenth">The codec of the tenth and last field.</param>
    /// <param name="getTenth">Gets the tenth field from a value being written.</param>
    /// <param name="factory">Builds the value from the ten fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 10 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TValue> factory)
    {
        return new CompositeCodec10<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>(first,
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
            factory);
    }

    /// <summary>Composes 11 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the eleven wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <typeparam name="TNinth">The value type of the ninth field, which <paramref name="ninth"/> reads and writes.</typeparam>
    /// <typeparam name="TTenth">The value type of the tenth field, which <paramref name="tenth"/> reads and writes.</typeparam>
    /// <typeparam name="TEleventh">The value type of the eleventh field, which <paramref name="eleventh"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="ninth">The codec of the ninth field.</param>
    /// <param name="getNinth">Gets the ninth field from a value being written.</param>
    /// <param name="tenth">The codec of the tenth field.</param>
    /// <param name="getTenth">Gets the tenth field from a value being written.</param>
    /// <param name="eleventh">The codec of the eleventh and last field.</param>
    /// <param name="getEleventh">Gets the eleventh field from a value being written.</param>
    /// <param name="factory">Builds the value from the eleven fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 11 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TValue> factory)
    {
        return new CompositeCodec11<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>(first,
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
            factory);
    }

    /// <summary>Composes 12 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the twelve wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <typeparam name="TNinth">The value type of the ninth field, which <paramref name="ninth"/> reads and writes.</typeparam>
    /// <typeparam name="TTenth">The value type of the tenth field, which <paramref name="tenth"/> reads and writes.</typeparam>
    /// <typeparam name="TEleventh">The value type of the eleventh field, which <paramref name="eleventh"/> reads and writes.</typeparam>
    /// <typeparam name="TTwelfth">The value type of the twelfth field, which <paramref name="twelfth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="ninth">The codec of the ninth field.</param>
    /// <param name="getNinth">Gets the ninth field from a value being written.</param>
    /// <param name="tenth">The codec of the tenth field.</param>
    /// <param name="getTenth">Gets the tenth field from a value being written.</param>
    /// <param name="eleventh">The codec of the eleventh field.</param>
    /// <param name="getEleventh">Gets the eleventh field from a value being written.</param>
    /// <param name="twelfth">The codec of the twelfth and last field.</param>
    /// <param name="getTwelfth">Gets the twelfth field from a value being written.</param>
    /// <param name="factory">Builds the value from the twelve fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 12 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TValue> factory)
    {
        return new CompositeCodec12<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>(first,
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
            factory);
    }

    /// <summary>Composes 13 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the thirteen wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <typeparam name="TNinth">The value type of the ninth field, which <paramref name="ninth"/> reads and writes.</typeparam>
    /// <typeparam name="TTenth">The value type of the tenth field, which <paramref name="tenth"/> reads and writes.</typeparam>
    /// <typeparam name="TEleventh">The value type of the eleventh field, which <paramref name="eleventh"/> reads and writes.</typeparam>
    /// <typeparam name="TTwelfth">The value type of the twelfth field, which <paramref name="twelfth"/> reads and writes.</typeparam>
    /// <typeparam name="TThirteenth">The value type of the thirteenth field, which <paramref name="thirteenth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="ninth">The codec of the ninth field.</param>
    /// <param name="getNinth">Gets the ninth field from a value being written.</param>
    /// <param name="tenth">The codec of the tenth field.</param>
    /// <param name="getTenth">Gets the tenth field from a value being written.</param>
    /// <param name="eleventh">The codec of the eleventh field.</param>
    /// <param name="getEleventh">Gets the eleventh field from a value being written.</param>
    /// <param name="twelfth">The codec of the twelfth field.</param>
    /// <param name="getTwelfth">Gets the twelfth field from a value being written.</param>
    /// <param name="thirteenth">The codec of the thirteenth and last field.</param>
    /// <param name="getThirteenth">Gets the thirteenth field from a value being written.</param>
    /// <param name="factory">Builds the value from the thirteen fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 13 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TValue> factory)
    {
        return new CompositeCodec13<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>(first,
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
            factory);
    }

    /// <summary>Composes 14 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the fourteen wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <typeparam name="TNinth">The value type of the ninth field, which <paramref name="ninth"/> reads and writes.</typeparam>
    /// <typeparam name="TTenth">The value type of the tenth field, which <paramref name="tenth"/> reads and writes.</typeparam>
    /// <typeparam name="TEleventh">The value type of the eleventh field, which <paramref name="eleventh"/> reads and writes.</typeparam>
    /// <typeparam name="TTwelfth">The value type of the twelfth field, which <paramref name="twelfth"/> reads and writes.</typeparam>
    /// <typeparam name="TThirteenth">The value type of the thirteenth field, which <paramref name="thirteenth"/> reads and writes.</typeparam>
    /// <typeparam name="TFourteenth">The value type of the fourteenth field, which <paramref name="fourteenth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="ninth">The codec of the ninth field.</param>
    /// <param name="getNinth">Gets the ninth field from a value being written.</param>
    /// <param name="tenth">The codec of the tenth field.</param>
    /// <param name="getTenth">Gets the tenth field from a value being written.</param>
    /// <param name="eleventh">The codec of the eleventh field.</param>
    /// <param name="getEleventh">Gets the eleventh field from a value being written.</param>
    /// <param name="twelfth">The codec of the twelfth field.</param>
    /// <param name="getTwelfth">Gets the twelfth field from a value being written.</param>
    /// <param name="thirteenth">The codec of the thirteenth field.</param>
    /// <param name="getThirteenth">Gets the thirteenth field from a value being written.</param>
    /// <param name="fourteenth">The codec of the fourteenth and last field.</param>
    /// <param name="getFourteenth">Gets the fourteenth field from a value being written.</param>
    /// <param name="factory">Builds the value from the fourteen fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 14 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TValue> factory)
    {
        return new CompositeCodec14<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(first,
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
            factory);
    }

    /// <summary>Composes 15 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the fifteen wire fields.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <typeparam name="TNinth">The value type of the ninth field, which <paramref name="ninth"/> reads and writes.</typeparam>
    /// <typeparam name="TTenth">The value type of the tenth field, which <paramref name="tenth"/> reads and writes.</typeparam>
    /// <typeparam name="TEleventh">The value type of the eleventh field, which <paramref name="eleventh"/> reads and writes.</typeparam>
    /// <typeparam name="TTwelfth">The value type of the twelfth field, which <paramref name="twelfth"/> reads and writes.</typeparam>
    /// <typeparam name="TThirteenth">The value type of the thirteenth field, which <paramref name="thirteenth"/> reads and writes.</typeparam>
    /// <typeparam name="TFourteenth">The value type of the fourteenth field, which <paramref name="fourteenth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifteenth">The value type of the fifteenth field, which <paramref name="fifteenth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="ninth">The codec of the ninth field.</param>
    /// <param name="getNinth">Gets the ninth field from a value being written.</param>
    /// <param name="tenth">The codec of the tenth field.</param>
    /// <param name="getTenth">Gets the tenth field from a value being written.</param>
    /// <param name="eleventh">The codec of the eleventh field.</param>
    /// <param name="getEleventh">Gets the eleventh field from a value being written.</param>
    /// <param name="twelfth">The codec of the twelfth field.</param>
    /// <param name="getTwelfth">Gets the twelfth field from a value being written.</param>
    /// <param name="thirteenth">The codec of the thirteenth field.</param>
    /// <param name="getThirteenth">Gets the thirteenth field from a value being written.</param>
    /// <param name="fourteenth">The codec of the fourteenth field.</param>
    /// <param name="getFourteenth">Gets the fourteenth field from a value being written.</param>
    /// <param name="fifteenth">The codec of the fifteenth and last field.</param>
    /// <param name="getFifteenth">Gets the fifteenth field from a value being written.</param>
    /// <param name="factory">Builds the value from the fifteen fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 15 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TValue> factory)
    {
        return new CompositeCodec15<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(first,
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
            factory);
    }

    /// <summary>Composes 16 field codecs: each field is read in order and passed to <paramref name="factory"/>, and written from its getter.</summary>
    /// <typeparam name="TValue">The record built from the sixteen wire fields, the most this combinator composes in one call.</typeparam>
    /// <typeparam name="TFirst">The value type of the first field, which <paramref name="first"/> reads and writes.</typeparam>
    /// <typeparam name="TSecond">The value type of the second field, which <paramref name="second"/> reads and writes.</typeparam>
    /// <typeparam name="TThird">The value type of the third field, which <paramref name="third"/> reads and writes.</typeparam>
    /// <typeparam name="TFourth">The value type of the fourth field, which <paramref name="fourth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifth">The value type of the fifth field, which <paramref name="fifth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixth">The value type of the sixth field, which <paramref name="sixth"/> reads and writes.</typeparam>
    /// <typeparam name="TSeventh">The value type of the seventh field, which <paramref name="seventh"/> reads and writes.</typeparam>
    /// <typeparam name="TEighth">The value type of the eighth field, which <paramref name="eighth"/> reads and writes.</typeparam>
    /// <typeparam name="TNinth">The value type of the ninth field, which <paramref name="ninth"/> reads and writes.</typeparam>
    /// <typeparam name="TTenth">The value type of the tenth field, which <paramref name="tenth"/> reads and writes.</typeparam>
    /// <typeparam name="TEleventh">The value type of the eleventh field, which <paramref name="eleventh"/> reads and writes.</typeparam>
    /// <typeparam name="TTwelfth">The value type of the twelfth field, which <paramref name="twelfth"/> reads and writes.</typeparam>
    /// <typeparam name="TThirteenth">The value type of the thirteenth field, which <paramref name="thirteenth"/> reads and writes.</typeparam>
    /// <typeparam name="TFourteenth">The value type of the fourteenth field, which <paramref name="fourteenth"/> reads and writes.</typeparam>
    /// <typeparam name="TFifteenth">The value type of the fifteenth field, which <paramref name="fifteenth"/> reads and writes.</typeparam>
    /// <typeparam name="TSixteenth">The value type of the sixteenth field, which <paramref name="sixteenth"/> reads and writes.</typeparam>
    /// <param name="first">The codec of the first field, at the start of the structure.</param>
    /// <param name="getFirst">Gets the first field from a value being written.</param>
    /// <param name="second">The codec of the second field.</param>
    /// <param name="getSecond">Gets the second field from a value being written.</param>
    /// <param name="third">The codec of the third field.</param>
    /// <param name="getThird">Gets the third field from a value being written.</param>
    /// <param name="fourth">The codec of the fourth field.</param>
    /// <param name="getFourth">Gets the fourth field from a value being written.</param>
    /// <param name="fifth">The codec of the fifth field.</param>
    /// <param name="getFifth">Gets the fifth field from a value being written.</param>
    /// <param name="sixth">The codec of the sixth field.</param>
    /// <param name="getSixth">Gets the sixth field from a value being written.</param>
    /// <param name="seventh">The codec of the seventh field.</param>
    /// <param name="getSeventh">Gets the seventh field from a value being written.</param>
    /// <param name="eighth">The codec of the eighth field.</param>
    /// <param name="getEighth">Gets the eighth field from a value being written.</param>
    /// <param name="ninth">The codec of the ninth field.</param>
    /// <param name="getNinth">Gets the ninth field from a value being written.</param>
    /// <param name="tenth">The codec of the tenth field.</param>
    /// <param name="getTenth">Gets the tenth field from a value being written.</param>
    /// <param name="eleventh">The codec of the eleventh field.</param>
    /// <param name="getEleventh">Gets the eleventh field from a value being written.</param>
    /// <param name="twelfth">The codec of the twelfth field.</param>
    /// <param name="getTwelfth">Gets the twelfth field from a value being written.</param>
    /// <param name="thirteenth">The codec of the thirteenth field.</param>
    /// <param name="getThirteenth">Gets the thirteenth field from a value being written.</param>
    /// <param name="fourteenth">The codec of the fourteenth field.</param>
    /// <param name="getFourteenth">Gets the fourteenth field from a value being written.</param>
    /// <param name="fifteenth">The codec of the fifteenth field.</param>
    /// <param name="getFifteenth">Gets the fifteenth field from a value being written.</param>
    /// <param name="sixteenth">The codec of the sixteenth and last field.</param>
    /// <param name="getSixteenth">Gets the sixteenth field from a value being written.</param>
    /// <param name="factory">Builds the value from the sixteen fields, passed in wire order, once all have been read.</param>
    /// <returns>A codec that reads the 16 fields in order and builds the value with <paramref name="factory"/>, and writes them in the same order from their getters.</returns>
    public static ICodec<TValue> Composite<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(ICodec<TFirst> first,
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
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth, TValue> factory)
    {
        return new CompositeCodec16<TValue, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(first,
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
            factory);
    }

    /// <summary>Creates a codec from explicit read and write functions, for wire forms whose later fields depend on earlier ones.</summary>
    /// <typeparam name="TValue">The value the two functions read from and write to the wire.</typeparam>
    /// <param name="read">Reads one whole value from the stream, consuming exactly its bytes.</param>
    /// <param name="write">Writes a value to the encoder in the layout <paramref name="read"/> expects.</param>
    /// <returns>A codec that calls <paramref name="read"/> and <paramref name="write"/> directly, after rejecting a <see langword="null"/> stream or encoder with <see cref="ArgumentNullException"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="read"/> or <paramref name="write"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Create<TValue>(Func<MessageStream, TValue> read, Action<ProtocolEncoder, TValue> write)
    {
        ArgumentNullException.ThrowIfNull(read);
        ArgumentNullException.ThrowIfNull(write);

        return new DelegateCodec<TValue>(read, write);
    }

    /// <summary>Gets a tagged value: the key selects the case codec that carries the rest.</summary>
    /// <typeparam name="TKey">The type of the tag written ahead of each case, such as an integer kind or an enum.</typeparam>
    /// <typeparam name="TValue">The common type of every case, usually an abstract record whose variants each have their own codec.</typeparam>
    /// <param name="key">The codec of the tag.</param>
    /// <param name="keyOf">Gets the tag that identifies a value's case when it is written.</param>
    /// <param name="cases">Each tag paired with the codec of the fields that follow it; every tag must be distinct.</param>
    /// <returns>A codec that reads the tag and then the case it names, and writes a value's tag followed by its case; a tag with no case fails with <see cref="InvalidDataException"/> in either direction.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key"/>, <paramref name="keyOf"/> or <paramref name="cases"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Dispatch<TKey, TValue>(ICodec<TKey> key, Func<TValue, TKey> keyOf, IEnumerable<KeyValuePair<TKey, ICodec<TValue>>> cases)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(keyOf);
        ArgumentNullException.ThrowIfNull(cases);

        return new DispatchCodec<TKey, TValue>(key, keyOf, cases.ToFrozenDictionary());
    }

    /// <summary>Gets an int-backed enum carried by an integer codec.</summary>
    /// <typeparam name="TEnum">The enum whose underlying type is <see cref="int"/>; its members' numeric values are what goes on the wire.</typeparam>
    /// <param name="underlying">The integer wire form, such as <see cref="Int32"/> or <see cref="VarInt"/>.</param>
    /// <returns>A codec that reinterprets the integer as <typeparamref name="TEnum"/> bit for bit, so numbers without a named member pass through unchanged instead of failing.</returns>
    public static ICodec<TEnum> Enum<TEnum>(ICodec<int> underlying) where TEnum : struct, Enum
    {
        return Map(underlying, static value => Unsafe.BitCast<int, TEnum>(value), static value => Unsafe.BitCast<TEnum, int>(value));
    }

    /// <summary>Gets exactly <paramref name="length"/> elements without a count prefix.</summary>
    /// <typeparam name="TElement">The type of each array item, which <paramref name="element"/> reads and writes.</typeparam>
    /// <param name="element">The codec applied to every item in order.</param>
    /// <param name="length">The number of items the layout always holds; native writes no count for them.</param>
    /// <returns>A codec that reads <paramref name="length"/> items into a new array, and writes an array only when it has exactly that many items, failing with <see cref="InvalidDataException"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    public static ICodec<TElement[]> FixedArray<TElement>(ICodec<TElement> element, int length)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new FixedArrayCodec<TElement>(element, length);
    }

    /// <summary>Gets a value behind a presence flag that takes <paramref name="absent"/> when the flag is clear and is written when <paramref name="isPresent"/> holds.</summary>
    /// <typeparam name="TValue">The value behind the flag, which may be a value type with a sentinel for absence.</typeparam>
    /// <param name="codec">The codec of the value, used only when the flag is set.</param>
    /// <param name="absent">The value a read produces when the flag is clear, such as 0 or an empty record.</param>
    /// <param name="isPresent">Decides from a value being written whether to set the flag and write the value; returning <see langword="false"/> writes only a clear flag.</param>
    /// <returns>A codec that reads a bit-packed flag and then the value only when the flag is set, and writes the flag from <paramref name="isPresent"/> followed by the value when it holds.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> or <paramref name="isPresent"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Flagged<TValue>(ICodec<TValue> codec, TValue absent, Func<TValue, bool> isPresent)
    {
        ArgumentNullException.ThrowIfNull(codec);
        ArgumentNullException.ThrowIfNull(isPresent);

        return new DelegateCodec<TValue>(stream => stream.ReadBoolean() ? codec.Read(stream) : absent,
            (stream, value) =>
            {
                bool present = isPresent(value);
                stream.WriteBoolean(present);

                if (present)
                    codec.Write(stream, value);
            });
    }

    /// <summary>Gets a codec that resolves another codec on first use.</summary>
    /// <typeparam name="TValue">The value the resolved codec reads and writes.</typeparam>
    /// <param name="factory">Returns the codec to delegate to; it runs once, on the first read or write, so it may name codecs that are initialized later, including the one being defined.</param>
    /// <returns>A codec that forwards every read and write to the codec <paramref name="factory"/> returned, which lets recursive or mutually dependent structures refer to each other.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Lazy<TValue>(Func<ICodec<TValue>> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        return new LazyCodec<TValue>(factory);
    }

    /// <summary>Gets a value converted to and from another codec's wire form.</summary>
    /// <typeparam name="TSource">The wire form that <paramref name="codec"/> reads and writes, such as an integer or a raw record.</typeparam>
    /// <typeparam name="TValue">The model type callers see in place of <typeparamref name="TSource"/>.</typeparam>
    /// <param name="codec">The codec of the wire form; the bytes on the wire are exactly what it reads and writes.</param>
    /// <param name="fromWire">Converts a value just read into the model type.</param>
    /// <param name="toWire">Converts a model value back into the wire form before it is written.</param>
    /// <returns>A codec with the same wire layout as <paramref name="codec"/> that passes each read through <paramref name="fromWire"/> and each write through <paramref name="toWire"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/>, <paramref name="fromWire"/> or <paramref name="toWire"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Map<TSource, TValue>(ICodec<TSource> codec, Func<TSource, TValue> fromWire, Func<TValue, TSource> toWire)
    {
        ArgumentNullException.ThrowIfNull(codec);
        ArgumentNullException.ThrowIfNull(fromWire);
        ArgumentNullException.ThrowIfNull(toWire);

        return new MappedCodec<TSource, TValue>(codec, fromWire, toWire);
    }

    /// <summary>Gets a counted array carried as read-only memory; the count defaults to a var-int bounded by the remaining payload.</summary>
    /// <typeparam name="TElement">The type of each item in the memory, which <paramref name="element"/> reads and writes.</typeparam>
    /// <param name="element">The codec applied to every item in order, after the count prefix.</param>
    /// <param name="count">The width and bounds of the count prefix, or <see langword="null"/> for <see cref="CollectionCount.VarInt"/>.</param>
    /// <returns>A codec with the same layout and count checks as the counted array codec, that reads into memory over a new array and copies the memory to an array before writing it.</returns>
    public static ICodec<ReadOnlyMemory<TElement>> Memory<TElement>(ICodec<TElement> element, CollectionCount? count = null)
    {
        return Map(Array(element, count), static values => (ReadOnlyMemory<TElement>)values, static values => values.ToArray());
    }

    /// <summary>Gets a counted array that native writes with a negative count when it is absent.</summary>
    /// <typeparam name="TElement">The type of each array item, which <paramref name="element"/> reads and writes.</typeparam>
    /// <param name="element">The codec applied to every item in order, after the count prefix.</param>
    /// <param name="count">The width and bounds of the count prefix, or <see langword="null"/> for <see cref="CollectionCount.VarInt"/>; with <see cref="CollectionCount.WithStrictAbsence"/> only -1 is accepted as the absent marker.</param>
    /// <returns>A codec that reads a negative count as a <see langword="null"/> array and any other count as that many items, and writes <see langword="null"/> as a count of -1.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    public static ICodec<TElement[]?> NullableArray<TElement>(ICodec<TElement> element, CollectionCount? count = null)
    {
        ArgumentNullException.ThrowIfNull(element);

        return new NullableArrayCodec<TElement>(element, count ?? CollectionCount.VarInt);
    }

    /// <summary>Gets a counted array carried as read-only memory that native writes with a negative count when it is absent.</summary>
    /// <typeparam name="TElement">The type of each item in the memory, which <paramref name="element"/> reads and writes.</typeparam>
    /// <param name="element">The codec applied to every item in order, after the count prefix.</param>
    /// <param name="count">The width and bounds of the count prefix, or <see langword="null"/> for <see cref="CollectionCount.VarInt"/>.</param>
    /// <returns>A codec that reads a negative count as <see langword="null"/> and otherwise as memory over the items read, and writes <see langword="null"/> as a count of -1; an empty but present collection keeps its zero count.</returns>
    public static ICodec<ReadOnlyMemory<TElement>?> NullableMemory<TElement>(ICodec<TElement> element, CollectionCount? count = null)
    {
        return Map(NullableArray(element, count),
            static values => values is null ? null : (ReadOnlyMemory<TElement>?)values,
            static values => values?.ToArray());
    }

    /// <summary>Gets a reference value preceded by a presence flag.</summary>
    /// <typeparam name="TValue">The reference type behind the flag, such as a nested record or a string.</typeparam>
    /// <param name="codec">The codec of the value, used only when the flag is set.</param>
    /// <returns>A codec that reads a bit-packed flag and then the value when the flag is set, or <see langword="null"/> when it is clear, and writes a set flag and the value for any non-<see langword="null"/> value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue?> Optional<TValue>(ICodec<TValue> codec) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new OptionalCodec<TValue>(codec);
    }

    /// <summary>Gets a value type preceded by a presence flag.</summary>
    /// <typeparam name="TValue">The value type behind the flag, such as an integer or a <see cref="Protocol.LongId"/>.</typeparam>
    /// <param name="codec">The codec of the value, used only when the flag is set.</param>
    /// <returns>A codec that reads a bit-packed flag and then the value when the flag is set, or <see langword="null"/> when it is clear, and writes a set flag and the value whenever the nullable has a value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue?> OptionalValue<TValue>(ICodec<TValue> codec) where TValue : struct
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new OptionalValueCodec<TValue>(codec);
    }

    /// <summary>Gets a diagnostic field native sends only outside the production command environment; production takes <paramref name="absent"/>.</summary>
    /// <typeparam name="TValue">The value of the diagnostic field.</typeparam>
    /// <param name="codec">The codec of the field in development, stage and other non-production environments.</param>
    /// <param name="absent">The value a read produces in <see cref="CommandEnvironment.Production"/>, where the field has no bytes.</param>
    /// <returns>A codec that reads and writes the field only when the stream's or encoder's command environment is not production (production is the encoder default); in production it reads <paramref name="absent"/> and writes nothing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> OutsideProduction<TValue>(ICodec<TValue> codec, TValue absent)
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new DelegateCodec<TValue>(stream => stream.CommandEnvironment is CommandEnvironment.Production ? absent : codec.Read(stream),
            (encoder, value) =>
            {
                if (encoder.CommandEnvironment is not CommandEnvironment.Production)
                    codec.Write(encoder, value);
            });
    }

    /// <summary>Gets a key followed by its value.</summary>
    /// <typeparam name="TKey">The type of the leading key, such as a data id.</typeparam>
    /// <typeparam name="TValue">The type of the value that follows the key.</typeparam>
    /// <param name="key">The codec of the key, which comes first on the wire.</param>
    /// <param name="value">The codec of the value, which follows the key directly.</param>
    /// <returns>A codec for one <see cref="KeyValuePair{TKey, TValue}"/>, written as the key's bytes followed at once by the value's, and typically used as the element of an array of key/value entries.</returns>
    public static ICodec<KeyValuePair<TKey, TValue>> Pair<TKey, TValue>(ICodec<TKey> key, ICodec<TValue> value)
    {
        return Composite(key,
            static pair => pair.Key,
            value,
            static pair => pair.Value,
            static (pairKey, pairValue) => new KeyValuePair<TKey, TValue>(pairKey, pairValue));
    }

    /// <summary>Gets a value that is always present, for use behind a presence flag or at the payload tail.</summary>
    /// <typeparam name="TValue">The value wrapped in a <see cref="Presence{TValue}"/>.</typeparam>
    /// <param name="codec">The codec of the value itself.</param>
    /// <returns>A codec with the wire layout of <paramref name="codec"/> that reads every value as present and writes <see cref="Presence{TValue}.Value"/> without looking at <see cref="Presence{TValue}.IsPresent"/>, so the surrounding flag or tail codec decides absence.</returns>
    public static ICodec<Presence<TValue>> Present<TValue>(ICodec<TValue> codec)
    {
        return Map(codec, static value => new Presence<TValue>(IsPresent: true, value), static presence => presence.Value);
    }

    /// <summary>Gets a field native sends from <paramref name="version"/> onward; earlier releases take <paramref name="absent"/>.</summary>
    /// <typeparam name="TValue">The value of the field that newer releases added.</typeparam>
    /// <param name="version">The first game release whose wire layout carries the field.</param>
    /// <param name="codec">The codec of the field in releases that carry it.</param>
    /// <param name="absent">The value a read produces for an older release, where the field has no bytes.</param>
    /// <returns>A codec that reads and writes the field when the stream's or encoder's game version is at least <paramref name="version"/>, and otherwise reads <paramref name="absent"/> and writes nothing; an unknown (<see langword="null"/>) game version counts as older.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="version"/> or <paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Since<TValue>(Version version, ICodec<TValue> codec, TValue absent)
    {
        ArgumentNullException.ThrowIfNull(version);
        ArgumentNullException.ThrowIfNull(codec);

        return new VersionGateCodec<TValue>(version, fromVersion: true, codec, absent);
    }

    /// <summary>Uses a derived type's codec where its base type is expected.</summary>
    /// <typeparam name="TBase">The base type the returned codec exposes, such as the case type of a <see cref="Dispatch{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TDerived">The concrete type that <paramref name="codec"/> actually reads and writes.</typeparam>
    /// <param name="codec">The codec of the derived type.</param>
    /// <returns>A codec that returns each value read by <paramref name="codec"/> as <typeparamref name="TBase"/>, and writes a base value only when it is a <typeparamref name="TDerived"/>, failing with <see cref="InvalidDataException"/> for any other type.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TBase> Subtype<TBase, TDerived>(ICodec<TDerived> codec) where TBase : class where TDerived : class, TBase
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new SubtypeCodec<TBase, TDerived>(codec);
    }

    /// <summary>Gets a tail reference value native reads only while payload bytes remain.</summary>
    /// <typeparam name="TValue">The reference type of the optional tail field.</typeparam>
    /// <param name="codec">The codec of the field when the payload still has bytes for it.</param>
    /// <returns>A codec that reads the field when the stream has bytes left and <see langword="null"/> at its end, and writes the field only when it is not <see langword="null"/>; it belongs at the end of a payload, after every always-present field.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue?> Trailing<TValue>(ICodec<TValue> codec) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new TrailingCodec<TValue?>(Map<TValue, TValue?>(codec,
                static value => value,
                static value => value ?? throw new InvalidDataException(message: "A present trailing value is null.")),
            absent: null,
            static value => value is not null);
    }

    /// <summary>Gets a tail field native reads only while payload bytes remain; it takes <paramref name="absent"/> otherwise and is written when <paramref name="isPresent"/> holds.</summary>
    /// <typeparam name="TValue">The value of the optional tail field, often a <see cref="Presence{TValue}"/> so that a present but empty value can still be told apart.</typeparam>
    /// <param name="codec">The codec of the field when the payload still has bytes for it.</param>
    /// <param name="absent">The value a read produces when the stream is already at its end.</param>
    /// <param name="isPresent">Decides from a value being written whether to write it; returning <see langword="false"/> ends the payload before this field.</param>
    /// <returns>A codec that reads the field only while the stream has bytes left, otherwise producing <paramref name="absent"/>, and writes it only when <paramref name="isPresent"/> holds; no flag or length is written.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> or <paramref name="isPresent"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Trailing<TValue>(ICodec<TValue> codec, TValue absent, Func<TValue, bool> isPresent)
    {
        ArgumentNullException.ThrowIfNull(codec);
        ArgumentNullException.ThrowIfNull(isPresent);

        return new TrailingCodec<TValue>(codec, absent, isPresent);
    }

    /// <summary>Gets a tail value type native reads only while payload bytes remain.</summary>
    /// <typeparam name="TValue">The value type of the optional tail field.</typeparam>
    /// <param name="codec">The codec of the field when the payload still has bytes for it.</param>
    /// <returns>A codec that reads the field when the stream has bytes left and <see langword="null"/> at its end, and writes the field only when the nullable has a value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue?> TrailingValue<TValue>(ICodec<TValue> codec) where TValue : struct
    {
        ArgumentNullException.ThrowIfNull(codec);

        return new TrailingCodec<TValue?>(Map<TValue, TValue?>(codec, static value => value, static value => value.GetValueOrDefault()),
            absent: null,
            static value => value is not null);
    }

    /// <summary>Gets a value without wire fields.</summary>
    /// <typeparam name="TValue">The value, typically a command or message record with no arguments.</typeparam>
    /// <param name="factory">Creates the value; it is called on every read.</param>
    /// <returns>A codec that consumes no bytes, returning a fresh value from <paramref name="factory"/> on each read, and writes nothing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Unit<TValue>(Func<TValue> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        return new UnitCodec<TValue>(factory);
    }

    /// <summary>Gets a field native sends only before <paramref name="version"/>; later releases take <paramref name="absent"/>.</summary>
    /// <typeparam name="TValue">The value of the field that newer releases removed.</typeparam>
    /// <param name="version">The first game release whose wire layout no longer carries the field.</param>
    /// <param name="codec">The codec of the field in releases older than <paramref name="version"/>.</param>
    /// <param name="absent">The value a read produces from <paramref name="version"/> onward, where the field has no bytes.</param>
    /// <returns>A codec that reads and writes the field when the stream's or encoder's game version is below <paramref name="version"/>, and otherwise reads <paramref name="absent"/> and writes nothing; an unknown (<see langword="null"/>) game version counts as older, so the field is kept.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="version"/> or <paramref name="codec"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Until<TValue>(Version version, ICodec<TValue> codec, TValue absent)
    {
        ArgumentNullException.ThrowIfNull(version);
        ArgumentNullException.ThrowIfNull(codec);

        return new VersionGateCodec<TValue>(version, fromVersion: false, codec, absent);
    }

    /// <summary>Selects a whole wire layout by game version: <paramref name="current"/> from <paramref name="version"/> onward, otherwise <paramref name="legacy"/>.</summary>
    /// <typeparam name="TValue">The value both layouts read and write.</typeparam>
    /// <param name="version">The first game release that uses <paramref name="current"/>.</param>
    /// <param name="current">The layout of <paramref name="version"/> and every later release.</param>
    /// <param name="legacy">The layout of releases older than <paramref name="version"/>.</param>
    /// <returns>A codec that picks the layout per read or write from the stream's or encoder's game version; an unknown (<see langword="null"/>) game version gets <paramref name="legacy"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="version"/>, <paramref name="current"/> or <paramref name="legacy"/> is <see langword="null"/>.</exception>
    public static ICodec<TValue> Versioned<TValue>(Version version, ICodec<TValue> current, ICodec<TValue> legacy)
    {
        ArgumentNullException.ThrowIfNull(version);
        ArgumentNullException.ThrowIfNull(current);
        ArgumentNullException.ThrowIfNull(legacy);

        return new VersionSwitchCodec<TValue>(version, current, legacy);
    }
}
