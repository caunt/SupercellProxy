using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Transport;

/// <summary>Reads and writes protocol values and delegates framed message exchange to its connection transport.</summary>
public sealed class MessageStream : IDisposable
{

    /// <summary>
    /// Defines the <c language="csharp">MaxPayloadLength</c> value.
    /// </summary>
    public const int MaximumPayloadLength = 0x1000000;
    private readonly byte[] _buffer = new byte[65536];
    private readonly bool _leaveOpen;
    private readonly MessageTransport _transport;
    private byte _booleanReadAdditionalValue;

    private int _booleanReadOffset;
    private byte _booleanWriteAccumulator;

    private int _booleanWriteOffset;

    /// <summary>Wraps a binary protocol stream, optionally retaining ownership of the underlying stream.</summary>
    public MessageStream(Stream stream, bool leaveOpen = true)
    {
        BaseStream = stream;
        _leaveOpen = leaveOpen;
        _transport = new MessageTransport(this);
    }
    /// <summary>
    /// Gets the <c language="csharp">CanRead</c> value.
    /// </summary>
    public bool CanRead => BaseStream.CanRead;
    /// <summary>
    /// Gets the <c language="csharp">CanWrite</c> value.
    /// </summary>
    public bool CanWrite => BaseStream.CanWrite;

    /// <summary>
    /// Gets or sets the <c language="csharp">CommandDataResolver</c> value.
    /// </summary>
    public ICommandDataResolver? CommandDataResolver { get; set; }

    /// <summary>
    /// Gets the <c language="csharp">Length</c> value.
    /// </summary>
    public long Length => GetMemoryStream().Length;

    /// <summary>
    /// Gets or sets the <c language="csharp">Position</c> value.
    /// </summary>
    public long Position
    {
        get => GetMemoryStream().Position;
        set
        {
            FlushWriteBoolean();
            ResetReadBoolean();
            GetMemoryStream().Position = value;
        }
    }

    /// <summary>Gets or sets the source of upstream keys. Offline codecs do not require one.</summary>
    public IServerPublicKeySource? ServerKeySource { get; set; }

    internal Stream BaseStream { get; }

