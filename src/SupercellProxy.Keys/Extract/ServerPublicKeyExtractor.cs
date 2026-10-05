using System.Globalization;

using SupercellProxy.Keys.Extract.Extensions;

namespace SupercellProxy.Keys.Extract;

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
    /// <para>Extracts a server public key from a native executable image.</para>
    /// </summary>
    public static byte[] ExtractBinary(ReadOnlySpan<byte> binary)
    {
        int foundIndex = -1;

        foreach (int index in binary.IndexesOf(PublicKeyCodec.TableAnchor))
        {
            bool invalid = index < PublicKeyCodec.EncodedLength + PublicKeyCodec.ZeroPrefixLength
                || !binary.SliceBefore(index - PublicKeyCodec.EncodedLength, PublicKeyCodec.ZeroPrefixLength).IsAllZeros();

            if (invalid)
                continue;

            if (foundIndex is not -1)
            {
                throw new InvalidOperationException(
                    "Multiple possible server public keys found in the binary (expected 1):\n"
                        + string.Create(
                            CultureInfo.InvariantCulture,
                            $"[{foundIndex}]:{Convert.ToHexString(binary.SliceBefore(foundIndex, PublicKeyCodec.EncodedLength))}\n"
                        )
                        + string.Create(CultureInfo.InvariantCulture, $"[{index}]:{Convert.ToHexString(binary.SliceBefore(index, PublicKeyCodec.EncodedLength))}")
                );
            }

            foundIndex = index;
        }

        return foundIndex is -1
            ? throw new InvalidOperationException(message: "Could not find server public key in the binary.")
            : PublicKeyCodec.Decode(binary.SliceBefore(foundIndex, PublicKeyCodec.EncodedLength)).ToArray();
    }

    /// <summary>
    /// <para>Extracts a server public key from a local file.</para>
    /// </summary>
    public static async ValueTask<byte[]> ExtractFileAsync(string path, CancellationToken cancellationToken = default)
    {
        byte[] binary = await ReadBinaryFileAsync(path, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return ExtractBinary(binary);
    }

    /// <summary>Extracts the public key and, when recognized, its numeric version.</summary>
    public static async ValueTask<ExtractedServerKey> ExtractKeyFileAsync(string path, CancellationToken cancellationToken = default)
    {
        byte[] binary = await ReadBinaryFileAsync(path, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return new ExtractedServerKey(KeyVersionExtractor.Extract(binary), Convert.ToHexString(ExtractBinary(binary)));
    }

    private static async ValueTask<byte[]> ReadBinaryFileAsync(string path, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        byte[] content = await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        ReadOnlyMemory<byte> contentMemory = content;

        return content.HasZipArchiveHeader()
            ? await contentMemory.GetIpaAppEntryAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)
            : content;
    }
}
