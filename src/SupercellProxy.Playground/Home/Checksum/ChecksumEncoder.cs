using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home.Checksum;

/// <summary>
/// <para>Hay Day 1.72.84's state checksum accumulator.</para>
/// </summary>
internal sealed class ChecksumEncoder
{
    public int Checksum { get; private set; }

    public void Reset()
    {
        Checksum = 0;
    }

    public void WriteNullableString(string? value)
    {
        Accumulate(value is null ? 27 : GetUnicodeScalarCount(value) + 28);
    }

    public void WriteString(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Accumulate(GetUnicodeScalarCount(value) + 38);
    }

    public void WriteBoolean(bool value)
    {
        Accumulate(value ? 13 : 7);
    }

    public void WriteInt32(int value)
    {
        Accumulate(value, 9);
    }

    public void WriteInt8(sbyte value)
    {
        Accumulate(byte.CreateTruncating(value), 11);
    }

    public void WriteInt16(short value)
    {
        Accumulate(ushort.CreateTruncating(value), 19);
    }

    public void WriteInt24(int value)
    {
        if (value is < -0x800000 or > 0x7fffff)
            throw new ArgumentOutOfRangeException(nameof(value));

        Accumulate(value & 0xffffff, 21);
    }

    public void WriteNullableByteArray(ReadOnlyMemory<byte>? value)
    {
        Accumulate(value is null ? 37 : value.Value.Length + 38);
    }

    public void WriteUInt8(byte value)
    {
        Accumulate(value, 11);
    }

    public void WriteUInt16(ushort value)
    {
        Accumulate(value, 19);
    }

    public void WriteVarInt(int value)
    {
        Accumulate(value, 33);
    }

    public void WriteInt64(long value)
    {
        unchecked
        {
            Accumulate(int.CreateTruncating((value >> 32)), 65);
            Accumulate(int.CreateTruncating(value), 88);
        }
    }

    public void WriteVarLong(long value)
    {
        unchecked
        {
            Accumulate(int.CreateTruncating((value >> 32)), 67);
            Accumulate(int.CreateTruncating(value), 91);
        }
    }

    public void WriteLongId(LongId value)
    {
        WriteInt32(value.HighInt32);
        WriteInt32(value.LowInt32);
    }

    private void Accumulate(int value)
    {
        unchecked
        {
            Checksum = int.RotateLeft(Checksum, 1) + value;
        }
    }

    private void Accumulate(int value, int discriminator)
    {
        unchecked
        {
            Checksum = int.RotateLeft(Checksum, 1) + value + discriminator;
        }
    }

    private static int GetUnicodeScalarCount(string value)
    {
        return value.EnumerateRunes().Count();
    }
}
