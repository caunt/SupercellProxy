using System.Globalization;
using SupercellProxy.PublicKeyExtractor.Extensions;

namespace SupercellProxy.PublicKeyExtractor;

/// <summary>
/// <para>Extracts an encoded server public key from client binaries and packages.</para>
/// </summary>
internal static class ServerPublicKeyExtractor
{
    /// <summary>
    /// <para>Extracts a server public key from raw native executable bytes.</para>
    /// </summary>
    public static byte[] Extract(ReadOnlySpan<byte> content)
    {
        return ExtractBinary(content);
    }

    /// <summary>
    /// <para>Extracts a server public key from a local file.</para>
    /// </summary>
    public static async ValueTask<byte[]> ExtractFileAsync(
        string path,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        var content = await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(false);
        ReadOnlyMemory<byte> contentMemory = content;
        var binary = content.HasZipArchiveHeader()
            ? await contentMemory.GetIpaAppEntryAsync(cancellationToken).ConfigureAwait(false)
            : content;
        return ExtractBinary(binary);
    }

    /// <summary>
    /// <para>Extracts a server public key from a native executable image.</para>
    /// </summary>
    public static byte[] ExtractBinary(ReadOnlySpan<byte> binary)
    {
        const int keyLength = 128;
        const int zeroesBeforeKey = 64;

        var foundIndex = -1;

        foreach (var index in binary.IndexesOf([0x1A, 0xD5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]))
        {
            if (
                index < keyLength + zeroesBeforeKey
                || !binary.SliceBefore(index - keyLength, zeroesBeforeKey).IsAllZeros()
            )
            {
                continue;
            }

            if (foundIndex is not -1)
            {
                throw new InvalidOperationException(
                    "Multiple possible server public keys found in the binary (expected 1):\n"
                        + string.Create(
                            CultureInfo.InvariantCulture,
                            $"[{foundIndex}]:{Convert.ToHexString(binary.SliceBefore(foundIndex, keyLength))}\n"
                        )
                        + string.Create(
                            CultureInfo.InvariantCulture,
                            $"[{index}]:{Convert.ToHexString(binary.SliceBefore(index, keyLength))}"
                        )
                );
            }

            foundIndex = index;
        }

        if (foundIndex is -1)
            throw new InvalidOperationException("Could not find server public key in the binary.");

        return PublicKeyCodec.Decode(binary.SliceBefore(foundIndex, keyLength)).ToArray();
    }
}
