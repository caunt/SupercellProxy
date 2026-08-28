using System.IO.Compression;

namespace SupercellProxy.PublicKeyExtractor.Extensions;

/// <summary>
/// <para>Provides ZIP and IPA archive inspection helpers.</para>
/// </summary>
internal static class ZipArchiveExtensions
{
    /// <summary>
    /// <para>Determines whether the input begins with a recognized ZIP header.</para>
    /// </summary>
    public static bool HasZipArchiveHeader(this ReadOnlySpan<byte> source)
    {
        if (source.Length < 4)
            return false;

        if (source[0] is not 0x50 || source[1] is not 0x4B)
            return false;

        if (source[2] is 0x03 && source[3] is 0x04)
            return true;

        if (source[2] is 0x05 && source[3] is 0x06)
            return true;

        if (source[2] is 0x07 && source[3] is 0x08)
            return true;

        return false;
    }

    /// <summary>
    /// <para>Reads a named file from a ZIP archive.</para>
    /// </summary>
    public static async ValueTask<byte[]> GetZipArchiveFileEntryAsync(
        this ReadOnlyMemory<byte> source,
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        var zipStream = new MemoryStream(source.ToArray(), writable: false);
        await using (zipStream.ConfigureAwait(false))
        {
            var archive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false);
            await using (archive.ConfigureAwait(false))
            {
                var match =
                    archive.Entries.FirstOrDefault(entry =>
                        !string.IsNullOrEmpty(entry.Name)
                        && string.Equals(entry.Name, fileName, StringComparison.OrdinalIgnoreCase)
                    )
                    ?? throw new FileNotFoundException(
                        $"Entry named '{fileName}' was not found in the ZIP archive."
                    );

                if (match.Length > int.MaxValue)
                    throw new IOException("Entry is too large to fit in a single byte array.");

                var entryStream = await match.OpenAsync(cancellationToken).ConfigureAwait(false);
                await using (entryStream.ConfigureAwait(false))
                {
                    var resultStream = new MemoryStream(
                        capacity: int.CreateTruncating(Math.Min(match.Length, int.MaxValue))
                    );
                    await using (resultStream.ConfigureAwait(false))
                    {
                        await entryStream
                            .CopyToAsync(resultStream, cancellationToken)
                            .ConfigureAwait(false);
                        return resultStream.ToArray();
                    }
                }
            }
        }
    }

    /// <summary>
    /// <para>Reads the primary application executable from IPA bytes.</para>
    /// </summary>
    public static async ValueTask<byte[]> GetIpaAppEntryAsync(
        this ReadOnlyMemory<byte> source,
        CancellationToken cancellationToken = default
    )
    {
        var zipStream = new MemoryStream(source.ToArray(), writable: false);
        await using (zipStream.ConfigureAwait(false))
        {
            return await zipStream.GetIpaAppEntryAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// <para>Reads the primary application executable from an IPA stream.</para>
    /// </summary>
    public static async ValueTask<byte[]> GetIpaAppEntryAsync(
        this Stream source,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(source);

        var archive = new ZipArchive(source, ZipArchiveMode.Read, leaveOpen: true);
        await using (archive.ConfigureAwait(false))
        {
            var match =
                archive.Entries.FirstOrDefault(static entry =>
                {
                    var fullName = entry.FullName;

                    if (!fullName.StartsWith("Payload/", StringComparison.OrdinalIgnoreCase))
                        return false;

                    var lastSlashIndex = fullName.LastIndexOf('/');

                    if (lastSlashIndex < 0)
                        return false;

                    var appIndex = fullName.LastIndexOf(
                        ".app/",
                        lastSlashIndex,
                        StringComparison.OrdinalIgnoreCase
                    );

                    if (appIndex < 0)
                        return false;

                    var parentSlashIndex = fullName.LastIndexOf('/', appIndex - 1);

                    if (parentSlashIndex < 0)
                        return false;

                    var expectedExecutableName = fullName[(parentSlashIndex + 1)..appIndex];
                    return string.Equals(
                        entry.Name,
                        expectedExecutableName,
                        StringComparison.OrdinalIgnoreCase
                    );
                })
                ?? throw new FileNotFoundException(
                    "Main app executable was not found in the IPA file (expected Payload/<App>.app/<App>)."
                );

            if (match.Length > int.MaxValue)
                throw new IOException("Entry is too large to fit in a single byte array.");

            var entryStream = await match.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using (entryStream.ConfigureAwait(false))
            {
                var resultStream = new MemoryStream(capacity: int.CreateTruncating(match.Length));
                await using (resultStream.ConfigureAwait(false))
                {
                    await entryStream
                        .CopyToAsync(resultStream, cancellationToken)
                        .ConfigureAwait(false);
                    return resultStream.ToArray();
                }
            }
        }
    }
}
