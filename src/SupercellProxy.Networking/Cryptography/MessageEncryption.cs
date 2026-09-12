using System.Security.Cryptography;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Cryptography;

internal sealed class MessageEncryption(RemotePeerRole with, Memory<byte> localPrivateKey, Memory<byte> sessionKey, Memory<byte> remotePublicKey = default)
{
    private const string EncryptionNotSetMessage = "Encryption is not set up.";
    private const int PromonPadSize = 508;
    private const string ReceiveNonceNotSetMessage = "Receive nonce is not set.";
    private readonly Encryption _encryption = new(with, localPrivateKey, sessionKey, remotePublicKey);

    internal MemoryStream Decrypt(MemoryStream memoryStream)
    {
        if (_encryption is null)
            throw new InvalidOperationException(EncryptionNotSetMessage);

        byte[] payload = GetPayloadBuffer(memoryStream);

        if (!_encryption.SharedKey.IsEmpty)
        {
            if (_encryption.ReceiveNonce is null)
                throw new InvalidOperationException(ReceiveNonceNotSetMessage);

            _encryption.ReceiveNonce.Increment();

            return new MemoryStream(NaClV3Cryptography.SecretBoxOpen(payload, _encryption.ReceiveNonce.Span, _encryption.SharedKey.Span), writable: false);
        }

        return _encryption.With is RemotePeerRole.Server
            ? DecryptClientboundHandshake(_encryption, payload)
            : DecryptServerboundHandshake(_encryption, payload);
    }

    internal MemoryStream Encrypt(MemoryStream memoryStream)
    {
        if (_encryption is null)
            throw new InvalidOperationException(EncryptionNotSetMessage);

        byte[] payload = GetPayloadBuffer(memoryStream);

        if (!_encryption.SharedKey.IsEmpty)
        {
            if (_encryption.SendNonce is null)
                throw new InvalidOperationException(message: "Send nonce is not set.");

            _encryption.SendNonce.Increment();

            return new MemoryStream(NaClV3Cryptography.SecretBox(payload, _encryption.SendNonce.Span, _encryption.SharedKey.Span), writable: false);
        }

        return _encryption.With is RemotePeerRole.Server
            ? EncryptServerboundHandshake(_encryption, payload)
            : EncryptClientboundHandshake(_encryption, payload);
    }

    private static MemoryStream DecryptClientboundHandshake(Encryption encryption, byte[] payload)
    {
        encryption.SendNonce ??= new Nonce();

        Nonce handshakeNonce = new(
            nonceBytes: encryption.SendNonce.Span,
            clientPublicKey: encryption.LocalPublicKey.Span,
            serverPublicKey: encryption.RemotePublicKey.Span
        );

        byte[] plaintext = NaClV3Cryptography.BoxOpen(payload, handshakeNonce.Span, encryption.RemotePublicKey.Span, encryption.LocalPrivateKey.Span);

        encryption.ReceiveNonce = new Nonce(nonceBytes: plaintext.AsSpan(..24));
        encryption.SharedKey = plaintext.AsMemory(24..56);

        return new MemoryStream(plaintext, index: 56, count: plaintext.Length - 56, writable: false);
    }

