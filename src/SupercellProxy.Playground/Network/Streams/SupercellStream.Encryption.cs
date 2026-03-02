using SupercellProxy.Playground.Crypto;
using SupercellProxy.Playground.Network.Sides;
using SupercellProxy.Playground.Supercell;
using System.Security.Cryptography;

namespace SupercellProxy.Playground.Network.Streams;

/// <summary>
/// Implements the encryption and decryption logic for the Supercell protocol, including both the handshake phase and the subsequent encrypted communication.
/// https://github.com/ReversedCell/ScDocumentation/wiki/Encryption-Setup
/// https://github.com/ReversedCell/ScDocumentation/wiki/Protocol
/// </summary>
public partial class SupercellStream
{
    private const int PromonPadSize = 508;

    private Encryption? _encryption;

    /// <summary>
    /// Initializes encryption for the current connection using the specified session key and connection side.
    /// </summary>
    /// <param name="with">Specifies the connection side, indicating whether encryption is being set up for the server or the client.</param>
    /// <param name="sessionKey">The session key to verify during the handshake phase.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation of setting up encryption.</returns>
    /// <exception cref="InvalidOperationException">Thrown if encryption has already been set up for this connection.</exception>
    public async ValueTask SetupEncryptionAsync(Side with, Memory<byte> sessionKey, CancellationToken cancellationToken = default)
    {
        if (_encryption is not null)
            throw new InvalidOperationException("Encryption is already set up.");

        _encryption = new Encryption(
            with: with,
            sessionKey: sessionKey,
            localPrivateKey: with is Side.Server ? RandomNumberGenerator.GetBytes(count: 32) : Proxy.StandardPrivateKey,
            remotePublicKey: with is Side.Server ? await HayDayApi.GetServerPublicKeyAsync(cancellationToken) : default(Memory<byte>));
    }

    private static MemoryStream DecryptServerboundHandshake(Encryption encryption, byte[] payload)
    {
        var payloadMemory = payload.AsMemory();

        encryption.RemotePublicKey = payloadMemory[..32];
        var ciphertext = payloadMemory[32..];

        var temporaryNonce = new Nonce(clientPublicKey: encryption.RemotePublicKey.Span, serverPublicKey: encryption.LocalPublicKey.Span);
        var plaintext = NaClV3Crypto.BoxOpen(ciphertext.Span, temporaryNonce.Span, encryption.RemotePublicKey.Span, encryption.LocalPrivateKey.Span);
        var plaintextMemory = plaintext.AsMemory();

        var receivedSessionKey = plaintextMemory[..encryption.SessionKey.Length];

        if (!receivedSessionKey.Span.SequenceEqual(encryption.SessionKey.Span))
            throw new InvalidOperationException("Received session key does not match the expected session key.");

        var payloadOffset = receivedSessionKey.Length;
        var serverNonce = plaintextMemory[payloadOffset..(payloadOffset + 24)];
        encryption.ReceiveNonce = new Nonce(nonceBytes: serverNonce.Span);

        var payloadStart = payloadOffset + 24;
        var payloadLength = plaintext.Length - payloadStart - PromonPadSize;

        return new MemoryStream(plaintext, index: payloadStart, count: payloadLength, writable: false);
    }

    private static MemoryStream DecryptClientboundHandshake(Encryption encryption, byte[] payload)
    {
        encryption.SendNonce ??= new Nonce();

        var handshakeNonce = new Nonce(nonceBytes: encryption.SendNonce.Span, clientPublicKey: encryption.LocalPublicKey.Span, serverPublicKey: encryption.RemotePublicKey.Span);
        var plaintext = NaClV3Crypto.BoxOpen(payload, handshakeNonce.Span, encryption.RemotePublicKey.Span, encryption.LocalPrivateKey.Span);

        encryption.ReceiveNonce = new Nonce(nonceBytes: plaintext.AsSpan(..24));
        encryption.SharedKey = plaintext.AsMemory(24..56);

        return new MemoryStream(plaintext, index: 56, count: plaintext.Length - 56, writable: false);
    }

