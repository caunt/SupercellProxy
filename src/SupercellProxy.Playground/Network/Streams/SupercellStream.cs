using SupercellProxy.Playground.Network.Messages;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace SupercellProxy.Playground.Network.Streams;

public partial class SupercellStream(Stream stream, bool leaveOpen = true) : IAsyncDisposable, IDisposable
{
    public const int MaxPayloadLength = 0x1000000;
    public long Position { get => GetMemoryStream().Position; set => GetMemoryStream().Position = value; }
    public long Length => GetMemoryStream().Length;

    public static SupercellStream Create()
    {
        return new SupercellStream(new MemoryStream());
    }

    public MessageContainer ReadMessage()
    {
        var headerSpan = ReadExactly(stackalloc byte[7]);

        var id = BinaryPrimitives.ReadUInt16BigEndian(headerSpan[0..2]);
        var length = (headerSpan[2] << 16) | (headerSpan[3] << 8) | headerSpan[4];
        var version = BinaryPrimitives.ReadUInt16BigEndian(headerSpan[5..7]);

        var buffer = new byte[length];
        _ = ReadExactly(buffer);

        var memoryStream = new MemoryStream(buffer);

        if (_encryption is not null)
            memoryStream = Decrypt(memoryStream);

        return new MessageContainer(id, version, new SupercellStream(memoryStream));
    }

    public async ValueTask<MessageContainer> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        var headerMemory = RentExactly(7);
        await ReadExactlyAsync(headerMemory, cancellationToken);

        var headerSpan = headerMemory.Span;
        var id = BinaryPrimitives.ReadUInt16BigEndian(headerSpan[0..2]);
        var length = (headerSpan[2] << 16) | (headerSpan[3] << 8) | headerSpan[4];
        var version = BinaryPrimitives.ReadUInt16BigEndian(headerSpan[5..7]);

        var buffer = new byte[length];
        _ = await ReadExactlyAsync(buffer, cancellationToken);

        var memoryStream = new MemoryStream(buffer);

        if (_encryption is not null)
            memoryStream = Decrypt(memoryStream);

        return new MessageContainer(id, version, new SupercellStream(memoryStream));
    }

    public void WriteMessage(MessageContainer messageContainer)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(messageContainer.Payload.Length, MaxPayloadLength, nameof(messageContainer.Payload.Length));

        var memoryStream = messageContainer.Payload.GetMemoryStream();
        memoryStream.Position = 0;

        if (_encryption is not null)
            memoryStream = Encrypt(memoryStream);

        var headerSpan = (stackalloc byte[7]);

        BinaryPrimitives.WriteUInt16BigEndian(headerSpan[..2], messageContainer.Id);

        var length = memoryStream.Length;

        headerSpan[2] = (byte)(length >> 16);
        headerSpan[3] = (byte)(length >> 8);
        headerSpan[4] = (byte)length;

        BinaryPrimitives.WriteUInt16BigEndian(headerSpan[5..7], messageContainer.Version);

        Write(headerSpan);

        memoryStream.CopyTo(stream);
    }

    public async ValueTask WriteMessageAsync(MessageContainer messageContainer, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(messageContainer.Payload.Length, MaxPayloadLength, nameof(messageContainer.Payload.Length));

        var memoryStream = messageContainer.Payload.GetMemoryStream();
        memoryStream.Position = 0;

        if (_encryption is not null)
            memoryStream = Encrypt(memoryStream);

        var headerMemory = RentExactly(7);
        var headerSpan = headerMemory.Span;

        BinaryPrimitives.WriteUInt16BigEndian(headerSpan[..2], messageContainer.Id);

        var length = memoryStream.Length;

        headerSpan[2] = (byte)(length >> 16);
        headerSpan[3] = (byte)(length >> 8);
        headerSpan[4] = (byte)length;

        BinaryPrimitives.WriteUInt16BigEndian(headerSpan[5..7], messageContainer.Version);

        await WriteAsync(headerMemory, cancellationToken);

        await memoryStream.CopyToAsync(stream, cancellationToken);
    }

    public byte[] ToArray()
    {
        return GetMemoryStream().ToArray();
    }

    private MemoryStream GetMemoryStream()
    {
        if (!TryGetMemoryStream(out var memoryStream))
            throw new NotSupportedException("This is online stream.");

        return memoryStream;
    }

    private bool TryGetMemoryStream([MaybeNullWhen(false)] out MemoryStream memoryStream)
    {
        memoryStream = stream as MemoryStream;
        return memoryStream is not null;
    }

    private static SupercellStream CreateOfflineStream(ReadOnlyMemory<byte> memory)
    {
        if (MemoryMarshal.TryGetArray(memory, out var segment) && segment.Array is not null)
            return new SupercellStream(new MemoryStream(segment.Array, segment.Offset, segment.Count));

        return new SupercellStream(new MemoryStream(memory.ToArray()));
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        FlushWriteBoolean();

        if (leaveOpen)
            return;

        await stream.DisposeAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        FlushWriteBoolean();

        if (leaveOpen)
            return;

        stream.Dispose();
    }

    public override string? ToString()
    {
        if (!TryGetMemoryStream(out var memoryStream))
            return base.ToString();

        var builder = new StringBuilder();
        var hex = Convert.ToHexString(memoryStream.ToArray());

        if (hex.Length <= 128)
        {
            builder.Append(hex);
        }
        else
        {
            builder.Append(hex, 0, 125);
            builder.Append("...");
        }

        return builder.ToString();
    }
}
