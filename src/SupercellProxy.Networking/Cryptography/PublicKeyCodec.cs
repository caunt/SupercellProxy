using System.Runtime.InteropServices;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>Encodes and decodes the native server-public-key representation.</summary>
public static class PublicKeyCodec
{
    /// <summary>Length of a decoded public key.</summary>
    public const int DecodedLength = 32;

    /// <summary>Length of the native encoded key table.</summary>
    public const int EncodedLength = 128;

    /// <summary>Zero-filled prefix used to locate the native key table.</summary>
    public const int ZeroPrefixLength = 64;

    /// <summary>Gets the marker following the native key table.</summary>
    /// <value>The eight bytes <c>1A D5 00 00 00 00 00 00</c> that the key extractor searches the game binary for; the key table is the 128 bytes just before them, preceded by 64 zero bytes.</value>
    public static ReadOnlySpan<byte> TableAnchor => [0x1a, 0xd5, 0, 0, 0, 0, 0, 0];

    /// <summary>Decodes a native key table into a 32-byte public key.</summary>
    /// <param name="input">The obfuscated key table as stored in the game binary, read as little-endian 16-bit words; it must be exactly <see cref="EncodedLength"/> (128) bytes.</param>
    /// <returns>A new 32-byte span holding the server's raw public key.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="input"/> is not exactly 128 bytes long.</exception>
    public static Span<byte> Decode(ReadOnlySpan<byte> input)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(input.Length, EncodedLength, nameof(input));
        ReadOnlySpan<ushort> inputWords = MemoryMarshal.Cast<byte, ushort>(input);
        ushort[] outputWords = new ushort[16];

        for (
            int outputIndex = 0, aIndex = 0, bIndex = 1, cIndex = 63, dIndex = 63; outputIndex < 16;
            outputIndex++, aIndex += 2, bIndex += 2, cIndex -= 2, dIndex -= 1
        )
        {
            ushort wordA = inputWords[aIndex];
            ushort wordB = inputWords[bIndex];
            ushort wordC = inputWords[cIndex];
            ushort wordD = inputWords[dIndex];

            ushort horizontalCoordinate = ushort.CreateTruncating(((wordB ^ wordC) | (wordC ^ wordA)) & 0xFFFF);
            int rotationCount = 11 - (outputIndex & 7);

            ushort rotatedValue = ushort.CreateTruncating((horizontalCoordinate << rotationCount) | (horizontalCoordinate >> (16 - rotationCount)));

            outputWords[outputIndex] = ushort.CreateTruncating(rotatedValue ^ wordD);
        }

        return MemoryMarshal.AsBytes(outputWords.AsSpan());
    }

    /// <summary>Encodes a 32-byte public key into the native key table.</summary>
    /// <param name="input">The raw public key; it must be exactly <see cref="DecodedLength"/> (32) bytes.</param>
    /// <returns>A new 128-byte table in which only the 16-bit words 0, 2, …, 30 carry the rotated key words and the rest stay zero, which the client accepts; <see cref="Decode"/> turns it back into <paramref name="input"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="input"/> is not exactly 32 bytes long.</exception>
    public static Span<byte> Encode(ReadOnlySpan<byte> input)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(input.Length, DecodedLength, nameof(input));
        ReadOnlySpan<ushort> inputWords = MemoryMarshal.Cast<byte, ushort>(input);
        ushort[] outputWords = new ushort[64];

        for (
            int inputIndex = 0, aIndex = 0, cIndex = 63; inputIndex < 16;
            inputIndex++, aIndex += 2, cIndex -= 2
        )
        {
            int rotationCount = 11 - (inputIndex & 7);

            ushort rotatedValue = ushort.CreateTruncating((inputWords[inputIndex] >> rotationCount) | (inputWords[inputIndex] << (16 - rotationCount)));

            outputWords[aIndex] = rotatedValue;
            // b, c and d indexes are left as zeroes
            // works anyway, verified with Hay Day client
        }

        return MemoryMarshal.AsBytes(outputWords.AsSpan());
    }
}
