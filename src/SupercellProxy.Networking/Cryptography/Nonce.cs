using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using Blake2Fast;
using Blake2Fast.Implementation;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// The 24-byte counter each direction of an encrypted connection seals its messages with. It is set up during the handshake and
/// advanced after every message, so both sides must process messages in the same order to stay in step.
/// </summary>
public sealed class Nonce
{
    private NonceBuffer _bytes;

    /// <summary>
    /// Builds a nonce in one of three ways: copies <paramref name="nonceBytes"/> as given; picks random bytes with the lowest bit
    /// cleared when nothing is given; or, when public keys are given, hashes the optional bytes and both keys with Blake2b, which is
    /// how the handshake derives its one-off nonces.
    /// </summary>
    /// <param name="nonceBytes">Exact 24 bytes to use, or extra input for the handshake hash.</param>
    /// <param name="clientPublicKey">The client's 32-byte public key; supplying it selects the hashed form.</param>
    /// <param name="serverPublicKey">The server's 32-byte public key, hashed after the client's.</param>
    /// <exception cref="InvalidOperationException">The Blake2b hash could not be produced.</exception>
    public Nonce(
        ReadOnlySpan<byte> nonceBytes = default,
        ReadOnlySpan<byte> clientPublicKey = default,
        ReadOnlySpan<byte> serverPublicKey = default
    )
    {
        if (Unsafe.IsNullRef(ref MemoryMarshal.GetReference(clientPublicKey)))
        {
            bool isNonceProvided = !Unsafe.IsNullRef(ref MemoryMarshal.GetReference(nonceBytes));

            if (isNonceProvided)
            {
                nonceBytes.CopyTo(_bytes);
            }
            else
            {
                RandomNumberGenerator.Fill(_bytes);
                _bytes[0] &= 0xFE;
            }
        }
        else
        {
            const int digestLength = 24;

            Blake2bHashState blake2bHasher = Blake2b.CreateIncrementalHasher(digestLength);
            bool isNonceProvided = !Unsafe.IsNullRef(ref MemoryMarshal.GetReference(nonceBytes));

            if (isNonceProvided)
                blake2bHasher.Update(nonceBytes);

            blake2bHasher.Update(clientPublicKey);
            blake2bHasher.Update(serverPublicKey);

            if (!blake2bHasher.TryFinish(_bytes, out int length))
                throw new InvalidOperationException(message: "Failed to compute nonce hash.");

            if (length is not digestLength)
                throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"Unexpected nonce hash length: {length}."));
        }
    }

    /// <summary>
    /// The current 24 bytes, writable in place; pass them to the box functions.
    /// </summary>
    public Span<byte> Span => _bytes;

    /// <summary>
    /// Advances the nonce as a little-endian number, once for every message sealed or opened with it.
    /// </summary>
    /// <param name="carryValue">Amount to add; the protocol always steps by 2.</param>
    /// <exception cref="OverflowException">All 24 bytes wrapped around.</exception>
    public void Increment(int carryValue = 2)
    {
        Span<byte> span = Span;

        for (int currentIndex = 0; currentIndex < span.Length; currentIndex++)
        {
            int currentSum = span[currentIndex] + carryValue;
            span[currentIndex] = byte.CreateTruncating(currentSum);
            carryValue = currentSum >> 8;

            if (carryValue is 0)
                return;
        }

        throw new OverflowException();
    }
}
