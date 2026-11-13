using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Transport;

/// <summary>Reads and writes protocol values and delegates framed message exchange to its connection transport.</summary>
public sealed class MessageChannel : ProtocolEncoder, IDisposable
{
    /// <summary>
    /// Defines the <c language="csharp">MaxPayloadLength</c> value.
    /// </summary>
    public const int MaximumPayloadLength = 0x1000000;

    private const int RentBufferLength = 65536;

    private readonly bool _leaveOpen;
    private readonly MessageTransport _transport;

    // Booleans are packed eight to a byte: the byte being read or filled, and the next bit within it.
    private byte _booleanReadAdditionalValue;
    private int _booleanReadOffset;
    private byte _booleanWriteAccumulator;
    private int _booleanWriteOffset;

    // A reusable buffer that payload reads borrow, allocated on first use.
    private byte[]? _buffer;

    /// <summary>Initializes a new instance of the <see cref="MessageChannel"/> class. Wraps a binary protocol stream, optionally retaining ownership of the underlying stream.</summary>
    /// <param name="stream">Stream the protocol bytes are read from and written to: a socket's network stream for a live connection, or a memory stream holding one message payload.</param>
    /// <param name="leaveOpen"><see langword="true"/> (the default) to leave <paramref name="stream"/> open when this instance is disposed; <see langword="false"/> to dispose it as well.</param>
    public MessageChannel(Stream stream, bool leaveOpen = true)
    {
        BaseStream = stream;
        _leaveOpen = leaveOpen;
        _transport = new MessageTransport(this);
    }

    /// <summary>
    /// Gets whether the wrapped stream can still be read, for a socket stream that is whether the connection is open for reading.
    /// </summary>
    /// <value><see langword="true"/> if the wrapped stream's own <see cref="Stream.CanRead"/> reports that it can be read; otherwise, <see langword="false"/>.</value>
    /// <remarks>The wrapped stream's property is read live on every call.</remarks>
    public bool CanRead => BaseStream.CanRead;

    /// <summary>
    /// Gets whether the wrapped stream accepts writes.
    /// </summary>
    /// <value><see langword="true"/> if the wrapped stream's own <see cref="Stream.CanWrite"/> reports that it accepts writes; otherwise, <see langword="false"/>.</value>
    /// <remarks>A payload stream created over read-only memory has the value <see langword="false"/>.</remarks>
    public bool CanWrite => BaseStream.CanWrite;

    /// <summary>
    /// Gets the size in bytes of an in-memory payload being decoded or built.
    /// </summary>
    /// <value>Length of the wrapped memory stream; a boolean still waiting in the packing byte is not counted until it is flushed.</value>
    /// <exception cref="NotSupportedException">The stream wraps a network connection, which has no fixed length.</exception>
    public long Length => GetMemoryStream().Length;

    /// <summary>
    /// Gets or sets the game tables used while decoding commands whose payload layout depends on the data entry they refer to; without them such
    /// commands cannot be decoded.
    /// </summary>
    /// <value>Resolver handed to the message registry for every message this stream decodes; <see langword="null"/> by default.</value>
    public ICommandDataResolver? CommandDataResolver { get; set; }

    /// <summary>
    /// Gets or sets an optional callback that observes received messages and plaintext frames right before they are written; payloads are borrowed for the callback only.
    /// </summary>
    /// <value>Called with <see cref="MessageDirection.Clientbound"/> for each decoded message read and <see cref="MessageDirection.Serverbound"/> for each frame written (with a <see langword="null"/> message for a raw container); <see langword="null"/> by default.</value>
    public Func<MessageDirection, MessageContainer, IMessage?, ValueTask>? ObserveMessage { get; set; }

    /// <summary>Gets or sets the application version optionally supplied for encrypted typed messages; explicit containers retain their version.</summary>
    /// <value>Message version written into the frame header of typed messages once encryption is set up; before that, or when <see langword="null"/>, typed messages carry version 0. The proxy copies it from the client's login frame.</value>
    public ushort? OutboundMessageVersion { get; set; }

    /// <summary>
    /// Gets or sets the read or write offset within an in-memory payload. Setting it first writes out a partly filled byte of packed booleans and
    /// forgets a partly read one, so packed booleans never straddle a jump.
    /// </summary>
    /// <value>Zero-based byte offset into the wrapped memory stream; it advances as values are read or written.</value>
    /// <exception cref="NotSupportedException">The stream wraps a network connection.</exception>
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
    /// <value>Supplies the real server's public key when encryption is set up toward a server; setting up that side without one throws <see cref="InvalidOperationException"/>. <see langword="null"/> by default.</value>
    public IServerPublicKeySource? ServerKeySource { get; set; }

