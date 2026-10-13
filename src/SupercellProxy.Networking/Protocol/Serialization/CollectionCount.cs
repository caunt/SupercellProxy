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
    public static CollectionCount Bounded { get; } = new(CountWidth.VarInt, CollectionLimit, minimumElementSize: 0, strictAbsence: false);

    /// <summary>Gets a full 32-bit count no larger than the remaining payload.</summary>
    public static CollectionCount Int32 { get; } = new(CountWidth.Int32, int.MaxValue, minimumElementSize: 1, strictAbsence: false);

    /// <summary>Gets a one-byte signed count of at most 127 elements, no larger than the remaining payload.</summary>
    public static CollectionCount SignedByte { get; } = new(CountWidth.SignedByte, sbyte.MaxValue, minimumElementSize: 1, strictAbsence: false);

    /// <summary>Gets a var-int count that is not bounded by the payload.</summary>
    public static CollectionCount Unbounded { get; } = new(CountWidth.VarInt, int.MaxValue, minimumElementSize: 0, strictAbsence: false);

    /// <summary>Gets a var-int count no larger than the remaining payload.</summary>
    public static CollectionCount VarInt { get; } = new(CountWidth.VarInt, int.MaxValue, minimumElementSize: 1, strictAbsence: false);

    /// <summary>Gets a var-int count of at most <paramref name="maximum"/> elements.</summary>
    public static CollectionCount AtMost(int maximum)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximum);

        return new(CountWidth.VarInt, maximum, minimumElementSize: 0, strictAbsence: false);
    }

    /// <summary>Gets a var-int count bounded by the remaining payload divided by the smallest encoded element.</summary>
    public static CollectionCount PerElement(int minimumElementSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minimumElementSize);

        return new(CountWidth.VarInt, int.MaxValue, minimumElementSize, strictAbsence: false);
    }

    /// <summary>Gets this count capped at <paramref name="maximum"/> elements.</summary>
    public CollectionCount WithMaximum(int maximum)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximum);

        return new(_width, maximum, _minimumElementSize, _strictAbsence, _negativeAsEmpty);
    }

    /// <summary>Gets this count where a negative value reads as an empty collection, as native's clamped readers.</summary>
    public CollectionCount WithNegativeAsEmpty()
    {
        return new(_width, _maximum, _minimumElementSize, _strictAbsence, negativeAsEmpty: true);
    }

    /// <summary>Gets this count where -1 is the only valid absent marker.</summary>
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
