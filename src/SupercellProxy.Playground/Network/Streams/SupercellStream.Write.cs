using SupercellProxy.Playground.Supercell;
using System.Buffers.Binary;
using System.Text;

namespace SupercellProxy.Playground.Network.Streams;

public partial class SupercellStream
{
    public bool CanWrite => stream.CanWrite;

    private int _booleanWriteOffset;
    private byte _booleanWriteAccumulator;

    public void WriteByte(byte value)
    {
        FlushWriteBoolean();
        stream.WriteByte(value);
    }

    public void Write(ReadOnlySpan<byte> source)
    {
        FlushWriteBoolean();
        stream.Write(source);
    }

    public async ValueTask WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
    {
        FlushWriteBoolean();
        await stream.WriteAsync(source, cancellationToken).AsTask().WaitAsync(cancellationToken);
    }

    public void WriteByteArray(ReadOnlySpan<byte> source)
    {
        WriteInt32(source.Length);
        Write(source);
    }

    public async ValueTask WriteByteArrayAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
    {
        await WriteInt32Async(source.Length, cancellationToken);
        await WriteAsync(source, cancellationToken);
    }

    public void WriteBoolean(bool value)
    {
        if (_booleanWriteOffset == 0)
            _booleanWriteAccumulator = 0;

        if (value)
            _booleanWriteAccumulator |= (byte)(1 << _booleanWriteOffset);

        _booleanWriteOffset = (_booleanWriteOffset + 1) & 7;

        if (_booleanWriteOffset == 0)
            stream.WriteByte(_booleanWriteAccumulator);
    }

    public void WriteUInt16(ushort value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(ushort)]);
        BinaryPrimitives.WriteUInt16BigEndian(span, value);
        stream.Write(span);
    }

    public async ValueTask WriteUInt16Async(ushort value, CancellationToken cancellationToken = default)
    {
        FlushWriteBoolean();

        var memory = RentExactly(sizeof(ushort));
        BinaryPrimitives.WriteUInt16BigEndian(memory.Span, value);
        await stream.WriteAsync(memory, cancellationToken);
    }

    public void WriteInt32(int value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(int)]);
        BinaryPrimitives.WriteInt32BigEndian(span, value);
        stream.Write(span);
    }

    public async ValueTask WriteInt32Async(int value, CancellationToken cancellationToken = default)
    {
        FlushWriteBoolean();

        var memory = RentExactly(sizeof(int));
        BinaryPrimitives.WriteInt32BigEndian(memory.Span, value);
        await stream.WriteAsync(memory, cancellationToken);
    }

    public void WriteUInt32(uint value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(uint)]);
        BinaryPrimitives.WriteUInt32BigEndian(span, value);
        stream.Write(span);
    }

    public async ValueTask WriteUInt32Async(uint value, CancellationToken cancellationToken = default)
    {
        FlushWriteBoolean();

        var memory = RentExactly(sizeof(uint));
        BinaryPrimitives.WriteUInt32BigEndian(memory.Span, value);
        await stream.WriteAsync(memory, cancellationToken);
    }

    public void WriteInt64(long value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(long)]);
        BinaryPrimitives.WriteInt64BigEndian(span, value);
        stream.Write(span);
    }

    public async ValueTask WriteUInt64Async(ulong value, CancellationToken cancellationToken = default)
    {
        FlushWriteBoolean();

        var memory = RentExactly(sizeof(ulong));
        BinaryPrimitives.WriteUInt64BigEndian(memory.Span, value);
        await stream.WriteAsync(memory, cancellationToken);
    }

    public void WriteUInt64(ulong value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(ulong)]);
        BinaryPrimitives.WriteUInt64BigEndian(span, value);
        stream.Write(span);
    }

    public async ValueTask WriteInt64Async(long value, CancellationToken cancellationToken = default)
    {
        FlushWriteBoolean();

        var memory = RentExactly(sizeof(long));
        BinaryPrimitives.WriteInt64BigEndian(memory.Span, value);
        await stream.WriteAsync(memory, cancellationToken);
    }

    public void WriteOptionalString(string? value = null)
    {
        if (value is null)
        {
            WriteInt32(-1);
            return;
        }

        var length = Encoding.UTF8.GetByteCount(value);
        WriteInt32(length);

        if (length is 0)
            return;

        var span = (stackalloc byte[length]);
        Encoding.UTF8.GetBytes(value, span);
        stream.Write(span);
    }

    public void WriteString(string value)
    {
        var length = Encoding.UTF8.GetByteCount(value);
        WriteInt32(length);

        if (length is 0)
            return;

        var span = (stackalloc byte[length]);
        Encoding.UTF8.GetBytes(value, span);
        stream.Write(span);
    }

    public async ValueTask WriteOptionalStringAsync(string? value = null, CancellationToken cancellationToken = default)
    {
        if (value is null)
        {
            await WriteInt32Async(-1, cancellationToken);
            return;
        }

        var length = Encoding.UTF8.GetByteCount(value);
        await WriteInt32Async(length, cancellationToken);

        if (length is 0)
            return;

        var memory = RentExactly(length);
        Encoding.UTF8.GetBytes(value, memory.Span);
        await stream.WriteAsync(memory, cancellationToken);
    }

    public async ValueTask WriteStringAsync(string value, CancellationToken cancellationToken = default)
    {
        var length = Encoding.UTF8.GetByteCount(value);
        await WriteInt32Async(length, cancellationToken);

        if (length is 0)
            return;

        var memory = RentExactly(length);
        Encoding.UTF8.GetBytes(value, memory.Span);
        await stream.WriteAsync(memory, cancellationToken);
    }

    public void WriteVarInt(int valueToWrite)
    {
        FlushWriteBoolean();

        var temporarySignByte = (valueToWrite >> 25) & 0x40;
        long boundariesTracker = valueToWrite < 0 ? -(long)valueToWrite : valueToWrite;

        temporarySignByte |= valueToWrite & 0x3F;
        valueToWrite >>= 6;

        var byteBuffer = (stackalloc byte[5]);
        var currentIndex = 0;

        if ((boundariesTracker >>= 6) == 0)
        {
            byteBuffer[currentIndex++] = (byte)temporarySignByte;
            stream.Write(byteBuffer[..currentIndex]);
            return;
        }

        byteBuffer[currentIndex++] = (byte)(temporarySignByte | 0x80);

        do
        {
            var dataByte = (byte)(valueToWrite & 0x7F);
            valueToWrite >>= 7;

            if ((boundariesTracker >>= 7) != 0)
                dataByte |= 0x80;

            byteBuffer[currentIndex++] = dataByte;
        } while (boundariesTracker != 0);

        if (currentIndex == 5)
            byteBuffer[4] &= 0x0F;

        stream.Write(byteBuffer[..currentIndex]);
    }

    public void WriteAccountId(AccountId accountId)
    {
        WriteInt32(accountId.HighInt32);
        WriteInt32(accountId.LowInt32);
    }

    public async ValueTask WriteAccountIdAsync(AccountId accountId, CancellationToken cancellationToken = default)
    {
        await WriteInt32Async(accountId.HighInt32, cancellationToken);
        await WriteInt32Async(accountId.LowInt32, cancellationToken);
    }

    private void FlushWriteBoolean()
    {
        if (_booleanWriteOffset <= 0)
            return;

        stream.WriteByte(_booleanWriteAccumulator);

        _booleanWriteOffset = 0;
        _booleanWriteAccumulator = 0;
    }
}
