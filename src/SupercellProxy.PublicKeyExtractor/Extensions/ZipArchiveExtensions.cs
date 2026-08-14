using System.IO.Compression;

namespace SupercellProxy.PublicKeyExtractor.Extensions;

public static class ZipArchiveExtensions
{
    public static bool HasZipArchiveHeader(this ReadOnlySpan<byte> source)
    {
        if (source.Length < 4)
            return false;

        if (source[0] != 0x50 || source[1] != 0x4B)
            return false;

        if (source[2] == 0x03 && source[3] == 0x04)
            return true;

        if (source[2] == 0x05 && source[3] == 0x06)
            return true;

        if (source[2] == 0x07 && source[3] == 0x08)
            return true;

        return false;
    }

    public static byte[] GetZipArchiveFileEntry(this byte[] source, string fileName)
    {
        using var zipStream = new MemoryStream(source, writable: false);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: false);

        var match = archive.Entries.Where(entry => !string.IsNullOrEmpty(entry.Name)).FirstOrDefault(entry => string.Equals(entry.Name, fileName, StringComparison.OrdinalIgnoreCase))
            ?? throw new FileNotFoundException($"Entry named '{fileName}' was not found in the ZIP archive.");

        if (match.Length > int.MaxValue)
            throw new IOException("Entry is too large to fit in a single byte array.");

        using var entryStream = match.Open();
        using var resultStream = new MemoryStream(capacity: (int)Math.Min(match.Length, int.MaxValue));

        entryStream.CopyTo(resultStream);
        return resultStream.ToArray();
    }

    public static byte[] GetIpaAppEntry(this byte[] source)
    {
        using var zipStream = new MemoryStream(source, writable: false);
        return zipStream.GetIpaAppEntry();
    }

    public static byte[] GetIpaAppEntry(this Stream source)
    {
        ArgumentNullException.ThrowIfNull(source);

        using var archive = new ZipArchive(source, ZipArchiveMode.Read, leaveOpen: true);

        var match = archive.Entries.FirstOrDefault(entry =>
        {
            var fullName = entry.FullName;

            if (!fullName.StartsWith("Payload/", StringComparison.OrdinalIgnoreCase))
                return false;

            var lastSlashIndex = fullName.LastIndexOf('/');

            if (lastSlashIndex < 0)
                return false;

            var appIndex = fullName.LastIndexOf(".app/", lastSlashIndex, StringComparison.OrdinalIgnoreCase);

            if (appIndex < 0)
                return false;

            var parentSlashIndex = fullName.LastIndexOf('/', appIndex - 1);

            if (parentSlashIndex < 0)
                return false;

            var expectedExecutableName = fullName[(parentSlashIndex + 1)..appIndex];
            return string.Equals(entry.Name, expectedExecutableName, StringComparison.OrdinalIgnoreCase);
        })
        ?? throw new FileNotFoundException("Main app executable was not found in the IPA file (expected Payload/<App>.app/<App>).");

        if (match.Length > int.MaxValue)
            throw new IOException("Entry is too large to fit in a single byte array.");

        using var entryStream = match.Open();
        using var resultStream = new MemoryStream(capacity: (int)match.Length);

        entryStream.CopyTo(resultStream);
        return resultStream.ToArray();
    }
}