    private static MemoryStream DecryptServerboundHandshake(Encryption encryption, byte[] payload)
    {
        Memory<byte> payloadMemory = payload.AsMemory();

        encryption.RemotePublicKey = payloadMemory[..32];
        Memory<byte> ciphertext = payloadMemory[32..];

        Nonce temporaryNonce = new(clientPublicKey: encryption.RemotePublicKey.Span, serverPublicKey: encryption.LocalPublicKey.Span);

        byte[] plaintext = NaClV3Cryptography.BoxOpen(ciphertext.Span, temporaryNonce.Span, encryption.RemotePublicKey.Span, encryption.LocalPrivateKey.Span);

        Memory<byte> plaintextMemory = plaintext.AsMemory();

        Memory<byte> receivedSessionKey = plaintextMemory[..encryption.SessionKey.Length];

        if (!receivedSessionKey.Span.SequenceEqual(encryption.SessionKey.Span))
            throw new InvalidOperationException(message: "Received session key does not match the expected session key.");

        int payloadOffset = receivedSessionKey.Length;
        Memory<byte> serverNonce = plaintextMemory[payloadOffset..(payloadOffset + 24)];
        encryption.ReceiveNonce = new Nonce(nonceBytes: serverNonce.Span);

        int payloadStart = payloadOffset + 24;
        int payloadLength = plaintext.Length - payloadStart;

        if (payloadLength >= PromonPadSize && plaintextMemory.Span[^PromonPadSize..].SequenceEqual(stackalloc byte[PromonPadSize]))
            payloadLength -= PromonPadSize;

        return new MemoryStream(plaintext, index: payloadStart, count: payloadLength, writable: false);
    }

    private static MemoryStream EncryptClientboundHandshake(Encryption encryption, byte[] payload)
    {
        if (encryption.ReceiveNonce is null)
            throw new InvalidOperationException(ReceiveNonceNotSetMessage);

        encryption.SharedKey = RandomNumberGenerator.GetBytes(count: 32);
        encryption.SendNonce = new Nonce(nonceBytes: RandomNumberGenerator.GetBytes(count: 24));

        Nonce handshakeNonce = new(
            nonceBytes: encryption.ReceiveNonce.Span,
            clientPublicKey: encryption.RemotePublicKey.Span,
            serverPublicKey: encryption.LocalPublicKey.Span
        );

        byte[] ciphertext = NaClV3Cryptography.Box(
            [.. encryption.SendNonce.Span, .. encryption.SharedKey.Span, .. payload],
            handshakeNonce.Span,
            encryption.RemotePublicKey.Span,
            encryption.LocalPrivateKey.Span
        );

        return new MemoryStream(ciphertext, writable: false);
    }

    private static MemoryStream EncryptServerboundHandshake(Encryption encryption, byte[] payload)
    {
        encryption.SendNonce ??= new Nonce();

        if (encryption.RemotePublicKey.IsEmpty)
            throw new InvalidOperationException(message: "Remote public key is not set.");

        Nonce temporaryNonce = new(clientPublicKey: encryption.LocalPublicKey.Span, serverPublicKey: encryption.RemotePublicKey.Span);

        byte[] ciphertext = NaClV3Cryptography.Box(
            [
                .. encryption.SessionKey.Span,
                .. encryption.SendNonce.Span,
                .. payload,
                .. stackalloc byte[PromonPadSize],
            ],
            temporaryNonce.Span,
            encryption.RemotePublicKey.Span,
            encryption.LocalPrivateKey.Span
        );

        return new MemoryStream([.. encryption.LocalPublicKey.Span, .. ciphertext], writable: false);
    }

    private static byte[] GetPayloadBuffer(MemoryStream stream)
    {
        if (!stream.TryGetBuffer(out ArraySegment<byte> segment) || segment.Array is not { } buffer)
            return stream.ToArray();

        bool coversBuffer = segment.Offset == 0 && segment.Count == buffer.Length;

        return coversBuffer ? buffer : stream.ToArray();
    }

    private sealed class Encryption(RemotePeerRole with, Memory<byte> localPrivateKey, Memory<byte> sessionKey, Memory<byte> remotePublicKey = default)
    {
        public RemotePeerRole With { get; init; } = with;

        public Memory<byte> LocalPrivateKey { get; init; } = localPrivateKey;

        public Nonce? ReceiveNonce { get; set; }
        public Memory<byte> LocalPublicKey { get; init; } =
            NaClV3Cryptography.CryptoScalarMultBase(localPrivateKey.Span);
        public Nonce? SendNonce { get; set; }
        public Memory<byte> RemotePublicKey { get; set; } = remotePublicKey;
        public Memory<byte> SharedKey { get; set; }

        public Memory<byte> SessionKey { get; init; } = sessionKey;
    }

}
