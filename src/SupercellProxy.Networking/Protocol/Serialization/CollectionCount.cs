using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization;

/// <summary>How native writes and bounds the element count that prefixes a collection.</summary>
public sealed class CollectionCount
{
    private const int CollectionLimit = 0x10000;

    private readonly int _maximum;
    private readonly int _minimumElementSize;
    private readonly bool _negativeAsEmpty;
    private readonly bool _strictAbsence;
    private readonly CountWidth _width;

    private CollectionCount(CountWidth width, int maximum, int minimumElementSize, bool strictAbsence, bool negativeAsEmpty = false)
    {
        _width = width;
        _maximum = maximum;
        _minimumElementSize = minimumElementSize;
        _strictAbsence = strictAbsence;
        _negativeAsEmpty = negativeAsEmpty;
    }

    private enum CountWidth
    {
        VarInt,
        Int32,
        SignedByte,
    }

    /// <summary>Gets a var-int count of at most 65536 elements, as native's generic collection reader.</summary>
    /// <value>A shared instance that reads and writes the count as a var-int and throws <see cref="InvalidDataException"/> for a count above 65536; it does not compare the count with the unread payload.</value>
    public static CollectionCount Bounded { get; } = new(CountWidth.VarInt, CollectionLimit, minimumElementSize: 0, strictAbsence: false);

    /// <summary>Gets a full 32-bit count no larger than the remaining payload.</summary>
    /// <value>A shared instance that writes the count as a 4-byte integer; a read count larger than the number of unread bytes is rejected, as if every element took at least one byte.</value>
    public static CollectionCount Int32 { get; } = new(CountWidth.Int32, int.MaxValue, minimumElementSize: 1, strictAbsence: false);

    /// <summary>Gets a one-byte signed count of at most 127 elements, no larger than the remaining payload.</summary>
    /// <value>A shared instance that writes the count as a single byte; reading sign-extends that byte, so 0x80 to 0xFF come back negative, and writing more than 127 elements throws <see cref="InvalidDataException"/>.</value>
    public static CollectionCount SignedByte { get; } = new(CountWidth.SignedByte, sbyte.MaxValue, minimumElementSize: 1, strictAbsence: false);

    /// <summary>Gets a var-int count that is not bounded by the payload.</summary>
    /// <value>A shared instance that accepts any non-negative var-int count up to <see cref="int.MaxValue"/>; it is the count <see cref="Encoders.Collection{TElement}"/> uses when none is given.</value>
    public static CollectionCount Unbounded { get; } = new(CountWidth.VarInt, int.MaxValue, minimumElementSize: 0, strictAbsence: false);

    /// <summary>Gets a var-int count no larger than the remaining payload.</summary>
    /// <value>A shared instance that reads and writes a var-int and rejects a read count larger than the number of unread bytes, which stops a corrupt count from allocating a huge collection.</value>
    public static CollectionCount VarInt { get; } = new(CountWidth.VarInt, int.MaxValue, minimumElementSize: 1, strictAbsence: false);

    /// <summary>Gets a var-int count of at most <paramref name="maximum"/> elements.</summary>
    /// <param name="maximum">The largest element count allowed, 0 or more; a larger count throws <see cref="InvalidDataException"/> on both read and write, and 0 allows only empty collections.</param>
    /// <returns>A new var-int count with that cap that does not compare the count with the unread payload.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maximum"/> is negative.</exception>
    public static CollectionCount AtMost(int maximum)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximum);

        return new(CountWidth.VarInt, maximum, minimumElementSize: 0, strictAbsence: false);
    }

    /// <summary>Gets a var-int count bounded by the remaining payload divided by the smallest encoded element.</summary>
    /// <param name="minimumElementSize">The fewest bytes one encoded element can take, at least 1; a read count above the unread byte count divided by this value is rejected as corrupt.</param>
    /// <returns>A new var-int count with that payload check and no cap of its own below <see cref="int.MaxValue"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minimumElementSize"/> is zero or negative.</exception>
    public static CollectionCount PerElement(int minimumElementSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minimumElementSize);

        return new(CountWidth.VarInt, int.MaxValue, minimumElementSize, strictAbsence: false);
    }

    /// <summary>Gets this count capped at <paramref name="maximum"/> elements.</summary>
    /// <param name="maximum">The new largest element count, 0 or more; it replaces this count's cap even when it is larger, and a count above it throws <see cref="InvalidDataException"/> on read and write.</param>
    /// <returns>A new count with the same width, payload check and negative-count handling as this one and the given cap; this instance is left unchanged.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maximum"/> is negative.</exception>
    public CollectionCount With(int maximum)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximum);

        return new(_width, maximum, _minimumElementSize, _strictAbsence, _negativeAsEmpty);
    }

    /// <summary>Gets this count where a negative value reads as an empty collection, as native's clamped readers.</summary>
    /// <returns>A new count, otherwise identical to this one, whose plain (non-nullable) reads return 0 for any negative count instead of throwing <see cref="InvalidDataException"/>.</returns>
    public CollectionCount WithNegativeAsEmpty()
    {
        return new(_width, _maximum, _minimumElementSize, _strictAbsence, negativeAsEmpty: true);
    }

    /// <summary>Gets this count where -1 is the only valid absent marker.</summary>
    /// <returns>A new count, otherwise identical to this one, whose nullable reads treat -1 as absent and throw <see cref="InvalidDataException"/> for any lower value; without it every negative count reads as absent.</returns>
    public CollectionCount WithStrictAbsence()
    {
        return new(_width, _maximum, _minimumElementSize, strictAbsence: true, _negativeAsEmpty);
    }

    internal int Read(MessageStream stream)
    {
        int count = ReadRaw(stream);

        return count >= 0 ? Validate(stream, count) : _negativeAsEmpty ? 0 : throw Invalid(count);
    }

    internal int ReadNullable(MessageStream stream)
    {
        int count = ReadRaw(stream);

        return count < -1 && _strictAbsence ? throw Invalid(count) : count < 0 ? -1 : Validate(stream, count);
    }

    internal void Write(ProtocolEncoder encoder, int value)
    {
        ArgumentNullException.ThrowIfNull(encoder);

        if (value > _maximum)
            throw Invalid(value);

        switch (_width)
        {
            case CountWidth.Int32:
                {
                    encoder.WriteInt32(value);

                    break;
                }
            case CountWidth.SignedByte:
                {
                    encoder.WriteByte(byte.CreateTruncating(value));

                    break;
                }
            case CountWidth.VarInt:
                {
                    encoder.WriteVarInt(value);

                    break;
                }
            default:
                throw new InvalidOperationException(message: "Unknown collection count width.");
        }
    }

    private static InvalidDataException Invalid(int count)
    {
        return new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid collection count: {count}."));
    }

    private int ReadRaw(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return _width switch
        {
            CountWidth.Int32 => stream.ReadInt32(),
            CountWidth.SignedByte => unchecked(sbyte.CreateTruncating(stream.ReadByte())),
            CountWidth.VarInt => stream.ReadVarInt(),
            _ => throw new InvalidOperationException(message: "Unknown collection count width."),
        };
    }

    private int Validate(MessageStream stream, int count)
    {
        long remaining = stream.Length - stream.Position;
        bool overPayload = _minimumElementSize > 0 && count > remaining / _minimumElementSize;

        return count > _maximum || overPayload ? throw Invalid(count) : count;
    }
}
