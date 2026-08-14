using SupercellProxy.PublicKeyExtractor.Extensions;

namespace SupercellProxy.PublicKeyExtractor;

public static class ServerPublicKeyExtractor
{
    public static byte[] Extract(byte[] content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var binary = content.HasZipArchiveHeader()
            ? content.GetIpaAppEntry()
            : content;

        return ExtractBinary(binary);
    }

    public static byte[] ExtractFile(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        using var stream = File.OpenRead(path);
        Span<byte> header = stackalloc byte[4];
        var headerLength = stream.Read(header);
        stream.Position = 0;

        var binary = header[..headerLength].HasZipArchiveHeader()
            ? stream.GetIpaAppEntry()
            : ReadAllBytes(stream);

        return ExtractBinary(binary);
    }

    public static byte[] ExtractBinary(ReadOnlySpan<byte> binary)
    {
        const int KeyLength = 128;
        const int ZeroesBeforeKey = 64;

        var foundIndex = -1;

        foreach (var index in binary.IndexesOf([0x1A, 0xD5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]))
        {
            if (index < KeyLength + ZeroesBeforeKey ||
                !binary.SliceBefore(index - KeyLength, ZeroesBeforeKey).IsAllZeros())
            {
                continue;
            }

            if (foundIndex is not -1)
            {
                throw new InvalidOperationException(
                    $"Multiple possible server public keys found in the binary (expected 1):\n" +
                    $"[{foundIndex}]:{Convert.ToHexString(binary.SliceBefore(foundIndex, KeyLength))}\n" +
                    $"[{index}]:{Convert.ToHexString(binary.SliceBefore(index, KeyLength))}");
            }

            foundIndex = index;
        }

        if (foundIndex is -1)
            throw new InvalidOperationException("Could not find server public key in the binary.");

        return PublicKeyCodec.Decode(binary.SliceBefore(foundIndex, KeyLength)).ToArray();
    }

    private static byte[] ReadAllBytes(Stream stream)
    {
        if (stream.Length > int.MaxValue)
            throw new IOException("Input is too large to fit in a single byte array.");

        using var destination = new MemoryStream(capacity: (int)stream.Length);
        stream.CopyTo(destination);
        return destination.ToArray();
    }
}
