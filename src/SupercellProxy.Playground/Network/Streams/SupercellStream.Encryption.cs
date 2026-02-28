using SupercellProxy.Playground.Crypto;
using SupercellProxy.Playground.Supercell;
using System.Security.Cryptography;

namespace SupercellProxy.Playground.Network.Streams;

public partial class SupercellStream
{
    private record Encryption(Memory<byte> ClientPrivateKey, Memory<byte> ClientPublicKey, Memory<byte> ServerPublicKey, Memory<byte> SessionKey)
    {
        public Nonce DecryptNonce { get; init; } = new Nonce();
        public Nonce Nonce { get; init; } = new Nonce(clientPublicKey: ClientPublicKey.Span, serverPublicKey: ServerPublicKey.Span);
        public Memory<byte> SharedKey { get; set; }
        public Nonce? EncryptNonce { get; set; }

        public Encryption(Memory<byte> ClientPrivateKey, Memory<byte> ServerPublicKey, Memory<byte> SessionKey) : this(ClientPrivateKey, NaClV3Crypto.CryptoScalarMultBase(ClientPrivateKey.Span), ServerPublicKey, SessionKey)
        {
            // Empty
        }
    }

    private Encryption? _encryption;

    public async ValueTask SetupEncryptionAsync(Memory<byte> sessionKey, CancellationToken cancellationToken = default)
    {
        if (_encryption is not null)
            throw new InvalidOperationException("Encryption is already set up.");

        _encryption = new Encryption(
            ClientPrivateKey: RandomNumberGenerator.GetBytes(count: 32),
            ServerPublicKey: await HayDayApi.GetServerPublicKeyAsync(cancellationToken),
            SessionKey: sessionKey
        );
    }

    private MemoryStream Encrypt(MemoryStream memoryStream)
    {
        if (_encryption is null)
            throw new InvalidOperationException("Encryption is not set up.");

        var payload = memoryStream.ToArray();

        if (_encryption.SharedKey.IsEmpty)
        {
            var ciphertext = NaClV3Crypto.Box(
            [
                .. _encryption.SessionKey.Span,
                .. _encryption.DecryptNonce.Span,
                .. payload,
                .. stackalloc byte[508]
            ], _encryption.Nonce.Span, _encryption.ServerPublicKey.Span, _encryption.ClientPrivateKey.Span);

            memoryStream = new MemoryStream([.. _encryption.ClientPublicKey.Span, .. ciphertext], writable: false);
        }
        else
        {
            _encryption.DecryptNonce.Increment();
            var ciphertext = NaClV3Crypto.SecretBox(payload, _encryption.DecryptNonce.Span, _encryption.SharedKey.Span);

            memoryStream = new MemoryStream(ciphertext, writable: false);
        }

        return memoryStream;
    }

    private MemoryStream Decrypt(MemoryStream memoryStream)
    {
        if (_encryption is null)
            throw new InvalidOperationException("Encryption is not set up.");

        var payload = memoryStream.ToArray();

        if (_encryption.SharedKey.IsEmpty || _encryption.EncryptNonce is null)
        {
            var nonce = new Nonce(_encryption.DecryptNonce.Span, _encryption.ClientPublicKey.Span, _encryption.ServerPublicKey.Span);
            var plaintext = NaClV3Crypto.BoxOpen(payload, nonce.Span, _encryption.ServerPublicKey.Span, _encryption.ClientPrivateKey.Span);

            _encryption.EncryptNonce = new Nonce(nonceBytes: plaintext.AsSpan(..24));
            _encryption.SharedKey = plaintext.AsMemory(24..56);

            memoryStream = new MemoryStream(plaintext, 56, plaintext.Length - 56, writable: false);
        }
        else
        {
            _encryption.EncryptNonce.Increment();
            var ciphertext = NaClV3Crypto.SecretBoxOpen(payload, _encryption.EncryptNonce.Span, _encryption.SharedKey.Span);

            memoryStream = new MemoryStream(ciphertext, writable: false);
        }

        return memoryStream;
    }
}