    internal Stream BaseStream { get; }
    internal ConnectionTraffic? Traffic { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">MessageChannel</c> from the supplied data.
    /// </summary>
    /// <returns>A stream over a new, empty, growable memory buffer, ready for encoding a payload and reading it back with <see cref="ToArray"/>.</returns>
    public static MessageChannel Create()
    {
        return new MessageChannel(new MemoryStream());
    }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    /// <param name="memory">Payload bytes to decode; an array-backed segment is wrapped without copying, and other memory is copied first.</param>
    /// <returns>A read-only stream positioned at the start of <paramref name="memory"/>.</returns>
    public static MessageChannel Create(ReadOnlyMemory<byte> memory)
    {
        return MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment) && segment.Array is not null
            ? new MessageChannel(new MemoryStream(segment.Array, segment.Offset, segment.Count))
            : new MessageChannel(new MemoryStream(memory.ToArray()));
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
    /// Reads <c language="csharp">Boolean</c> from the stream.
    /// </summary>
    /// <returns><see langword="true"/> if the next bit of the current packing byte is set; otherwise, <see langword="false"/>.</returns>
    /// <remarks>Bits are taken lowest first, and a new packing byte is read for the first of every eight consecutive booleans.</remarks>
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
    /// <returns>The next byte, read as is; any partly read packing byte of booleans is abandoned first.</returns>
    /// <exception cref="EndOfStreamException">The stream has no more bytes.</exception>
    public byte ReadByte()
    {
        ResetReadBoolean();

        int value = BaseStream.ReadByte();

        return value < 0 ? throw new EndOfStreamException() : byte.CreateTruncating(value);
    }

    /// <summary>
    /// Reads <c language="csharp">ByteArray</c> from the stream.
    /// </summary>
    /// <returns>The bytes that follow a big-endian 32-bit length, or empty memory when the length is 0.</returns>
    /// <exception cref="InvalidDataException">The length is negative.</exception>
    public Memory<byte> ReadByteArray()
    {
        int length = ReadInt32();

        return length is 0
            ? Memory<byte>.Empty
            : length < 0 ? throw new InvalidDataException(message: "Negative length for byte array.") : (Memory<byte>)ReadBytes(length);
    }

    /// <summary>Reads an exact number of bytes without consuming a length prefix.</summary>
    /// <param name="length">Number of bytes to read; 0 returns an empty array.</param>
    /// <returns>A new array holding exactly <paramref name="length"/> bytes; the read fails with <see cref="EndOfStreamException"/> when fewer remain.</returns>
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
    /// <param name="cancellationToken">Stops waiting for the frame header or payload.</param>
    /// <returns>The next frame's message id, header version and payload, decrypted once encryption is set up; the payload is not decoded. A closed connection raises <see cref="StreamClosedException"/>.</returns>
    public ValueTask<MessageContainer> ReadContainerAsync(CancellationToken cancellationToken = default)
    {
        return _transport.ReadContainerAsync(cancellationToken);
    }

    /// <summary>
    /// Reads <c language="csharp">Exactly</c> from the stream.
    /// </summary>
    /// <param name="buffer">Span to fill completely; its length is the number of bytes read.</param>
    /// <returns>The same <paramref name="buffer"/>, now filled.</returns>
    public Span<byte> ReadExactly(Span<byte> buffer)
    {
        ResetReadBoolean();
        BaseStream.ReadExactly(buffer);

        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">ExactlyAsync</c> from the stream.
    /// </summary>
    /// <param name="buffer">Memory to fill completely; its length is the number of bytes read.</param>
    /// <param name="cancellationToken">Stops waiting even when the underlying read does not observe cancellation itself.</param>
    /// <returns>The same <paramref name="buffer"/>, now filled; the bytes are counted as received traffic for the connection.</returns>
    public async ValueTask<Memory<byte>> ReadExactlyAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        await BaseStream
            .ReadExactlyAsync(buffer, cancellationToken)
            .AsTask()
            .WaitAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        Traffic?.Receive(buffer.Length);

        return buffer;
    }

    /// <summary>
    /// Reads <c language="csharp">Int32</c> from the stream.
    /// </summary>
    /// <returns>A signed 32-bit integer decoded from the next four bytes in big-endian order.</returns>
    public int ReadInt32()
    {
        return BinaryPrimitives.ReadInt32BigEndian(ReadExactly(stackalloc byte[sizeof(int)]));
    }

    /// <summary>
    /// Reads <c language="csharp">Int64</c> from the stream.
    /// </summary>
    /// <returns>A signed 64-bit integer decoded from the next eight bytes in big-endian order.</returns>
    public long ReadInt64()
    {
        return BinaryPrimitives.ReadInt64BigEndian(ReadExactly(stackalloc byte[sizeof(long)]));
    }

    /// <summary>
    /// Reads <c language="csharp">LongId</c> from the stream.
    /// </summary>
    /// <returns>An id built from two big-endian 32-bit integers, the high word first.</returns>
    public LongId ReadLongId()
    {
        return new LongId(ReadInt32(), ReadInt32());
    }

    /// <summary>
    /// Reads the next message and requires it to be of the given type, for exchanges where the protocol allows only one reply.
    /// </summary>
    /// <typeparam name="TValue">Message type the caller expects next, such as a login reply type.</typeparam>
    /// <param name="cancellationToken">Stops waiting for the message.</param>
    /// <returns>The next message, already reported to <see cref="ObserveMessage"/>, as <typeparamref name="TValue"/>.</returns>
    /// <exception cref="InvalidOperationException">The next message is of another type; it is consumed anyway.</exception>
    /// <exception cref="StreamClosedException">The connection ended.</exception>
    public Task<TValue> ReadMessageAsync<TValue>(CancellationToken cancellationToken = default)
        where TValue : IMessage
    {
        return _transport.ReadMessageAsync<TValue>(cancellationToken);
    }

    /// <summary>
    /// Reads, decrypts and decodes the next message, whatever its type, and reports it to <see cref="ObserveMessage"/>.
    /// </summary>
    /// <param name="cancellationToken">Stops waiting for the message.</param>
    /// <returns>The typed message decoded for this stream's game version, or a <see cref="PassthroughMessage"/> carrying the raw payload when no message type owns the frame's id.</returns>
    /// <exception cref="StreamClosedException">The connection ended.</exception>
    public Task<IMessage> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        return _transport.ReadMessageAsync(cancellationToken);
    }

