using Nito.AsyncEx;
using SupercellProxy.Playground.Network.Messages;
using System.Buffers.Binary;

namespace SupercellProxy.Playground.Network.Streams;

public partial class SupercellStream
{
    private readonly AsyncLock _readLock = new();
    private readonly AsyncLock _writeLock = new();

    public async Task WriteMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage
    {
        await WriteContainerAsync(message.ToContainer(MessageRegistry.GetId(message), version: MessageRegistry.GetVersion(message)), cancellationToken);
    }

    public async Task<T> ReadMessageAsync<T>(CancellationToken cancellationToken) where T : IMessage
    {
        var genericMessage = await ReadMessageAsync(cancellationToken);

        if (genericMessage is not T message)
            throw new InvalidOperationException($"Expected message {typeof(T)}, but received {genericMessage}.");

        return message;
    }

    public async Task<IMessage> ReadMessageAsync(CancellationToken cancellationToken)
    {
        var container = await ReadContainerAsync(cancellationToken);
        var message = MessageRegistry.Resolve(container);

        if (container.Payload.Position != container.Payload.Length)
            Console.WriteLine($"Warning: Not all payload data was consumed for message {message}. Remaining bytes: {container.Payload.Length - container.Payload.Position}");

        return message;
    }

    public MessageContainer ReadContainer()
    {
        using var disposable = _readLock.Lock();

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

    public async ValueTask<MessageContainer> ReadContainerAsync(CancellationToken cancellationToken = default)
    {
        using var disposable = await _readLock.LockAsync(cancellationToken);

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

    public void WriteContainer(MessageContainer messageContainer)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(messageContainer.Payload.Length, MaxPayloadLength, nameof(messageContainer.Payload.Length));

        using var disposable = _writeLock.Lock();

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

    public async ValueTask WriteContainerAsync(MessageContainer messageContainer, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(messageContainer.Payload.Length, MaxPayloadLength, nameof(messageContainer.Payload.Length));

        using var disposable = await _writeLock.LockAsync(cancellationToken);

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
}
