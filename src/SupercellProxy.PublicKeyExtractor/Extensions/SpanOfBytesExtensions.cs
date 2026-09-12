using System.Numerics;

namespace SupercellProxy.PublicKeyExtractor.Extensions;

/// <summary>
/// <para>Provides byte-span search and validation helpers.</para>
/// </summary>
internal static class SpanOfBytesExtensions
{

    /// <summary>
    /// <para>Finds all overlapping occurrences of a byte pattern.</para>
    /// </summary>
    public static IEnumerable<int> IndexesOf(this ReadOnlySpan<byte> source, ReadOnlySpan<byte> pattern)
    {
        if (pattern.Length is 0)
            return [];

        List<int> result = new(capacity: 2);
        int start = 0;

        while (start <= source.Length - pattern.Length)
        {
            int index = source[start..].IndexOf(pattern);

            if (index < 0)
                break;

            index += start;
            start = index + 1;

            result.Add(index);
        }

        return result;
    }

    /// <summary>
    /// <para>Determines whether every byte is zero.</para>
    /// </summary>
    public static bool IsAllZeros(this ReadOnlySpan<byte> input)
    {
        int index = 0;

        if (Vector.IsHardwareAccelerated && input.Length >= Vector<byte>.Count)
        {
            Vector<byte> zeroVector = Vector<byte>.Zero;
            int vectorSize = Vector<byte>.Count;
            int lastVectorStart = input.Length - (input.Length % vectorSize);

            while (index < lastVectorStart)
            {
                if (!Vector.EqualsAll(new Vector<byte>(input.Slice(index, vectorSize)), zeroVector))
                    return false;

                index += vectorSize;
            }
        }

        while (index + sizeof(ulong) <= input.Length)
        {
            if (BitConverter.ToUInt64(input.Slice(index, sizeof(ulong))) is not 0UL)
                return false;

            index += sizeof(ulong);
        }

        while (index < input.Length)
        {
            if (input[index] is not 0)
                return false;

            index++;
        }

        return true;
    }

    /// <summary>
    /// <para>Returns a slice immediately preceding the specified index.</para>
    /// </summary>
    public static ReadOnlySpan<byte> SliceBefore(this ReadOnlySpan<byte> input, int index, int count)
    {
        return input.Slice(index - count, count);
    }
}
