using SupercellProxy.Playground.Crypto;
using SupercellProxy.Playground.Network.Sides;
using SupercellProxy.Playground.Supercell;
using System.Security.Cryptography;

namespace SupercellProxy.Playground.Network.Streams;

public partial class SupercellStream
{
    private const int PromonPadSize = 508;

    private class Encryption(Side with, Memory<byte> localPrivateKey, Memory<byte> sessionKey)
    {
        public Side With { get; init; } = with;
        public Memory<byte> LocalPrivateKey { get; init; } = localPrivateKey;
        public Memory<byte> LocalPublicKey { get; init; } = NaClV3Crypto.CryptoScalarMultBase(localPrivateKey.Span);
        public Memory<byte> RemotePublicKey { get; set; }
        public Memory<byte> SessionKey { get; init; } = sessionKey;
        public Nonce? TempNonce { get; set; }
        public Nonce? ServerboundNonce { get; set; }
        public Nonce? ClientboundNonce { get; set; }
        public Memory<byte> SharedKey { get; set; }

        public Encryption(Side with, Memory<byte> localPrivateKey, Memory<byte> remotePublicKey, Memory<byte> sessionKey) : this(with, localPrivateKey, sessionKey)
        {
            RemotePublicKey = remotePublicKey;
            TempNonce = new Nonce(clientPublicKey: LocalPublicKey.Span, serverPublicKey: RemotePublicKey.Span);
        }
    }

    private Encryption? _encryption;

    public async ValueTask SetupEncryptionAsync(Side with, Memory<byte> sessionKey, CancellationToken cancellationToken = default)
    {
        if (_encryption is not null)
            throw new InvalidOperationException("Encryption is already set up.");

        if (with is Side.Server)
        {
            _encryption = new Encryption(
                with: with,
                localPrivateKey: RandomNumberGenerator.GetBytes(count: 32),
                remotePublicKey: await HayDayApi.GetServerPublicKeyAsync(cancellationToken),
                sessionKey: sessionKey);
        }
        else
        {
            _encryption = new Encryption(
                with: with,
                localPrivateKey: Proxy.StandardPrivateKey,
                sessionKey: sessionKey);
        }
    }

    private MemoryStream Encrypt(MemoryStream memoryStream)
    {
        if (_encryption is null)
            throw new InvalidOperationException("Encryption is not set up.");

        var payload = memoryStream.ToArray();

        if (_encryption.With is Side.Server)
        {
            // We are encrypting messages to the server
            _encryption.ServerboundNonce ??= new Nonce();

            if (_encryption.SharedKey.IsEmpty)
            {
                if (_encryption.TempNonce is null)
                {
                    if (_encryption.RemotePublicKey.IsEmpty)
                        throw new InvalidOperationException("Remote public key is not set.");

                    _encryption.TempNonce = new Nonce(clientPublicKey: _encryption.LocalPublicKey.Span, serverPublicKey: _encryption.RemotePublicKey.Span);
                }

                var ciphertext = NaClV3Crypto.Box(
                [
                    .. _encryption.SessionKey.Span,
                    .. _encryption.ServerboundNonce.Span,
                    .. payload,
                    .. stackalloc byte[PromonPadSize]
                ], _encryption.TempNonce.Span, _encryption.RemotePublicKey.Span, _encryption.LocalPrivateKey.Span);

                memoryStream = new MemoryStream([.. _encryption.LocalPublicKey.Span, .. ciphertext], writable: false);
            }
            else
            {
                _encryption.ServerboundNonce.Increment();
                var ciphertext = NaClV3Crypto.SecretBox(payload, _encryption.ServerboundNonce.Span, _encryption.SharedKey.Span);

                memoryStream = new MemoryStream(ciphertext, writable: false);
            }
        }
        else
        {
            // We are encrypting messages to the client
            if (_encryption.SharedKey.IsEmpty)
            {
                if (_encryption.ServerboundNonce is null)
                    throw new InvalidOperationException("Serverbound nonce is not set.");

                _encryption.SharedKey = RandomNumberGenerator.GetBytes(count: 32);
                _encryption.ClientboundNonce = new Nonce(nonceBytes: RandomNumberGenerator.GetBytes(count: 24));

                var boxNonce = new Nonce(nonceBytes: _encryption.ServerboundNonce.Span, clientPublicKey: _encryption.RemotePublicKey.Span, serverPublicKey: _encryption.LocalPublicKey.Span);

                var ciphertext = NaClV3Crypto.Box(
                    [.. _encryption.ClientboundNonce.Span, .. _encryption.SharedKey.Span, .. payload],
                    boxNonce.Span,
                    _encryption.RemotePublicKey.Span,
                    _encryption.LocalPrivateKey.Span);

                memoryStream = new MemoryStream(ciphertext, writable: false);
            }
            else
            {
                if (_encryption.ClientboundNonce is null)
                    throw new InvalidOperationException("Clientbound nonce is not set.");

                _encryption.ClientboundNonce.Increment();
                var ciphertext = NaClV3Crypto.SecretBox(payload, _encryption.ClientboundNonce.Span, _encryption.SharedKey.Span);

                memoryStream = new MemoryStream(ciphertext, writable: false);
            }
        }

        return memoryStream;
    }

