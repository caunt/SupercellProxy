using SupercellProxy.Playground.Supercell;
using System.Buffers.Binary;
using System.Text;

namespace SupercellProxy.Playground.Network.Streams;

public partial class SupercellStream
{
    public bool CanRead => stream.CanRead;

    private int _booleanReadOffset;
    private byte _booleanReadAdditionalValue;

    public byte ReadByte()
    {
        ResetReadBoolean();

        var value = stream.ReadByte();

        if (value < 0)
            throw new EndOfStreamException();

        return (byte)value;
    }

    public Span<byte> ReadExactly(Span<byte> buffer)
    {
        ResetReadBoolean();
        stream.ReadExactly(buffer);
        return buffer;
    }

    public async ValueTask<Memory<byte>> ReadExactlyAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        await stream.ReadExactlyAsync(buffer, cancellationToken).AsTask().WaitAsync(cancellationToken);
        return buffer;
    }

    public Memory<byte> ReadToEnd()
    {
        var buffer = new byte[Length - Position];
        ReadExactly(buffer);
        return buffer;
    }

    public Memory<byte> ReadByteArray()
    {
        var length = ReadInt32();

        if (length is 0)
            return Memory<byte>.Empty;

        if (length < 0)
            throw new InvalidDataException("Negative length for byte array.");

        var buffer = new byte[length];
        ReadExactly(buffer);

        return buffer;
    }

    public async ValueTask<Memory<byte>> ReadByteArrayAsync(CancellationToken cancellationToken = default)
    {
        var length = await ReadInt32Async(cancellationToken);

        if (length is 0)
            return Memory<byte>.Empty;

        if (length < 0)
            throw new InvalidDataException("Negative length for byte array.");

        return await ReadExactlyAsync(new byte[length], cancellationToken);
    }

    public bool ReadBoolean()
    {
        if (_booleanReadOffset == 0)
            _booleanReadAdditionalValue = ReadByte();

        var value = ((_booleanReadAdditionalValue >> _booleanReadOffset) & 1) != 0;
        _booleanReadOffset = (_booleanReadOffset + 1) & 7;

        return value;
    }

    public ushort ReadUInt16()
    {
        return BinaryPrimitives.ReadUInt16BigEndian(ReadExactly(stackalloc byte[sizeof(ushort)]));
    }

    public async ValueTask<ushort> ReadUInt16Async(CancellationToken cancellationToken = default)
    {
        var memory = await ReadExactlyAsync(RentExactly(sizeof(ushort)), cancellationToken);
        return BinaryPrimitives.ReadUInt16BigEndian(memory.Span);
    }

    public int ReadInt32()
    {
        return BinaryPrimitives.ReadInt32BigEndian(ReadExactly(stackalloc byte[sizeof(int)]));
    }

    public async ValueTask<int> ReadInt32Async(CancellationToken cancellationToken = default)
    {
        var memory = await ReadExactlyAsync(RentExactly(sizeof(int)), cancellationToken);
        return BinaryPrimitives.ReadInt32BigEndian(memory.Span);
    }

    public uint ReadUInt32()
    {
        return BinaryPrimitives.ReadUInt32BigEndian(ReadExactly(stackalloc byte[sizeof(uint)]));
    }

    public async ValueTask<uint> ReadUInt32Async(CancellationToken cancellationToken = default)
    {
        var memory = await ReadExactlyAsync(RentExactly(sizeof(uint)), cancellationToken);
        return BinaryPrimitives.ReadUInt32BigEndian(memory.Span);
    }

    public long ReadInt64()
    {
        return BinaryPrimitives.ReadInt64BigEndian(ReadExactly(stackalloc byte[sizeof(long)]));
    }

    public async ValueTask<long> ReadInt64Async(CancellationToken cancellationToken = default)
    {
        var memory = await ReadExactlyAsync(RentExactly(sizeof(ulong)), cancellationToken);
        return BinaryPrimitives.ReadInt64BigEndian(memory.Span);
    }

    public ulong ReadUInt64()
    {
        return BinaryPrimitives.ReadUInt64BigEndian(ReadExactly(stackalloc byte[sizeof(long)]));
    }

    public async ValueTask<ulong> ReadUInt64Async(CancellationToken cancellationToken = default)
    {
        var memory = await ReadExactlyAsync(RentExactly(sizeof(ulong)), cancellationToken);
        return BinaryPrimitives.ReadUInt64BigEndian(memory.Span);
    }

    public string? ReadOptionalString()
    {
        var length = ReadInt32();

        if (length < 0)
            return null;

        if (length is 0)
            return string.Empty;

        Span<byte> buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];
        ReadExactly(buffer);
        return Encoding.UTF8.GetString(buffer);
    }

    public string ReadString()
    {
        var length = ReadInt32();

        if (length < 0)
            throw new InvalidDataException("Negative length for string array.");

        if (length is 0)
            return string.Empty;

        Span<byte> buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];
        ReadExactly(buffer);
        return Encoding.UTF8.GetString(buffer);
    }

    public async ValueTask<string?> ReadOptionalStringAsync(CancellationToken cancellationToken = default)
    {
        var length = await ReadInt32Async(cancellationToken);

        if (length < 0)
            return null;

        if (length is 0)
            return string.Empty;

        var memory = await ReadExactlyAsync(RentExactly(length), cancellationToken);
        return Encoding.UTF8.GetString(memory.Span);
    }

    public async ValueTask<string> ReadStringAsync(CancellationToken cancellationToken = default)
    {
        var length = await ReadInt32Async(cancellationToken);

        if (length < 0)
            throw new InvalidDataException("Negative length for string array.");

        if (length is 0)
            return string.Empty;

        var memory = await ReadExactlyAsync(RentExactly(length), cancellationToken);
        return Encoding.UTF8.GetString(memory.Span);
    }

    public int ReadVarInt()
    {
        var firstByte = ReadByte();
        var isNegative = (firstByte & 0x40) != 0;
        var accumulator = firstByte & 0x3F;
        var consumedBitWidth = 6;

        var currentByte = firstByte;
        while ((currentByte & 0x80) != 0 && consumedBitWidth < 35)
        {
            currentByte = ReadByte();
            accumulator |= (currentByte & 0x7F) << consumedBitWidth;
            consumedBitWidth += 7;
        }

        if (isNegative)
        {
            if (consumedBitWidth == 34)
            {
                accumulator |= 1 << 31;
            }
            else
            {
                accumulator |= -1 << consumedBitWidth;
            }
        }

        return accumulator;
    }

    public LogicLong ReadLogicLong()
    {
        return new LogicLong(highInt32: ReadInt32(), lowInt32: ReadInt32());
    }

    public async ValueTask<LogicLong> ReadLogicLongAsync(CancellationToken cancellationToken = default)
    {
        return new LogicLong(highInt32: await ReadInt32Async(cancellationToken), lowInt32: await ReadInt32Async(cancellationToken));
    }

    private void ResetReadBoolean()
    {
        if (_booleanReadOffset <= 0)
            return;

        _booleanReadOffset = 0;
        _booleanReadAdditionalValue = 0;
    }
}
