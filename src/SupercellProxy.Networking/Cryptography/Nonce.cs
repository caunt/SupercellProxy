using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using Blake2Fast;
using Blake2Fast.Implementation;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// Represents <c language="csharp">Nonce</c>.
/// </summary>
public sealed class Nonce
{
    private NonceBuffer _bytes;

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
    /// Gets the <c language="csharp">Span</c> value.
    /// </summary>
    public Span<byte> Span => _bytes;

    /// <summary>
    /// Executes the <c language="csharp">Increment</c> operation.
    /// </summary>
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