    /// <summary>
    /// Reads <c language="csharp">OptionalByteArray</c> from the stream.
    /// </summary>
    /// <returns>The bytes that follow a big-endian 32-bit length, empty memory for a length of 0, or <see langword="null"/> for a negative length, which marks the array as absent.</returns>
    public Memory<byte>? ReadOptionalByteArray()
    {
        int length = ReadInt32();

        return length < 0 ? null : length is 0 ? Memory<byte>.Empty : (Memory<byte>?)ReadBytes(length);
    }

    /// <summary>
    /// Reads <c language="csharp">OptionalLongId</c> from the stream.
    /// </summary>
    /// <returns>The id that follows a packed <see langword="true"/> flag, or <see langword="null"/> when the flag is <see langword="false"/> and nothing else is read.</returns>
    public LongId? ReadOptionalLongId()
    {
        return ReadBoolean() ? ReadLongId() : null;
    }

    /// <summary>
    /// Reads <c language="csharp">OptionalString</c> from the stream.
    /// </summary>
    /// <returns>Text decoded as UTF-8 from the bytes after a big-endian 32-bit length, the empty string for a length of 0, or <see langword="null"/> for a negative length.</returns>
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
    /// <returns>Text decoded as UTF-8 from the bytes after a big-endian 32-bit length, or the empty string for a length of 0.</returns>
    /// <exception cref="InvalidDataException">The length is negative, which only an optional string may use.</exception>
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
    /// <returns>A copy of every byte from <see cref="Position"/> to the end of the in-memory payload, or empty memory when nothing is left; a network stream throws <see cref="NotSupportedException"/>.</returns>
    public Memory<byte> ReadToEnd()
    {
        return ReadBytes(checked(int.CreateChecked(Length - Position)));
    }

    /// <summary>
    /// Reads <c language="csharp">UInt16</c> from the stream.
    /// </summary>
    /// <returns>An unsigned 16-bit integer decoded from the next two bytes in big-endian order.</returns>
    public ushort ReadUInt16()
    {
        return BinaryPrimitives.ReadUInt16BigEndian(ReadExactly(stackalloc byte[sizeof(ushort)]));
    }

