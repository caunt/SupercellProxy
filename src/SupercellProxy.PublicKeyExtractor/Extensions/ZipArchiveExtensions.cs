using System.IO.Compression;

namespace SupercellProxy.PublicKeyExtractor.Extensions;

/// <summary>
/// <para>Provides ZIP and IPA archive inspection helpers.</para>
/// </summary>
internal static class ZipArchiveExtensions
{

    /// <summary>
    /// <para>Reads the primary application executable from IPA bytes.</para>
    /// </summary>
    public static async ValueTask<byte[]> GetIpaAppEntryAsync(this ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
    {
        MemoryStream zipStream = new(source.ToArray(), writable: false);

        await using (zipStream.ConfigureAwait(continueOnCapturedContext: false))
            return await zipStream.GetIpaAppEntryAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>
    /// <para>Reads the primary application executable from an IPA stream.</para>
    /// </summary>
    public static async ValueTask<byte[]> GetIpaAppEntryAsync(this Stream source, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);

        ZipArchive archive = new(source, ZipArchiveMode.Read, leaveOpen: true);

        await using (archive.ConfigureAwait(continueOnCapturedContext: false))
        {
            ZipArchiveEntry match =
                archive.Entries.FirstOrDefault(
                    static entry =>
                {
                    string fullName = entry.FullName;

                    if (!fullName.StartsWith(value: "Payload/", StringComparison.OrdinalIgnoreCase))
                        return false;

                    int lastSlashIndex = fullName.LastIndexOf(value: '/');

                    if (lastSlashIndex < 0)
                        return false;

                    int appIndex = fullName.LastIndexOf(value: ".app/", lastSlashIndex, StringComparison.OrdinalIgnoreCase);

                    if (appIndex < 0)
                        return false;

                    int parentSlashIndex = fullName.LastIndexOf(value: '/', appIndex - 1);

                    if (parentSlashIndex < 0)
                        return false;

                    string expectedExecutableName = fullName[(parentSlashIndex + 1)..appIndex];

                    return string.Equals(entry.Name, expectedExecutableName, StringComparison.OrdinalIgnoreCase);
                }
                )
                ?? throw new FileNotFoundException(message: "Main app executable was not found in the IPA file (expected Payload/<App>.app/<App>).");

            if (match.Length > int.MaxValue)
                throw new IOException(message: "Entry is too large to fit in a single byte array.");

            Stream entryStream = await match.OpenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            await using (entryStream.ConfigureAwait(continueOnCapturedContext: false))
            {
                MemoryStream resultStream = new(capacity: int.CreateTruncating(match.Length));

                await using (resultStream.ConfigureAwait(continueOnCapturedContext: false))
                {
                    await entryStream
                        .CopyToAsync(resultStream, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    return resultStream.ToArray();
                }
            }
        }
    }

    /// <summary>
    /// <para>Reads a named file from a ZIP archive.</para>
    /// </summary>
    public static async ValueTask<byte[]> GetZipArchiveFileEntryAsync(this ReadOnlyMemory<byte> source, string fileName, CancellationToken cancellationToken = default)
    {
        MemoryStream zipStream = new(source.ToArray(), writable: false);

        await using (zipStream.ConfigureAwait(continueOnCapturedContext: false))
        {
            ZipArchive archive = new(zipStream, ZipArchiveMode.Read, leaveOpen: false);

            await using (archive.ConfigureAwait(continueOnCapturedContext: false))
            {
                ZipArchiveEntry match =
                    archive.Entries.FirstOrDefault(entry => !string.IsNullOrEmpty(entry.Name) && string.Equals(entry.Name, fileName, StringComparison.OrdinalIgnoreCase))
                    ?? throw new FileNotFoundException($"Entry named '{fileName}' was not found in the ZIP archive.");

                if (match.Length > int.MaxValue)
                    throw new IOException(message: "Entry is too large to fit in a single byte array.");

                Stream entryStream = await match.OpenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

                await using (entryStream.ConfigureAwait(continueOnCapturedContext: false))
                {
                    MemoryStream resultStream = new(capacity: int.CreateTruncating(Math.Min(match.Length, int.MaxValue)));

                    await using (resultStream.ConfigureAwait(continueOnCapturedContext: false))
                    {
                        await entryStream
                            .CopyToAsync(resultStream, cancellationToken)
                            .ConfigureAwait(continueOnCapturedContext: false);

                        return resultStream.ToArray();
                    }
                }
            }
        }
    }

    /// <summary>
    /// <para>Determines whether the input begins with a recognized ZIP header.</para>
    /// </summary>
    public static bool HasZipArchiveHeader(this ReadOnlySpan<byte> source)
    {
        return source.Length >= 4 && source[index: 0] is 0x50 && source[index: 1] is 0x4B && ((source[index: 2] is 0x03 && source[index: 3] is 0x04) || (source[index: 2] is 0x05 && source[index: 3] is 0x06) || (source[index: 2] is 0x07 && source[index: 3] is 0x08));
    }
}
