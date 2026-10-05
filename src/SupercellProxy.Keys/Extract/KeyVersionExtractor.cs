using System.Buffers.Binary;

using SupercellProxy.Keys.Extract.Extensions;

namespace SupercellProxy.Keys.Extract;

/// <summary>Reads the key-version integer immediately before its native data anchor.</summary>
internal static class KeyVersionExtractor
{
    private static readonly byte[][] Anchors =
    [
        Convert.FromHexString(s: "00000000000048430000C8420000000000000000"),
        Convert.FromHexString(s: "00000000010000000000000000000000010000000000000000000000000048430000C842FFFFFFFF"),
    ];

    public static int? Extract(ReadOnlySpan<byte> binary)
    {
        int? version = null;

        foreach (byte[] anchor in Anchors)
        {
            foreach (int index in binary.IndexesOf(anchor))
            {
                if (index < sizeof(int))
                    continue;

                int candidate = BinaryPrimitives.ReadInt32LittleEndian(binary.SliceBefore(index, sizeof(int)));

                if (candidate <= 0)
                    continue;

                if (version is not null)
                    throw new InvalidOperationException(message: "Multiple possible key versions found in the binary (expected 1).");

                version = candidate;
            }
        }

        return version;
    }
}
