using System.Buffers.Binary;
using System.Text;

namespace Playground;

public class ScStream(byte[] buffer) : IDisposable
{
    public int Position { get; private set; } = 0;
    public int Length => buffer.Length;
    public Span<byte> Span => buffer;

    protected int _booleanOffset;
    protected byte _booleanAdditionalValue;

    public byte ReadByte()
    {
        ResetBoolean();

        if (Position + 1 > Length)
            throw new EndOfStreamException();

        return buffer[Position++];
    }

    public Span<byte> ReadBytes(int count)
    {
        ResetBoolean();

        if (Position + count > Length)
            throw new EndOfStreamException();

        var span = buffer.AsSpan(Position, count);
        Position += count;

        return span;
    }

    public Span<byte> ReadByteArray()
    {
        var length = ReadInt32();

        if (length == 0)
            return [];

        if (length < 0)
            throw new InvalidDataException("Negative length for byte array.");

        return ReadBytes(length);
    }

    public Span<byte> ReadToEnd()
    {
        return ReadBytes(Length - Position);
    }

    public bool ReadBoolean()
    {
        if (_booleanOffset == 0)
            _booleanAdditionalValue = ReadByte();

        var value = ((_booleanAdditionalValue >> _booleanOffset) & 1) != 0;
        _booleanOffset = (_booleanOffset + 1) & 7;

        return value;
    }

    public ushort ReadUInt16()
    {
        return BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
    }

    public int ReadInt32()
    {
        return BinaryPrimitives.ReadInt32BigEndian(ReadBytes(4));
    }

    public string? ReadString()
    {
        var length = ReadInt32();

        if (length < 0)
            return null;

        return Encoding.UTF8.GetString(ReadBytes(length));
    }

    public int ReadVarInt()
    {
        var firstByte = ReadByte();
        var isNegative = (firstByte & 0x40) != 0;
        var accumulator = firstByte & 0x3FL;
        var consumedBitWidth = 6;

        var currentByte = firstByte;
        while ((currentByte & 0x80) != 0 && consumedBitWidth < 64)
        {
            currentByte = ReadByte();
            accumulator |= (long)(currentByte & 0x7F) << consumedBitWidth;
            consumedBitWidth += 7;
        }

        if (isNegative)
        {
            var twoComplementBase = 1L << consumedBitWidth;
            accumulator -= twoComplementBase;
        }

        return (int)accumulator;
    }

    private void ResetBoolean()
    {
        if (_booleanOffset <= 0)
            return;

        _booleanOffset = 0;
        _booleanAdditionalValue = 0;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (Position >= Length)
            return;

        throw new InvalidOperationException($"Read operation incomplete: {Position} / {Length} bytes read.\n{BitConverter.ToString(buffer).Replace('-', ' ').Insert(Position * 3, "> ")}");
    }
}
