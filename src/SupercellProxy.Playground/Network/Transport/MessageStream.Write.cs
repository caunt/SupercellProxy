using System.Buffers.Binary;
using System.Text;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Network.Transport;

public partial class MessageStream
{
    /// <summary>
    /// Gets the <c>CanWrite</c> value.
    /// </summary>
    public bool CanWrite => stream.CanWrite;

    private int _booleanWriteOffset;
    private byte _booleanWriteAccumulator;

    /// <summary>
    /// Writes <c>Byte</c> to the stream.
    /// </summary>
    public void WriteByte(byte value)
    {
        FlushWriteBoolean();
        stream.WriteByte(value);
    }

    /// <summary>
    /// Writes <c></c> to the stream.
    /// </summary>
    public void Write(ReadOnlySpan<byte> source)
    {
        FlushWriteBoolean();
        stream.Write(source);
    }

    /// <summary>
    /// Writes <c>Async</c> to the stream.
    /// </summary>
    public async ValueTask WriteAsync(
        ReadOnlyMemory<byte> source,
        CancellationToken cancellationToken = default
    )
    {
        FlushWriteBoolean();
        await stream
            .WriteAsync(source, cancellationToken)
            .AsTask()
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Writes <c>ByteArray</c> to the stream.
    /// </summary>
    public void WriteByteArray(ReadOnlySpan<byte> source)
    {
        WriteInt32(source.Length);
        Write(source);
    }

    /// <summary>
    /// Writes <c>OptionalByteArray</c> to the stream.
    /// </summary>
    public void WriteOptionalByteArray(ReadOnlyMemory<byte>? source = null)
    {
        if (source is null)
        {
            WriteInt32(-1);
            return;
        }

        WriteByteArray(source.Value.Span);
    }

    /// <summary>
    /// Writes <c>VarIntByteArray</c> to the stream.
    /// </summary>
    public void WriteVarIntByteArray(ReadOnlySpan<byte> source)
    {
        if (source.Length > MaxPayloadLength)
            throw new InvalidDataException("Variable-length byte array is too large.");

        WriteVarInt(source.Length);
        Write(source);
    }

    /// <summary>
    /// Writes <c>Boolean</c> to the stream.
    /// </summary>
    public void WriteBoolean(bool value)
    {
        if (_booleanWriteOffset is 0)
            _booleanWriteAccumulator = 0;

        if (value)
            _booleanWriteAccumulator |= byte.CreateTruncating((1 << _booleanWriteOffset));

        _booleanWriteOffset = (_booleanWriteOffset + 1) & 7;

        if (_booleanWriteOffset is 0)
            stream.WriteByte(_booleanWriteAccumulator);
    }

    /// <summary>
    /// Writes <c>UInt16</c> to the stream.
    /// </summary>
    public void WriteUInt16(ushort value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(ushort)]);
        BinaryPrimitives.WriteUInt16BigEndian(span, value);
        stream.Write(span);
    }

    /// <summary>
    /// Writes <c>Int32</c> to the stream.
    /// </summary>
    public void WriteInt32(int value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(int)]);
        BinaryPrimitives.WriteInt32BigEndian(span, value);
        stream.Write(span);
    }

    /// <summary>
    /// Writes <c>UInt32</c> to the stream.
    /// </summary>
    public void WriteUInt32(uint value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(uint)]);
        BinaryPrimitives.WriteUInt32BigEndian(span, value);
        stream.Write(span);
    }

    /// <summary>
    /// Writes <c>Int64</c> to the stream.
    /// </summary>
    public void WriteInt64(long value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(long)]);
        BinaryPrimitives.WriteInt64BigEndian(span, value);
        stream.Write(span);
    }

    /// <summary>
    /// Writes <c>UInt64</c> to the stream.
    /// </summary>
    public void WriteUInt64(ulong value)
    {
        FlushWriteBoolean();

        var span = (stackalloc byte[sizeof(ulong)]);
        BinaryPrimitives.WriteUInt64BigEndian(span, value);
        stream.Write(span);
    }

    /// <summary>
    /// Writes <c>OptionalString</c> to the stream.
    /// </summary>
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

        Span<byte> span = length <= 1024 ? stackalloc byte[length] : new byte[length];
        Encoding.UTF8.GetBytes(value, span);
        stream.Write(span);
    }

    /// <summary>
    /// Writes <c>String</c> to the stream.
    /// </summary>
    public void WriteString(string value)
    {
        var length = Encoding.UTF8.GetByteCount(value);
        WriteInt32(length);

        if (length is 0)
            return;

        Span<byte> span = length <= 1024 ? stackalloc byte[length] : new byte[length];
        Encoding.UTF8.GetBytes(value, span);
        stream.Write(span);
    }

    /// <summary>
    /// Writes <c>VarInt</c> to the stream.
    /// </summary>
    public void WriteVarInt(int valueToWrite)
    {
        FlushWriteBoolean();

        var temporarySignByte = (valueToWrite >> 25) & 0x40;
        long boundariesTracker =
            valueToWrite < 0 ? -long.CreateTruncating(valueToWrite) : valueToWrite;

        temporarySignByte |= valueToWrite & 0x3F;
        valueToWrite >>= 6;

        var byteBuffer = (stackalloc byte[5]);
        var currentIndex = 0;

        if ((boundariesTracker >>= 6) == 0)
        {
            byteBuffer[currentIndex++] = byte.CreateTruncating(temporarySignByte);
            stream.Write(byteBuffer[..currentIndex]);
            return;
        }

        byteBuffer[currentIndex++] = byte.CreateTruncating((temporarySignByte | 0x80));

        do
        {
            var dataByte = byte.CreateTruncating((valueToWrite & 0x7F));
            valueToWrite >>= 7;

            if ((boundariesTracker >>= 7) != 0)
                dataByte |= 0x80;

            byteBuffer[currentIndex++] = dataByte;
        } while (boundariesTracker != 0);

        if (currentIndex is 5)
            byteBuffer[4] &= 0x0F;

        stream.Write(byteBuffer[..currentIndex]);
    }

    /// <summary>
    /// Writes <c>VarLong</c> to the stream.
    /// </summary>
    public void WriteVarLong(long valueToWrite)
    {
        FlushWriteBoolean();

        var temporarySignByte = int.CreateTruncating((valueToWrite >> 57)) & 0x40;
        var boundariesTracker =
            valueToWrite < 0
                ? unchecked(ulong.CreateTruncating((-(valueToWrite + 1)))) + 1
                : ulong.CreateTruncating(valueToWrite);

        temporarySignByte |= int.CreateTruncating(valueToWrite) & 0x3F;
        valueToWrite >>= 6;

        Span<byte> byteBuffer = stackalloc byte[10];
        var currentIndex = 0;

        if ((boundariesTracker >>= 6) == 0)
        {
            byteBuffer[currentIndex++] = byte.CreateTruncating(temporarySignByte);
            stream.Write(byteBuffer[..currentIndex]);
            return;
        }

        byteBuffer[currentIndex++] = byte.CreateTruncating((temporarySignByte | 0x80));

        do
        {
            var dataByte = byte.CreateTruncating((valueToWrite & 0x7F));
            valueToWrite >>= 7;

            if ((boundariesTracker >>= 7) != 0)
                dataByte |= 0x80;

            byteBuffer[currentIndex++] = dataByte;
        } while (boundariesTracker != 0);

        if (currentIndex == byteBuffer.Length)
            byteBuffer[^1] &= 0x03;

        stream.Write(byteBuffer[..currentIndex]);
    }

    /// <summary>
    /// Writes <c>LongId</c> to the stream.
    /// </summary>
    public void WriteLongId(LongId logicLong)
    {
        WriteInt32(logicLong.HighInt32);
        WriteInt32(logicLong.LowInt32);
    }

    /// <summary>
    /// Writes <c>OptionalLongId</c> to the stream.
    /// </summary>
    public void WriteOptionalLongId(LongId? value)
    {
        WriteBoolean(value is not null);

        if (value is not null)
            WriteLongId(value.Value);
    }

    /// <summary>
    /// Writes <c>Array</c> to the stream.
    /// </summary>
    public void WriteArray<T>(ReadOnlySpan<T> values, Action<MessageStream, T> encode)
    {
        WriteVarInt(values.Length);

        foreach (var value in values)
            encode(this, value);
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