    /// <summary>
    /// Reads <c language="csharp">UInt32</c> from the stream.
    /// </summary>
    /// <returns>An unsigned 32-bit integer decoded from the next four bytes in big-endian order.</returns>
    public uint ReadUInt32()
    {
        return BinaryPrimitives.ReadUInt32BigEndian(ReadExactly(stackalloc byte[sizeof(uint)]));
    }

    /// <summary>
    /// Reads <c language="csharp">UInt64</c> from the stream.
    /// </summary>
    /// <returns>An unsigned 64-bit integer decoded from the next eight bytes in big-endian order.</returns>
    public ulong ReadUInt64()
    {
        return BinaryPrimitives.ReadUInt64BigEndian(ReadExactly(stackalloc byte[sizeof(long)]));
    }

    /// <summary>
    /// Reads <c language="csharp">UntilMessageAsync</c> from the stream.
    /// </summary>
    /// <typeparam name="TValue">Message type to wait for, such as the home data that follows a login.</typeparam>
    /// <param name="cancellationToken">Stops waiting; messages already skipped stay consumed.</param>
    /// <returns>The first message of type <typeparamref name="TValue"/>; every message read before it is decoded, reported to <see cref="ObserveMessage"/> and discarded.</returns>
    public Task<TValue> ReadUntilMessageAsync<TValue>(CancellationToken cancellationToken = default)
        where TValue : IMessage
    {
        return _transport.ReadUntilMessageAsync<TValue>(cancellationToken);
    }

    /// <summary>
    /// Reads <c language="csharp">VarInt</c> from the stream.
    /// </summary>
    /// <returns>The 32-bit value of a one- to five-byte variable-length integer whose first byte holds a continuation bit, a sign bit and the six lowest bits, and whose later bytes add seven bits each.</returns>
    public int ReadVarInt()
    {
        // The first byte carries the sign flag and only six value bits.
        byte firstByte = ReadByte();
        bool isNegative = (firstByte & 0x40) is not 0;
        int accumulator = firstByte & 0x3F;

        // Later bytes add seven bits each while the continuation bit stays set.
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
    /// Reads <c language="csharp">VarIntByteArray</c> from the stream.
    /// </summary>
    /// <returns>The bytes that follow a variable-length count, or empty memory when the count is 0.</returns>
    /// <exception cref="InvalidDataException">The count is negative or larger than <see cref="MaximumPayloadLength"/>.</exception>
    public Memory<byte> ReadVarIntByteArray()
    {
        int length = ReadVarInt();

        return length is 0
            ? Memory<byte>.Empty
            : length is < 0 or > MaximumPayloadLength
            ? throw new InvalidDataException(message: "Invalid var-length byte array length.")
            : (Memory<byte>)ReadBytes(length);
    }

    /// <summary>
    /// Reads <c language="csharp">VarLong</c> from the stream.
    /// </summary>
    /// <returns>The 64-bit value of a variable-length integer of up to ten bytes, laid out like a 32-bit one with a sign bit in the first byte.</returns>
    /// <exception cref="InvalidDataException">The last byte the format allows still has its continuation bit set.</exception>
    public long ReadVarLong()
    {
        // As in the 32-bit form, the first byte holds the sign flag and six value bits.
        byte firstByte = ReadByte();
        bool isNegative = (firstByte & 0x40) is not 0;
        ulong accumulator = firstByte & 0x3Fu;

        // Continuation bytes then fill the remaining bits, at most seven at a time.
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
            throw new InvalidDataException(message: "Var-length long is too long.");

        if (isNegative && consumedBitWidth < 64)
            accumulator |= ulong.MaxValue << consumedBitWidth;

        return unchecked(long.CreateTruncating(accumulator));
    }

    /// <summary>
    /// Executes the <c language="csharp">RentExactly</c> operation.
    /// </summary>
    /// <param name="length">Number of bytes needed, at most 65536.</param>
    /// <returns>The first <paramref name="length"/> bytes of a scratch buffer owned by this stream and reused by every call, so its contents are only valid until the next rent.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is greater than 65536.</exception>
    public Memory<byte> RentExactly(int length)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, RentBufferLength);

        // Allocated on first use: a stream is created per decoded message, and most never rent.
        _buffer ??= new byte[RentBufferLength];

