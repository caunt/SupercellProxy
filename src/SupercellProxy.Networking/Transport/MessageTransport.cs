using System.Buffers.Binary;
using System.Globalization;
using System.Net.Sockets;
using System.Security.Cryptography;

using Nito.AsyncEx;

using SupercellProxy.Networking.Cryptography;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Transport;

internal sealed class MessageTransport(MessageStream stream)
{
    private readonly AsyncLock _readLock = new();
    private readonly AsyncLock _writeLock = new();
    private MessageEncryption? _encryption;

    /// <summary>
    /// Reads <c language="csharp">ContainerAsync</c> from the stream.
    /// </summary>
    internal async ValueTask<MessageContainer> ReadContainerAsync(CancellationToken cancellationToken = default)
    {
        using IDisposable disposable = await _readLock.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            Memory<byte> headerMemory = await stream.ReadExactlyAsync(stream.RentExactly(length: 7), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            Span<byte> headerSpan = headerMemory.Span;
            ushort identifier = BinaryPrimitives.ReadUInt16BigEndian(headerSpan[0..2]);
            int length = (headerSpan[index: 2] << 16) | (headerSpan[index: 3] << 8) | headerSpan[index: 4];
            ushort version = BinaryPrimitives.ReadUInt16BigEndian(headerSpan[5..7]);

            byte[] buffer = new byte[length];

            Memory<byte> payload = await stream.ReadExactlyAsync(buffer, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (_encryption is not null)
            {
                MemoryStream encryptedStream = new(buffer);

                await using (encryptedStream.ConfigureAwait(continueOnCapturedContext: false))
                {
                    MemoryStream decryptedStream = _encryption.Decrypt(encryptedStream);

                    await using (decryptedStream.ConfigureAwait(continueOnCapturedContext: false))
                        payload = decryptedStream.ToArray();
                }
            }

            MessageStream messageStream = MessageStream.Create(payload);

            try
            {
                return new MessageContainer(identifier, version, messageStream);
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
    /// Reads <c language="csharp">MessageAsync</c> from the stream.
    /// </summary>
    internal async Task<TValue> ReadMessageAsync<TValue>(CancellationToken cancellationToken = default)
        where TValue : IMessage
    {
        IMessage genericMessage = await ReadMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return genericMessage is not TValue message
            ? throw new InvalidOperationException($"Expected message {typeof(TValue)}, but received {genericMessage}.")
            : message;
    }

    /// <summary>
    /// Reads <c language="csharp">MessageAsync</c> from the stream.
    /// </summary>
    internal async Task<IMessage> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        MessageContainer container = await ReadContainerAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return ResolveMessage(container);
    }

    /// <summary>
    /// Reads <c language="csharp">UntilMessageAsync</c> from the stream.
    /// </summary>
    internal async Task<TValue> ReadUntilMessageAsync<TValue>(CancellationToken cancellationToken = default)
        where TValue : IMessage
    {
        IMessage message;

        do
            message = await ReadMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        while (message is not TValue);

        return message is TValue expectedMessage
            ? expectedMessage
            : throw new InvalidOperationException(message: "Message loop ended without the expected type.");
    }

    /// <summary>
    /// Provides the Resolve Message value or operation.
    /// </summary>
    internal IMessage ResolveMessage(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        IMessage message = MessageRegistry.Resolve(container, stream.CommandDataResolver);

        if (container.Payload.Position != container.Payload.Length)
        {
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Warning: Not all payload data was consumed for message {message}. Remaining bytes: {container.Payload.Length - container.Payload.Position}"
                )
            );
        }

        return message;
    }

    /// <summary>
    /// <para>Initializes encryption for the current connection using the specified session key and connection side.</para>
    /// </summary>
    /// <param name="with">Specifies the connection side, indicating whether encryption is being set up for the server or the client.</param>
    /// <param name="sessionKey">The session key to verify during the handshake phase.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation of setting up encryption.</returns>
    /// <exception cref="InvalidOperationException">Thrown if encryption has already been set up for this connection.</exception>
    internal async ValueTask SetupEncryptionAsync(RemotePeerRole with, Memory<byte> sessionKey, CancellationToken cancellationToken = default)
    {
        if (_encryption is not null)
            throw new InvalidOperationException(message: "Encryption is already set up.");

        _encryption = new MessageEncryption(
            with: with,
            sessionKey: sessionKey,
            localPrivateKey: with is RemotePeerRole.Server
                ? RandomNumberGenerator.GetBytes(count: 32)
                : ProxyKeyMaterial.StandardPrivateKey,
            remotePublicKey: with is RemotePeerRole.Server
                ? await (
                    stream.ServerKeySource
                    ?? throw new InvalidOperationException(message: "Configure a server key source before connecting upstream.")
                )
                    .GetServerPublicKeyAsync(cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false)
                : default(Memory<byte>)
        );
    }

    /// <summary>
    /// Writes <c language="csharp">ContainerAsync</c> to the stream.
    /// </summary>
    internal async ValueTask WriteContainerAsync(MessageContainer messageContainer, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messageContainer);
        ArgumentNullException.ThrowIfNull(messageContainer);
        messageContainer.Payload.FlushWriteBoolean();
        ArgumentOutOfRangeException.ThrowIfGreaterThan(messageContainer.Payload.Length, MessageStream.MaximumPayloadLength, nameof(messageContainer));

        using IDisposable disposable = await _writeLock.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            MemoryStream memoryStream = messageContainer.Payload.GetMemoryStream();
            memoryStream.Position = 0;

            if (_encryption is not null)
                memoryStream = _encryption.Encrypt(memoryStream);

            Memory<byte> headerMemory = stream.RentExactly(length: 7);
            Span<byte> headerSpan = headerMemory.Span;

            BinaryPrimitives.WriteUInt16BigEndian(headerSpan[..2], messageContainer.Identifier);

            long length = memoryStream.Length;

            headerSpan[index: 2] = byte.CreateTruncating(length >> 16);
            headerSpan[index: 3] = byte.CreateTruncating(length >> 8);
            headerSpan[index: 4] = byte.CreateTruncating(length);

            BinaryPrimitives.WriteUInt16BigEndian(headerSpan[5..7], messageContainer.Version);

            await stream.WriteAsync(headerMemory, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            await memoryStream.CopyToAsync(stream.BaseStream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (Exception exception)
            when (exception is EndOfStreamException or IOException { InnerException: SocketException })
        {
            throw new StreamClosedException(StreamClosedException.DefaultMessage, exception);
        }
    }

    /// <summary>
    /// Writes <c language="csharp">MessageAsync</c> to the stream.
    /// </summary>
    internal async Task WriteMessageAsync<TValue>(TValue message, CancellationToken cancellationToken = default)
        where TValue : IMessage
    {
        await WriteContainerAsync(message.ToContainer(MessageRegistry.GetIdentifier(message), version: MessageRegistry.GetVersion(message)), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

}