    private static MemoryStream EncryptServerboundHandshake(Encryption encryption, byte[] payload)
    {
        encryption.SendNonce ??= new Nonce();

        if (encryption.RemotePublicKey.IsEmpty)
            throw new InvalidOperationException("Remote public key is not set.");

        var temporaryNonce = new Nonce(clientPublicKey: encryption.LocalPublicKey.Span, serverPublicKey: encryption.RemotePublicKey.Span);
        var ciphertext = NaClV3Crypto.Box(
        [
            .. encryption.SessionKey.Span,
            .. encryption.SendNonce.Span,
            .. payload,
            .. stackalloc byte[PromonPadSize]
        ], temporaryNonce.Span, encryption.RemotePublicKey.Span, encryption.LocalPrivateKey.Span);

        return new MemoryStream([.. encryption.LocalPublicKey.Span, .. ciphertext], writable: false);
    }

    private static MemoryStream EncryptClientboundHandshake(Encryption encryption, byte[] payload)
    {
        if (encryption.ReceiveNonce is null)
            throw new InvalidOperationException("Receive nonce is not set.");

        encryption.SharedKey = RandomNumberGenerator.GetBytes(count: 32);
        encryption.SendNonce = new Nonce(nonceBytes: RandomNumberGenerator.GetBytes(count: 24));

        var handshakeNonce = new Nonce(nonceBytes: encryption.ReceiveNonce.Span, clientPublicKey: encryption.RemotePublicKey.Span, serverPublicKey: encryption.LocalPublicKey.Span);
        var ciphertext = NaClV3Crypto.Box(
        [
            .. encryption.SendNonce.Span,
            .. encryption.SharedKey.Span,
            .. payload
        ], handshakeNonce.Span, encryption.RemotePublicKey.Span, encryption.LocalPrivateKey.Span);

        return new MemoryStream(ciphertext, writable: false);
    }

    private MemoryStream Decrypt(MemoryStream memoryStream)
    {
        if (_encryption is null)
            throw new InvalidOperationException("Encryption is not set up.");

        var payload = memoryStream.TryGetBuffer(out var streamBuffer) && streamBuffer.Array is { } buffer && streamBuffer.Offset == 0 && streamBuffer.Count == buffer.Length ? buffer : memoryStream.ToArray();

        if (!_encryption.SharedKey.IsEmpty)
        {
            if (_encryption.ReceiveNonce is null)
                throw new InvalidOperationException("Receive nonce is not set.");

            _encryption.ReceiveNonce.Increment();
            return new MemoryStream(NaClV3Crypto.SecretBoxOpen(payload, _encryption.ReceiveNonce.Span, _encryption.SharedKey.Span), writable: false);
        }

        return _encryption.With is Side.Server
            ? DecryptClientboundHandshake(_encryption, payload)
            : DecryptServerboundHandshake(_encryption, payload);
    }

    private MemoryStream Encrypt(MemoryStream memoryStream)
    {
        if (_encryption is null)
            throw new InvalidOperationException("Encryption is not set up.");

        var payload = memoryStream.TryGetBuffer(out var streamBuffer) && streamBuffer.Array is { } buffer && streamBuffer.Offset == 0 && streamBuffer.Count == buffer.Length ? buffer : memoryStream.ToArray();

        if (!_encryption.SharedKey.IsEmpty)
        {
            if (_encryption.SendNonce is null)
                throw new InvalidOperationException("Send nonce is not set.");

            _encryption.SendNonce.Increment();
            return new MemoryStream(NaClV3Crypto.SecretBox(payload, _encryption.SendNonce.Span, _encryption.SharedKey.Span), writable: false);
        }

        return _encryption.With is Side.Server
            ? EncryptServerboundHandshake(_encryption, payload)
            : EncryptClientboundHandshake(_encryption, payload);
    }

    private class Encryption(Side with, Memory<byte> localPrivateKey, Memory<byte> sessionKey, Memory<byte> remotePublicKey = default)
    {
        public Side With { get; init; } = with;

        public Memory<byte> LocalPrivateKey { get; init; } = localPrivateKey;
        public Memory<byte> LocalPublicKey { get; init; } = NaClV3Crypto.CryptoScalarMultBase(localPrivateKey.Span);
        public Memory<byte> RemotePublicKey { get; set; } = remotePublicKey;

        public Nonce? ReceiveNonce { get; set; }
        public Nonce? SendNonce { get; set; }

        public Memory<byte> SessionKey { get; init; } = sessionKey;
        public Memory<byte> SharedKey { get; set; }
    }
}