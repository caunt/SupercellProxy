using System.Buffers.Binary;
using System.Text;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Network.Transport;

internal sealed partial class MessageStream
{
    /// <summary>
    /// Gets the <c language="csharp">CanRead</c> value.
    /// </summary>
    public bool CanRead => _stream.CanRead;

    private int _booleanReadOffset;
    private byte _booleanReadAdditionalValue;

    /// <summary>
    /// Reads <c language="csharp">Byte</c> from the stream.
    /// </summary>
    public byte ReadByte()
    {
        ResetReadBoolean();

        var value = _stream.ReadByte();

        if (value < 0)
            throw new EndOfStreamException();

        return byte.CreateTruncating(value);
    }

    /// <summary>
    /// Reads <c language="csharp">Exactly</c> from the stream.
    /// </summary>
    public Span<byte> ReadExactly(Span<byte> buffer)
    {
        ResetReadBoolean();
        _stream.ReadExactly(buffer);
        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">ExactlyAsync</c> from the stream.
    /// </summary>
    public async ValueTask<Memory<byte>> ReadExactlyAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default
    )
    {
        await _stream
            .ReadExactlyAsync(buffer, cancellationToken)
            .AsTask()
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">ToEnd</c> from the stream.
    /// </summary>
    public Memory<byte> ReadToEnd()
    {
        var buffer = new byte[Length - Position];
        ReadExactly(buffer);
        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">ByteArray</c> from the stream.
    /// </summary>
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

    /// <summary>
    /// Reads <c language="csharp">OptionalByteArray</c> from the stream.
    /// </summary>
    public Memory<byte>? ReadOptionalByteArray()
    {
        var length = ReadInt32();

        if (length < 0)
            return null;

        if (length is 0)
            return Memory<byte>.Empty;

        var buffer = new byte[length];
        ReadExactly(buffer);
        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">VarIntByteArray</c> from the stream.
    /// </summary>
    public Memory<byte> ReadVarIntByteArray()
    {
        var length = ReadVarInt();

        if (length is 0)
            return Memory<byte>.Empty;

        if (length < 0 || length > MaxPayloadLength)
            throw new InvalidDataException("Invalid variable-length byte array length.");

        var buffer = new byte[length];
        ReadExactly(buffer);

        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">Boolean</c> from the stream.
    /// </summary>
    public bool ReadBoolean()
    {
        if (_booleanReadOffset is 0)
            _booleanReadAdditionalValue = ReadByte();

        var value = ((_booleanReadAdditionalValue >> _booleanReadOffset) & 1) is not 0;
        _booleanReadOffset = (_booleanReadOffset + 1) & 7;

        return value;
    }

    /// <summary>
    /// Reads <c language="csharp">UInt16</c> from the stream.
    /// </summary>
    public ushort ReadUInt16()
    {
        return BinaryPrimitives.ReadUInt16BigEndian(ReadExactly(stackalloc byte[sizeof(ushort)]));
    }

    /// <summary>
    /// Reads <c language="csharp">Int32</c> from the stream.
    /// </summary>
    public int ReadInt32()
    {
        return BinaryPrimitives.ReadInt32BigEndian(ReadExactly(stackalloc byte[sizeof(int)]));
    }

    /// <summary>
    /// Reads <c language="csharp">UInt32</c> from the stream.
    /// </summary>
    public uint ReadUInt32()
    {
        return BinaryPrimitives.ReadUInt32BigEndian(ReadExactly(stackalloc byte[sizeof(uint)]));
    }

    /// <summary>
    /// Reads <c language="csharp">Int64</c> from the stream.
    /// </summary>
    public long ReadInt64()
    {
        return BinaryPrimitives.ReadInt64BigEndian(ReadExactly(stackalloc byte[sizeof(long)]));
    }

    /// <summary>
    /// Reads <c language="csharp">UInt64</c> from the stream.
    /// </summary>
    public ulong ReadUInt64()
    {
        return BinaryPrimitives.ReadUInt64BigEndian(ReadExactly(stackalloc byte[sizeof(long)]));
    }

    /// <summary>
    /// Reads <c language="csharp">OptionalString</c> from the stream.
    /// </summary>
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

    /// <summary>
    /// Reads <c language="csharp">String</c> from the stream.
    /// </summary>
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

    /// <summary>
    /// Reads <c language="csharp">VarInt</c> from the stream.
    /// </summary>
    public int ReadVarInt()
    {
        var firstByte = ReadByte();
        var isNegative = (firstByte & 0x40) is not 0;
        var accumulator = firstByte & 0x3F;
        var consumedBitWidth = 6;

        var currentByte = firstByte;
        while ((currentByte & 0x80) is not 0 && consumedBitWidth < 35)
        {
            currentByte = ReadByte();
            accumulator |= (currentByte & 0x7F) << consumedBitWidth;
            consumedBitWidth += 7;
        }

        if (isNegative)
        {
            if (consumedBitWidth is 34)
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

    /// <summary>
    /// Reads <c language="csharp">VarLong</c> from the stream.
    /// </summary>
    public long ReadVarLong()
    {
        var firstByte = ReadByte();
        var isNegative = (firstByte & 0x40) is not 0;
        ulong accumulator = uint.CreateTruncating((firstByte & 0x3F));
        var consumedBitWidth = 6;

        var currentByte = firstByte;
        while ((currentByte & 0x80) is not 0 && consumedBitWidth < 64)
        {
            currentByte = ReadByte();
            var availableBitWidth = Math.Min(7, 64 - consumedBitWidth);
            var valueMask = (1 << availableBitWidth) - 1;
            accumulator |= ulong.CreateTruncating((currentByte & valueMask)) << consumedBitWidth;
            consumedBitWidth += availableBitWidth;
        }

        if ((currentByte & 0x80) is not 0)
            throw new InvalidDataException("Variable-length long is too long.");

        if (isNegative && consumedBitWidth < 64)
            accumulator |= ulong.MaxValue << consumedBitWidth;

        return unchecked(long.CreateTruncating(accumulator));
    }

    /// <summary>
    /// Reads <c language="csharp">LongId</c> from the stream.
    /// </summary>
    public LongId ReadLongId()
    {
        return new LongId(highInt32: ReadInt32(), lowInt32: ReadInt32());
    }

    /// <summary>
    /// Reads <c language="csharp">OptionalLongId</c> from the stream.
    /// </summary>
    public LongId? ReadOptionalLongId()
    {
        return ReadBoolean() ? ReadLongId() : null;
    }

    /// <summary>
    /// Reads <c language="csharp">Array</c> from the stream.
    /// </summary>
    public T[] ReadArray<T>(Func<MessageStream, T> decode)
    {
        const int maxCollectionCount = 0x10000;
        var count = ReadVarInt();

        if (count < 0 || count > maxCollectionCount)
            throw new InvalidDataException("Invalid collection count.");

        var values = new T[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = decode(this);

        return values;
    }

    /// <summary>
    /// Reads <c language="csharp">Values</c> from the stream.
    /// </summary>
    public int[] ReadVarIntArray(int count)
    {
        var values = new int[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = ReadVarInt();

        return values;
    }

    private void ResetReadBoolean()
    {
        if (_booleanReadOffset <= 0)
            return;

        _booleanReadOffset = 0;
        _booleanReadAdditionalValue = 0;
    }
}
