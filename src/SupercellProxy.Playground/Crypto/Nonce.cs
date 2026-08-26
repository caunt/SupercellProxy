using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Blake2Fast;

namespace SupercellProxy.Playground.Crypto;

/// <summary>
/// Represents <c>Nonce</c>.
/// </summary>
public class Nonce
{
    private NonceBuffer _bytes;

    /// <summary>
    /// Gets the <c>Span</c> value.
    /// </summary>
    public Span<byte> Span => _bytes;

    /// <summary>
    /// Initializes a new <see cref="Nonce"/> instance.
    /// </summary>
    public Nonce(
        ReadOnlySpan<byte> nonceBytes = default,
        ReadOnlySpan<byte> clientPublicKey = default,
        ReadOnlySpan<byte> serverPublicKey = default
    )
    {
        if (Unsafe.IsNullRef(ref MemoryMarshal.GetReference(clientPublicKey)))
        {
            var isNonceProvided = !Unsafe.IsNullRef(ref MemoryMarshal.GetReference(nonceBytes));

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
            const int DigestLength = 24;

            var blake2bHasher = Blake2b.CreateIncrementalHasher(DigestLength);
            var isNonceProvided = !Unsafe.IsNullRef(ref MemoryMarshal.GetReference(nonceBytes));

            if (isNonceProvided)
                blake2bHasher.Update(nonceBytes);

            blake2bHasher.Update(clientPublicKey);
            blake2bHasher.Update(serverPublicKey);

            if (!blake2bHasher.TryFinish(_bytes, out var length))
                throw new InvalidOperationException("Failed to compute nonce hash.");

            if (length is not DigestLength)
                throw new InvalidOperationException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Unexpected nonce hash length: {length}."
                    )
                );
        }
    }

    /// <summary>
    /// Executes the <c>Increment</c> operation.
    /// </summary>
    public void Increment(int carryValue = 2)
    {
        var span = Span;

        for (var currentIndex = 0; currentIndex < span.Length; currentIndex++)
        {
            var currentSum = span[currentIndex] + carryValue;
            span[currentIndex] = byte.CreateTruncating(currentSum);
            carryValue = currentSum >> 8;

            if (carryValue is 0)
                return;
        }

        throw new OverflowException();
    }
}
