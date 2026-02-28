using Blake2Fast;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SupercellProxy.Playground.Crypto;

public class Nonce
{
    [InlineArray(24)]
    private struct NonceBuffer
    {
        private byte _element0;
    }

    private NonceBuffer _bytes;

    public Span<byte> Span => _bytes;

    public Nonce(ReadOnlySpan<byte> nonceBytes = default, ReadOnlySpan<byte> clientPublicKey = default, ReadOnlySpan<byte> serverPublicKey = default)
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
                throw new InvalidOperationException($"Unexpected nonce hash length: {length}.");
        }
    }

    public void Increment(int carryValue = 2)
    {
        var span = Span;

        for (var currentIndex = 0; currentIndex < span.Length; currentIndex++)
        {
            var currentSum = span[currentIndex] + carryValue;
            span[currentIndex] = (byte)currentSum;
            carryValue = currentSum >> 8;

            if (carryValue is 0)
                return;
        }

        throw new OverflowException();
    }
}
