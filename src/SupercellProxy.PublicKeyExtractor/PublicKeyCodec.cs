using System.Runtime.InteropServices;

namespace SupercellProxy.PublicKeyExtractor;

/// <summary>
/// <para>Encodes and decodes the native server-public-key representation.</para>
/// </summary>
internal static class PublicKeyCodec
{
    /// <summary>
    /// <para>Decodes a native key table into a 32-byte public key.</para>
    /// </summary>
    public static Span<byte> Decode(ReadOnlySpan<byte> input)
    {
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

    /// <summary>
    /// <para>Encodes a 32-byte public key into the native key table.</para>
    /// </summary>
    public static Span<byte> Encode(ReadOnlySpan<byte> input)
    {
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
