using System.Runtime.InteropServices;

namespace SupercellProxy.PublicKeyExtractor;

public static class PublicKeyCodec
{
    public static Span<byte> Decode(ReadOnlySpan<byte> input)
    {
        var inputWords = MemoryMarshal.Cast<byte, ushort>(input);
        var outputWords = new ushort[16];

        for (int outputIndex = 0, aIndex = 0, bIndex = 1, cIndex = 63, dIndex = 63; outputIndex < 16; outputIndex++, aIndex += 2, bIndex += 2, cIndex -= 2, dIndex -= 1)
        {
            var wordA = inputWords[aIndex];
            var wordB = inputWords[bIndex];
            var wordC = inputWords[cIndex];
            var wordD = inputWords[dIndex];

            var x = (ushort)(((wordB ^ wordC) | (wordC ^ wordA)) & 0xFFFF);
            var rotationCount = 11 - (outputIndex & 7);
            var rotatedValue = (ushort)((x << rotationCount) | (x >> (16 - rotationCount)));

            outputWords[outputIndex] = (ushort)(rotatedValue ^ wordD);
        }

        return MemoryMarshal.AsBytes(outputWords.AsSpan());
    }

    public static Span<byte> Encode(ReadOnlySpan<byte> input)
    {
        var inputWords = MemoryMarshal.Cast<byte, ushort>(input);
        var outputWords = new ushort[64];

        for (int inputIndex = 0, aIndex = 0, cIndex = 63; inputIndex < 16; inputIndex++, aIndex += 2, cIndex -= 2)
        {
            var rotationCount = 11 - (inputIndex & 7);
            var rotatedValue = (ushort)((inputWords[inputIndex] >> rotationCount) | (inputWords[inputIndex] << (16 - rotationCount)));
            outputWords[aIndex] = rotatedValue;
            // b, c and d indexes are left as zeroes
            // should work anyway
        }

        return MemoryMarshal.AsBytes(outputWords.AsSpan());
    }
}