        return _buffer.AsMemory(start: 0, length);
    }

    /// <summary>
    /// Decodes an already received frame into its typed message, using this connection's game version and command tables; useful
    /// when frames are read with <see cref="ReadContainerAsync"/> and decoded separately.
    /// </summary>
    /// <param name="container">A decrypted frame with its message id, version and payload.</param>
    /// <returns>The typed message, or a <see cref="PassthroughMessage"/> with the raw payload when no type owns the id; a client hello or login also updates this stream's game version.</returns>
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
    /// <exception cref="InvalidOperationException">Thrown if encryption has already been set up for this connection.</exception>
    public ValueTask SetupEncryptionAsync(RemotePeerRole with, ReadOnlyMemory<byte> sessionKey, CancellationToken cancellationToken = default)
    {
        return _transport.SetupEncryptionAsync(with, sessionKey, cancellationToken);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToArray</c> operation.
    /// </summary>
    /// <returns>A copy of the whole in-memory payload, including a trailing byte of packed booleans that is flushed first; a network stream throws <see cref="NotSupportedException"/>.</returns>
    public byte[] ToArray()
    {
        FlushWriteBoolean();

        return GetMemoryStream().ToArray();
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    /// <returns>The in-memory payload as upper-case hexadecimal, cut to 125 digits plus <c>...</c> when longer than 64 bytes; the type name for a network stream.</returns>
    public override string? ToString()
    {
        if (!TryGet(out MemoryStream? memoryStream))
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
    /// <param name="source">Bytes written as is after any pending packed booleans are flushed.</param>
    public override void Write(ReadOnlySpan<byte> source)
    {
        FlushWriteBoolean();
        BaseStream.Write(source);
    }

    /// <summary>
    /// Writes <c language="csharp">Async</c> to the stream.
    /// </summary>
    /// <param name="source">Raw bytes written as is, without framing or encryption; they are counted as sent traffic.</param>
    /// <param name="cancellationToken">Stops waiting even when the underlying write does not observe cancellation itself.</param>
    public async ValueTask WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
    {
        FlushWriteBoolean();

        await BaseStream
            .WriteAsync(source, cancellationToken)
            .AsTask()
            .WaitAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        Traffic?.Send(source.Length);
    }

    /// <summary>
    /// Writes <c language="csharp">MessageAsync</c> to the stream.
    /// </summary>
    /// <typeparam name="TValue">Concrete message type, which must be registered so its wire id can be looked up for this stream's game version.</typeparam>
    /// <param name="message">Message to encode, frame and send; a client hello or login also sets this stream's game version.</param>
    /// <param name="cancellationToken">Stops waiting for the write lock or the socket write.</param>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
    public Task WriteAsync<TValue>(TValue message, CancellationToken cancellationToken = default)
        where TValue : class, IMessage
    {
        ArgumentNullException.ThrowIfNull(message);

        return _transport.WriteAsync(message, cancellationToken);
    }

    /// <summary>
    /// Writes <c language="csharp">Boolean</c> to the stream.
    /// </summary>
    /// <param name="value">Flag stored in the next bit of a packing byte, lowest bit first; the byte is written once eight flags are collected or another value is written.</param>
    public override void WriteBoolean(bool value)
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
    /// <param name="value">Byte written as is.</param>
    public override void WriteByte(byte value)
    {
        FlushWriteBoolean();
        BaseStream.WriteByte(value);
    }

    /// <summary>
    /// Writes <c language="csharp">ByteArray</c> to the stream.
    /// </summary>
    /// <param name="source">Bytes written after their length as a big-endian 32-bit integer.</param>
    public override void WriteByteArray(ReadOnlySpan<byte> source)
    {
        WriteInt32(source.Length);
        Write(source);
    }

    /// <summary>
    /// Writes <c language="csharp">ContainerAsync</c> to the stream.
    /// </summary>
    /// <param name="messageContainer">Frame to send with its own id and header version; its payload is encrypted once encryption is set up and must not exceed <see cref="MaximumPayloadLength"/>.</param>
    /// <param name="cancellationToken">Stops waiting for the write lock or the socket write.</param>
    public ValueTask WriteContainerAsync(MessageContainer messageContainer, CancellationToken cancellationToken = default)
    {
        return _transport.WriteContainerAsync(messageContainer, cancellationToken);
    }

    /// <summary>
    /// Writes a big-endian 16-bit integer.
    /// </summary>
    /// <param name="value">Signed value written in two bytes, most significant first.</param>
    public override void WriteInt16(short value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(short)];

        BinaryPrimitives.WriteInt16BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">Int32</c> to the stream.
    /// </summary>
    /// <param name="value">Signed value written in four bytes, most significant first.</param>
    public override void WriteInt32(int value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(int)];

        BinaryPrimitives.WriteInt32BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">Int64</c> to the stream.
    /// </summary>
    /// <param name="value">Signed value written in eight bytes, most significant first.</param>
    public override void WriteInt64(long value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(long)];

        BinaryPrimitives.WriteInt64BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">LongId</c> to the stream.
    /// </summary>
    /// <param name="logicLong">Id written as its high word then its low word, each a big-endian 32-bit integer.</param>
    public override void WriteLongId(LongId logicLong)
    {
        WriteInt32(logicLong.HighInt32);
        WriteInt32(logicLong.LowInt32);
    }

    /// <summary>
    /// Writes <c language="csharp">OptionalByteArray</c> to the stream.
    /// </summary>
    /// <param name="source">Bytes written after a big-endian 32-bit length, or <see langword="null"/> (the default) to write only a length of -1.</param>
    public override void WriteOptionalByteArray(ReadOnlyMemory<byte>? source = null)
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
    /// <param name="value">Id written after a packed <see langword="true"/> flag, or <see langword="null"/> to write only a packed <see langword="false"/> flag.</param>
    public override void WriteOptionalLongId(LongId? value)
    {
        WriteBoolean(value is not null);

        if (value is not null)
            WriteLongId(value.Value);
    }

    /// <summary>
    /// Writes <c language="csharp">OptionalString</c> to the stream.
    /// </summary>
    /// <param name="value">Text written as its UTF-8 byte count (big-endian 32-bit) and bytes, or <see langword="null"/> (the default) to write only a count of -1.</param>
    public override void WriteOptionalString(string? value = null)
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
    /// <param name="value">Text written as its UTF-8 byte count (big-endian 32-bit) and bytes; the empty string writes a count of 0.</param>
    public override void WriteString(string value)
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
    /// <param name="value">Unsigned value written in two bytes, most significant first.</param>
    public override void WriteUInt16(ushort value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(ushort)];

        BinaryPrimitives.WriteUInt16BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">UInt32</c> to the stream.
    /// </summary>
    /// <param name="value">Unsigned value written in four bytes, most significant first.</param>
    public override void WriteUInt32(uint value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(uint)];

        BinaryPrimitives.WriteUInt32BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">UInt64</c> to the stream.
    /// </summary>
    /// <param name="value">Unsigned value written in eight bytes, most significant first.</param>
    public override void WriteUInt64(ulong value)
    {
        FlushWriteBoolean();

        Span<byte> span = stackalloc byte[sizeof(ulong)];

        BinaryPrimitives.WriteUInt64BigEndian(span, value);
        BaseStream.Write(span);
    }

    /// <summary>
    /// Writes <c language="csharp">VarInt</c> to the stream.
    /// </summary>
    /// <param name="valueToWrite">Any 32-bit value; the first byte takes a sign bit and the six lowest bits, and each further byte seven more, so values from -63 to 63 fit in one byte and the largest magnitudes take five.</param>
    public override void WriteVarInt(int valueToWrite)
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
    /// <param name="source">Bytes written after their count as a variable-length integer.</param>
    /// <exception cref="InvalidDataException"><paramref name="source"/> is longer than <see cref="MaximumPayloadLength"/>.</exception>
    public override void WriteVarIntByteArray(ReadOnlySpan<byte> source)
    {
        if (source.Length > MaximumPayloadLength)
            throw new InvalidDataException(message: "Var-length byte array is too large.");

        WriteVarInt(source.Length);
        Write(source);
    }

    /// <summary>
    /// Writes <c language="csharp">VarLong</c> to the stream.
    /// </summary>
    /// <param name="valueToWrite">Any 64-bit value, encoded like a variable-length 32-bit integer but in up to ten bytes.</param>
    public override void WriteVarLong(long valueToWrite)
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
        return !TryGet(out MemoryStream? memoryStream)
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

    private bool TryGet([MaybeNullWhen(false)] out MemoryStream memoryStream)
    {
        memoryStream = BaseStream as MemoryStream;

        return memoryStream is not null;
    }
}