    private MemoryStream Decrypt(MemoryStream memoryStream)
    {
        if (_encryption is null)
            throw new InvalidOperationException("Encryption is not set up.");

        var payload = memoryStream.ToArray();

        if (_encryption.With is Side.Server)
        {
            // We are decrypting messages from the server
            if (_encryption.SharedKey.IsEmpty || _encryption.ClientboundNonce is null)
            {
                _encryption.ServerboundNonce ??= new Nonce();

                var nonce = new Nonce(nonceBytes: _encryption.ServerboundNonce.Span, clientPublicKey: _encryption.LocalPublicKey.Span, serverPublicKey: _encryption.RemotePublicKey.Span);
                var plaintext = NaClV3Crypto.BoxOpen(payload, nonce.Span, _encryption.RemotePublicKey.Span, _encryption.LocalPrivateKey.Span);

                _encryption.ClientboundNonce = new Nonce(nonceBytes: plaintext.AsSpan(..24));
                _encryption.SharedKey = plaintext.AsMemory(24..56);

                memoryStream = new MemoryStream(plaintext, 56, plaintext.Length - 56, writable: false);
            }
            else
            {
                _encryption.ClientboundNonce.Increment();
                var plaintext = NaClV3Crypto.SecretBoxOpen(payload, _encryption.ClientboundNonce.Span, _encryption.SharedKey.Span);

                memoryStream = new MemoryStream(plaintext, writable: false);
            }
        }
        else
        {
            // We are decrypting messages from the client
            if (_encryption.SharedKey.IsEmpty || _encryption.ClientboundNonce is null)
            {
                var payloadMemory = payload.AsMemory();

                var clientPublicKey = payloadMemory[..32];
                var ciphertext = payloadMemory[32..];

                _encryption.RemotePublicKey = clientPublicKey;
                _encryption.TempNonce = new Nonce(clientPublicKey: _encryption.RemotePublicKey.Span, serverPublicKey: _encryption.LocalPublicKey.Span);

                var plaintext = NaClV3Crypto.BoxOpen(ciphertext.Span, _encryption.TempNonce.Span, _encryption.RemotePublicKey.Span, _encryption.LocalPrivateKey.Span);
                var plaintextMemory = plaintext.AsMemory();

                var receivedSessionKey = plaintextMemory[.._encryption.SessionKey.Length];

                if (!receivedSessionKey.Span.SequenceEqual(_encryption.SessionKey.Span))
                    throw new InvalidOperationException("Received session key does not match the expected session key.");

                var serverboundNonce = plaintextMemory[receivedSessionKey.Length..(receivedSessionKey.Length + 24)];
                var plaintextPayload = plaintextMemory[(receivedSessionKey.Length + serverboundNonce.Length)..^PromonPadSize];

                _encryption.ServerboundNonce = new Nonce(nonceBytes: serverboundNonce.Span);

                memoryStream = new MemoryStream(plaintextPayload.ToArray(), writable: false);
            }
            else
            {
                if (_encryption.ServerboundNonce is null)
                    throw new InvalidOperationException("Serverbound nonce is not set.");

                _encryption.ServerboundNonce.Increment();
                var plaintext = NaClV3Crypto.SecretBoxOpen(payload, _encryption.ServerboundNonce.Span, _encryption.SharedKey.Span);

                memoryStream = new MemoryStream(plaintext, writable: false);
            }
        }

        return memoryStream;
    }
}