    /// <summary>
    /// Creates a <c language="csharp">MessageStream</c> from the supplied data.
    /// </summary>
    public static MessageStream Create()
    {
        return new MessageStream(new MemoryStream());
    }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MessageStream Create(ReadOnlyMemory<byte> memory)
    {
        return MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment) && segment.Array is not null
            ? new MessageStream(new MemoryStream(segment.Array, segment.Offset, segment.Count))
            : new MessageStream(new MemoryStream(memory.ToArray()));
    }

    /// <summary>
    /// Executes the <c language="csharp">Dispose</c> operation.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        FlushWriteBoolean();

        if (_leaveOpen)
            return;

        BaseStream.Dispose();
    }

    /// <summary>
    /// Reads <c language="csharp">Array</c> from the stream.
    /// </summary>
    public TValue[] ReadArray<TValue>(Func<MessageStream, TValue> decode)
    {
        ArgumentNullException.ThrowIfNull(decode);
        const int maximumCollectionCount = 0x10000;
        int count = ReadVariableInt();

        if (count is < 0 or > maximumCollectionCount)
            throw new InvalidDataException(message: "Invalid collection count.");

        TValue[] values = new TValue[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = decode(this);

        return values;
    }

    /// <summary>
    /// Reads <c language="csharp">Boolean</c> from the stream.
    /// </summary>
    public bool ReadBoolean()
    {
        if (_booleanReadOffset is 0)
            _booleanReadAdditionalValue = ReadByte();

        bool value = ((_booleanReadAdditionalValue >> _booleanReadOffset) & 1) is not 0;
        _booleanReadOffset = (_booleanReadOffset + 1) & 7;

        return value;
    }

    /// <summary>
    /// Reads <c language="csharp">Byte</c> from the stream.
    /// </summary>
    public byte ReadByte()
    {
        ResetReadBoolean();

        int value = BaseStream.ReadByte();

        return value < 0 ? throw new EndOfStreamException() : byte.CreateTruncating(value);
    }

    /// <summary>
    /// Reads <c language="csharp">ByteArray</c> from the stream.
    /// </summary>
    public Memory<byte> ReadByteArray()
    {
        int length = ReadInt32();

        return length is 0
            ? Memory<byte>.Empty
            : length < 0 ? throw new InvalidDataException(message: "Negative length for byte array.") : (Memory<byte>)ReadBytes(length);
    }

    /// <summary>Reads an exact number of bytes without consuming a length prefix.</summary>
    public byte[] ReadBytes(int length)
    {
        byte[] buffer = new byte[length];
        ResetReadBoolean();
        BaseStream.ReadExactly(buffer);

        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">ContainerAsync</c> from the stream.
    /// </summary>
    public ValueTask<MessageContainer> ReadContainerAsync(CancellationToken cancellationToken = default)
    {
        return _transport.ReadContainerAsync(cancellationToken);
    }

    /// <summary>
    /// Reads <c language="csharp">Exactly</c> from the stream.
    /// </summary>
    public Span<byte> ReadExactly(Span<byte> buffer)
    {
        ResetReadBoolean();
        BaseStream.ReadExactly(buffer);

        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">ExactlyAsync</c> from the stream.
    /// </summary>
    public async ValueTask<Memory<byte>> ReadExactlyAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        await BaseStream
            .ReadExactlyAsync(buffer, cancellationToken)
            .AsTask()
            .WaitAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">Int32</c> from the stream.
    /// </summary>
    public int ReadInt32()
    {
        return BinaryPrimitives.ReadInt32BigEndian(ReadExactly(stackalloc byte[sizeof(int)]));
    }

    /// <summary>
    /// Reads <c language="csharp">Int64</c> from the stream.
    /// </summary>
    public long ReadInt64()
    {
        return BinaryPrimitives.ReadInt64BigEndian(ReadExactly(stackalloc byte[sizeof(long)]));
    }

    /// <summary>
    /// Reads <c language="csharp">LongId</c> from the stream.
    /// </summary>
    public LongIdentifier ReadLongIdentifier()
    {
        return new LongIdentifier(highInt32: ReadInt32(), lowInt32: ReadInt32());
    }

    /// <summary>
    /// Reads <c language="csharp">MessageAsync</c> from the stream.
    /// </summary>
    public Task<TValue> ReadMessageAsync<TValue>(CancellationToken cancellationToken = default)
        where TValue : IMessage
    {
        return _transport.ReadMessageAsync<TValue>(cancellationToken);
    }

    /// <summary>
    /// Reads <c language="csharp">MessageAsync</c> from the stream.
    /// </summary>
    public Task<IMessage> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        return _transport.ReadMessageAsync(cancellationToken);
    }

    /// <summary>
    /// Reads <c language="csharp">OptionalByteArray</c> from the stream.
    /// </summary>
    public Memory<byte>? ReadOptionalByteArray()
    {
        int length = ReadInt32();

        return length < 0 ? null : length is 0 ? Memory<byte>.Empty : (Memory<byte>?)ReadBytes(length);
    }

    /// <summary>
    /// Reads <c language="csharp">OptionalLongId</c> from the stream.
    /// </summary>
    public LongIdentifier? ReadOptionalLongIdentifier()
    {
        return ReadBoolean() ? ReadLongIdentifier() : null;
    }

    /// <summary>
    /// Reads <c language="csharp">OptionalString</c> from the stream.
    /// </summary>
    public string? ReadOptionalString()
    {
        int length = ReadInt32();

        if (length < 0)
            return null;

        if (length is 0)
            return string.Empty;

        Span<byte> buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];

        return Encoding.UTF8.GetString(ReadExactly(buffer));
    }

    /// <summary>
    /// Reads <c language="csharp">String</c> from the stream.
    /// </summary>
    public string ReadString()
    {
        int length = ReadInt32();

        if (length < 0)
            throw new InvalidDataException(message: "Negative length for string array.");

        if (length is 0)
            return string.Empty;

        Span<byte> buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];

        return Encoding.UTF8.GetString(ReadExactly(buffer));
    }

    /// <summary>
    /// Reads <c language="csharp">ToEnd</c> from the stream.
    /// </summary>
    public Memory<byte> ReadToEnd()
    {
        return ReadBytes(checked((int)(Length - Position)));
    }

    /// <summary>
    /// Reads <c language="csharp">UInt16</c> from the stream.
    /// </summary>
    public ushort ReadUInt16()
    {
        return BinaryPrimitives.ReadUInt16BigEndian(ReadExactly(stackalloc byte[sizeof(ushort)]));
    }

    /// <summary>
    /// Reads <c language="csharp">UInt32</c> from the stream.
    /// </summary>
    public uint ReadUInt32()
    {
        return BinaryPrimitives.ReadUInt32BigEndian(ReadExactly(stackalloc byte[sizeof(uint)]));
    }

    /// <summary>
    /// Reads <c language="csharp">UInt64</c> from the stream.
    /// </summary>
    public ulong ReadUInt64()
    {
        return BinaryPrimitives.ReadUInt64BigEndian(ReadExactly(stackalloc byte[sizeof(long)]));
    }

    /// <summary>
    /// Reads <c language="csharp">UntilMessageAsync</c> from the stream.
    /// </summary>
    public Task<TValue> ReadUntilMessageAsync<TValue>(CancellationToken cancellationToken = default)
        where TValue : IMessage
    {
        return _transport.ReadUntilMessageAsync<TValue>(cancellationToken);
    }

    /// <summary>
    /// Reads <c language="csharp">VarInt</c> from the stream.
    /// </summary>
    public int ReadVariableInt()
    {
        byte firstByte = ReadByte();
        bool isNegative = (firstByte & 0x40) is not 0;
        int accumulator = firstByte & 0x3F;
        int consumedBitWidth = 6;

        byte currentByte = firstByte;

        while ((currentByte & 0x80) is not 0 && consumedBitWidth < 35)
        {
            currentByte = ReadByte();
            accumulator |= (currentByte & 0x7F) << consumedBitWidth;
            consumedBitWidth += 7;
        }

        if (isNegative)
        {
            if (consumedBitWidth is 34)
                accumulator |= 1 << 31;
            else
                accumulator |= -1 << consumedBitWidth;
        }

        return accumulator;
    }

    /// <summary>
    /// Reads <c language="csharp">Values</c> from the stream.
    /// </summary>
    public int[] ReadVariableIntArray(int count)
    {
        int[] values = new int[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = ReadVariableInt();

        return values;
    }

    /// <summary>
    /// Reads <c language="csharp">VarIntByteArray</c> from the stream.
    /// </summary>
    public Memory<byte> ReadVariableIntByteArray()
    {
        int length = ReadVariableInt();

        return length is 0
            ? Memory<byte>.Empty
            : length is < 0 or > MaximumPayloadLength
            ? throw new InvalidDataException(message: "Invalid variable-length byte array length.")
            : (Memory<byte>)ReadBytes(length);
    }

    /// <summary>
    /// Reads <c language="csharp">VarLong</c> from the stream.
    /// </summary>
    public long ReadVariableLong()
    {
        byte firstByte = ReadByte();
        bool isNegative = (firstByte & 0x40) is not 0;
        ulong accumulator = uint.CreateTruncating(firstByte & 0x3F);
        int consumedBitWidth = 6;

        byte currentByte = firstByte;

        while ((currentByte & 0x80) is not 0 && consumedBitWidth < 64)
        {
            currentByte = ReadByte();
            int availableBitWidth = Math.Min(val1: 7, 64 - consumedBitWidth);
            int valueMask = (1 << availableBitWidth) - 1;
            accumulator |= ulong.CreateTruncating(currentByte & valueMask) << consumedBitWidth;
            consumedBitWidth += availableBitWidth;
        }

        if ((currentByte & 0x80) is not 0)
            throw new InvalidDataException(message: "Variable-length long is too long.");

        if (isNegative && consumedBitWidth < 64)
            accumulator |= ulong.MaxValue << consumedBitWidth;

        return unchecked(long.CreateTruncating(accumulator));
    }

    /// <summary>
    /// Executes the <c language="csharp">RentExactly</c> operation.
    /// </summary>
    public Memory<byte> RentExactly(int length)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, _buffer.Length);

        return _buffer.AsMemory(start: 0, length);
    }

    /// <summary>
    /// Provides the Resolve Message value or operation.
    /// </summary>
    public IMessage ResolveMessage(MessageContainer container)
    {
        return _transport.ResolveMessage(container);
    }

    /// <summary>
    /// <para>Initializes encryption for the current connection using the specified session key and connection side.</para>
    /// </summary>
    /// <param name="with">Specifies the connection side, indicating whether encryption is being set up for the server or the client.</param>
    /// <param name="sessionKey">The session key to verify during the handshake phase.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation of setting up encryption.</returns>
    /// <exception cref="InvalidOperationException">Thrown if encryption has already been set up for this connection.</exception>
    public ValueTask SetupEncryptionAsync(RemotePeerRole with, Memory<byte> sessionKey, CancellationToken cancellationToken = default)
    {
        return _transport.SetupEncryptionAsync(with, sessionKey, cancellationToken);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToArray</c> operation.
    /// </summary>
    public byte[] ToArray()
    {
        FlushWriteBoolean();

        return GetMemoryStream().ToArray();
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string? ToString()
    {
        if (!TryGetMemoryStream(out MemoryStream? memoryStream))
            return base.ToString();

        StringBuilder builder = new();
        string hex = Convert.ToHexString(memoryStream.ToArray());

        if (hex.Length <= 128)
        {
            builder = builder.Append(hex);
        }
        else
        {
            builder = builder.Append(hex, startIndex: 0, count: 125);
            builder = builder.Append(value: "...");
        }

        return builder.ToString();
    }

    /// <summary>
    /// Writes <c language="csharp"></c> to the stream.
    /// </summary>
    public void Write(ReadOnlySpan<byte> source)
    {
        FlushWriteBoolean();
        BaseStream.Write(source);
    }

    /// <summary>
    /// Writes <c language="csharp">Array</c> to the stream.
    /// </summary>
    public void WriteArray<TValue>(ReadOnlySpan<TValue> values, Action<MessageStream, TValue> encode)
    {
        ArgumentNullException.ThrowIfNull(encode);
        WriteVariableInt(values.Length);

        foreach (TValue value in values)
            encode(this, value);
    }

    /// <summary>
    /// Writes <c language="csharp">Async</c> to the stream.
    /// </summary>
    public async ValueTask WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
    {
        FlushWriteBoolean();
        await BaseStream
            .WriteAsync(source, cancellationToken)
            .AsTask()
            .WaitAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>
    /// Writes <c language="csharp">Boolean</c> to the stream.
    /// </summary>
    public void WriteBoolean(bool value)
    {
        if (_booleanWriteOffset is 0)
            _booleanWriteAccumulator = 0;

        if (value)
            _booleanWriteAccumulator |= byte.CreateTruncating(1 << _booleanWriteOffset);

        _booleanWriteOffset = (_booleanWriteOffset + 1) & 7;

        if (_booleanWriteOffset is 0)
            BaseStream.WriteByte(_booleanWriteAccumulator);
    }

    /// <summary>
    /// Writes <c language="csharp">Byte</c> to the stream.
    /// </summary>
    public void WriteByte(byte value)
    {
        FlushWriteBoolean();
        BaseStream.WriteByte(value);
    }

    /// <summary>
    /// Writes <c language="csharp">ByteArray</c> to the stream.
    /// </summary>
    public void WriteByteArray(ReadOnlySpan<byte> source)
    {
        WriteInt32(source.Length);
        Write(source);
    }

    /// <summary>
    /// Writes <c language="csharp">ContainerAsync</c> to the stream.
    /// </summary>
    public ValueTask WriteContainerAsync(MessageContainer messageContainer, CancellationToken cancellationToken = default)
    {
        return _transport.WriteContainerAsync(messageContainer, cancellationToken);
    }

    /// <summary>
    /// Writes <c language="csharp">Int32</c> to the stream.
    /// </summary>
    public void WriteInt32(int value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">Int64</c> to the stream.
    /// </summary>
    public void WriteInt64(long value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">LongId</c> to the stream.
    /// </summary>
    public void WriteLongIdentifier(LongIdentifier logicLong)
    {
        WriteInt32(logicLong.HighInt32);
        WriteInt32(logicLong.LowInt32);
    }

    /// <summary>
    /// Writes <c language="csharp">MessageAsync</c> to the stream.
    /// </summary>
    public Task WriteMessageAsync<TValue>(TValue message, CancellationToken cancellationToken = default)
        where TValue : IMessage
    {
        return _transport.WriteMessageAsync(message, cancellationToken);
    }

    /// <summary>
    /// Writes <c language="csharp">OptionalByteArray</c> to the stream.
    /// </summary>
    public void WriteOptionalByteArray(ReadOnlyMemory<byte>? source = null)
    {
        if (source is null)
        {
            WriteInt32(value: -1);

            return;
        }

        WriteByteArray(source.Value.Span);
    }

    /// <summary>
    /// Writes <c language="csharp">OptionalLongId</c> to the stream.
    /// </summary>
    public void WriteOptionalLongIdentifier(LongIdentifier? value)
    {
        WriteBoolean(value is not null);

        if (value is not null)
            WriteLongIdentifier(value.Value);
    }

    /// <summary>
    /// Writes <c language="csharp">OptionalString</c> to the stream.
    /// </summary>
    public void WriteOptionalString(string? value = null)
    {
        if (value is null)
        {
            WriteInt32(value: -1);

            return;
        }

        int length = Encoding.UTF8.GetByteCount(value);
        WriteInt32(length);

        if (length is 0)
            return;

        Span<byte> span = length <= 1024 ? stackalloc byte[length] : new byte[length];
        int written = Encoding.UTF8.GetBytes(value, span);
        BaseStream.Write(span[..written]);
    }

    /// <summary>
    /// Writes <c language="csharp">String</c> to the stream.
    /// </summary>
    public void WriteString(string value)
    {
        int length = Encoding.UTF8.GetByteCount(value);
        WriteInt32(length);

        if (length is 0)
            return;

        Span<byte> span = length <= 1024 ? stackalloc byte[length] : new byte[length];
        int written = Encoding.UTF8.GetBytes(value, span);
        BaseStream.Write(span[..written]);
    }

    /// <summary>
    /// Writes <c language="csharp">UInt16</c> to the stream.
    /// </summary>
    public void WriteUInt16(ushort value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">UInt32</c> to the stream.
    /// </summary>
    public void WriteUInt32(uint value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">UInt64</c> to the stream.
    /// </summary>
    public void WriteUInt64(ulong value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">VarInt</c> to the stream.
    /// </summary>
    public void WriteVariableInt(int valueToWrite)
    {
        FlushWriteBoolean();

        int temporarySignByte = (valueToWrite >> 25) & 0x40;

        long boundariesTracker =
            valueToWrite < 0 ? -long.CreateTruncating(valueToWrite) : valueToWrite;

        temporarySignByte |= valueToWrite & 0x3F;
        valueToWrite >>= 6;

        Span<byte> byteBuffer = stackalloc byte[5];
        int currentIndex = 0;

        if ((boundariesTracker >>= 6) == 0)
        {
            byteBuffer[currentIndex++] = byte.CreateTruncating(temporarySignByte);
            BaseStream.Write(byteBuffer[..currentIndex]);

            return;
        }

        byteBuffer[currentIndex++] = byte.CreateTruncating(temporarySignByte | 0x80);

        do
        {
            byte dataByte = byte.CreateTruncating(valueToWrite & 0x7F);
            valueToWrite >>= 7;

            if ((boundariesTracker >>= 7) != 0)
                dataByte |= 0x80;

            byteBuffer[currentIndex++] = dataByte;
        } while (boundariesTracker != 0);

        if (currentIndex is 5)
            byteBuffer[index: 4] &= 0x0F;

        BaseStream.Write(byteBuffer[..currentIndex]);
    }

    /// <summary>
    /// Writes <c language="csharp">VarIntByteArray</c> to the stream.
    /// </summary>
    public void WriteVariableIntByteArray(ReadOnlySpan<byte> source)
    {
        if (source.Length > MaximumPayloadLength)
            throw new InvalidDataException(message: "Variable-length byte array is too large.");

        WriteVariableInt(source.Length);
        Write(source);
    }

    /// <summary>
    /// Writes <c language="csharp">VarLong</c> to the stream.
    /// </summary>
    public void WriteVariableLong(long valueToWrite)
    {
        FlushWriteBoolean();

        int temporarySignByte = int.CreateTruncating(valueToWrite >> 57) & 0x40;

        ulong boundariesTracker =
            valueToWrite < 0
                ? unchecked(ulong.CreateTruncating(-(valueToWrite + 1))) + 1
                : ulong.CreateTruncating(valueToWrite);

        temporarySignByte |= int.CreateTruncating(valueToWrite) & 0x3F;
        valueToWrite >>= 6;

        Span<byte> byteBuffer = stackalloc byte[10];
        int currentIndex = 0;

        if ((boundariesTracker >>= 6) == 0)
        {
            byteBuffer[currentIndex++] = byte.CreateTruncating(temporarySignByte);
            BaseStream.Write(byteBuffer[..currentIndex]);

            return;
        }

        byteBuffer[currentIndex++] = byte.CreateTruncating(temporarySignByte | 0x80);

        do
        {
            byte dataByte = byte.CreateTruncating(valueToWrite & 0x7F);
            valueToWrite >>= 7;

            if ((boundariesTracker >>= 7) != 0)
                dataByte |= 0x80;

            byteBuffer[currentIndex++] = dataByte;
        } while (boundariesTracker != 0);

        if (currentIndex == byteBuffer.Length)
            byteBuffer[new Index(value: 1, fromEnd: true)] &= 0x03;

        BaseStream.Write(byteBuffer[..currentIndex]);
    }

    internal void FlushWriteBoolean()
    {
        if (_booleanWriteOffset <= 0)
            return;

        BaseStream.WriteByte(_booleanWriteAccumulator);

        _booleanWriteOffset = 0;
        _booleanWriteAccumulator = 0;
    }

    internal MemoryStream GetMemoryStream()
    {
        return !TryGetMemoryStream(out MemoryStream? memoryStream)
            ? throw new NotSupportedException(message: "This is online stream.")
            : memoryStream;
    }

    private void ResetReadBoolean()
    {
        if (_booleanReadOffset <= 0)
            return;

        _booleanReadOffset = 0;
        _booleanReadAdditionalValue = 0;
    }

    private bool TryGetMemoryStream([MaybeNullWhen(false)] out MemoryStream memoryStream)
    {
        memoryStream = BaseStream as MemoryStream;

        return memoryStream is not null;
    }
}
