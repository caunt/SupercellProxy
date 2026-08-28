using System.Buffers.Binary;
using System.Globalization;
using System.Net.Sockets;
using Nito.AsyncEx;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Transport.Exceptions;

namespace SupercellProxy.Playground.Network.Transport;

internal sealed partial class MessageStream
{
    private readonly AsyncLock _readLock = new();
    private readonly AsyncLock _writeLock = new();

    /// <summary>
    /// Writes <c language="csharp">MessageAsync</c> to the stream.
    /// </summary>
    public async Task WriteMessageAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : IMessage
    {
        await WriteContainerAsync(
                message.ToContainer(
                    MessageRegistry.GetId(message),
                    version: MessageRegistry.GetVersion(message)
                ),
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads <c language="csharp">MessageAsync</c> from the stream.
    /// </summary>
    public async Task<T> ReadMessageAsync<T>(CancellationToken cancellationToken = default)
        where T : IMessage
    {
        var genericMessage = await ReadMessageAsync(cancellationToken).ConfigureAwait(false);

        if (genericMessage is not T message)
            throw new InvalidOperationException(
                $"Expected message {typeof(T)}, but received {genericMessage}."
            );

        return message;
    }

    /// <summary>
    /// Reads <c language="csharp">UntilMessageAsync</c> from the stream.
    /// </summary>
    public async Task<T> ReadUntilMessageAsync<T>(CancellationToken cancellationToken = default)
        where T : IMessage
    {
        IMessage message;

        do
        {
            message = await ReadMessageAsync(cancellationToken).ConfigureAwait(false);
        } while (message is not T);

        return message is T expectedMessage
            ? expectedMessage
            : throw new InvalidOperationException("Message loop ended without the expected type.");
    }

    /// <summary>
    /// Reads <c language="csharp">MessageAsync</c> from the stream.
    /// </summary>
    public async Task<IMessage> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        var container = await ReadContainerAsync(cancellationToken).ConfigureAwait(false);
        return ResolveMessage(container);
    }

    internal IMessage ResolveMessage(MessageContainer container)
    {
        var message = MessageRegistry.Resolve(container, CommandDataResolver);

        if (container.Payload.Position != container.Payload.Length)
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Warning: Not all payload data was consumed for message {message}. Remaining bytes: {container.Payload.Length - container.Payload.Position}"
                )
            );

        return message;
    }

    /// <summary>
    /// Reads <c language="csharp">ContainerAsync</c> from the stream.
    /// </summary>
    public async ValueTask<MessageContainer> ReadContainerAsync(
        CancellationToken cancellationToken = default
    )
    {
        using var disposable = await _readLock.LockAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var headerMemory = RentExactly(7);
            await ReadExactlyAsync(headerMemory, cancellationToken).ConfigureAwait(false);

            var headerSpan = headerMemory.Span;
            var id = BinaryPrimitives.ReadUInt16BigEndian(headerSpan[0..2]);
            var length = (headerSpan[2] << 16) | (headerSpan[3] << 8) | headerSpan[4];
            var version = BinaryPrimitives.ReadUInt16BigEndian(headerSpan[5..7]);

            var buffer = new byte[length];
            _ = await ReadExactlyAsync(buffer, cancellationToken).ConfigureAwait(false);

            Memory<byte> payload = buffer;
            if (_encryption is not null)
            {
                var encryptedStream = new MemoryStream(buffer);
                await using (encryptedStream.ConfigureAwait(false))
                {
                    var decryptedStream = Decrypt(encryptedStream);
                    await using (decryptedStream.ConfigureAwait(false))
                        payload = decryptedStream.ToArray();
                }
            }

            var messageStream = Create(payload);
            try
            {
                return new MessageContainer(id, version, messageStream);
            }
            finally
            {
                messageStream.Dispose();
            }
        }
        catch (EndOfStreamException exception)
        {
            throw new StreamClosedException(StreamClosedException.DefaultMessage, exception);
        }
    }

    /// <summary>
    /// Writes <c language="csharp">ContainerAsync</c> to the stream.
    /// </summary>
    public async ValueTask WriteContainerAsync(
        MessageContainer messageContainer,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(
            messageContainer.Payload.Length,
            MaxPayloadLength,
            nameof(messageContainer)
        );

        using var disposable = await _writeLock.LockAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var memoryStream = messageContainer.Payload.GetMemoryStream();
            memoryStream.Position = 0;

            if (_encryption is not null)
                memoryStream = Encrypt(memoryStream);

            var headerMemory = RentExactly(7);
            var headerSpan = headerMemory.Span;

            BinaryPrimitives.WriteUInt16BigEndian(headerSpan[..2], messageContainer.Id);

            var length = memoryStream.Length;

            headerSpan[2] = byte.CreateTruncating((length >> 16));
            headerSpan[3] = byte.CreateTruncating((length >> 8));
            headerSpan[4] = byte.CreateTruncating(length);

            BinaryPrimitives.WriteUInt16BigEndian(headerSpan[5..7], messageContainer.Version);

            await WriteAsync(headerMemory, cancellationToken).ConfigureAwait(false);

            await memoryStream.CopyToAsync(_stream, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
            when (exception
                    is EndOfStreamException
                        or IOException { InnerException: SocketException }
            )
        {
            throw new StreamClosedException(StreamClosedException.DefaultMessage, exception);
        }
    }
}
